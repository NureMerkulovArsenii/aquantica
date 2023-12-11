using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Requests.Weather;
using Aquantica.Core.DTOs;
using Aquantica.Core.DTOs.Irrigation;
using Aquantica.Core.DTOs.Ruleset;
using Aquantica.Core.DTOs.Section;
using Aquantica.Core.Entities;
using Aquantica.Core.ServiceResult;
using Aquantica.DAL.UnitOfWork;
using Microsoft.Extensions.Logging;
using BackgroundJob = Hangfire.BackgroundJob;

namespace Aquantica.BLL.Services;

public class ArduinoControllersService : IArduinoControllersService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpService _httpService;
    private readonly IRuleSetService _ruleSetService;
    private readonly ISectionService _sectionService;
    private readonly IJobHelperService _jobHelperService;
    private readonly ISettingsService _settingsService;
    private readonly IWeatherForecastService _weatherForecastService;
    private readonly ILogger<ArduinoControllersService> _logger;

    public ArduinoControllersService(
        IUnitOfWork unitOfWork,
        IHttpService httpService,
        IRuleSetService ruleSetService,
        ISectionService sectionService,
        IJobHelperService jobHelperService,
        ISettingsService settingsService,
        IWeatherForecastService weatherForecastService,
        ILogger<ArduinoControllersService> logger)
    {
        _unitOfWork = unitOfWork;
        _httpService = httpService;
        _ruleSetService = ruleSetService;
        _sectionService = sectionService;
        _jobHelperService = jobHelperService;
        _settingsService = settingsService;
        _weatherForecastService = weatherForecastService;
        _logger = logger;
    }

    public void StartIrrigationIfNeeded(BackgroundJobDTO job)
    {
        try
        {
            _jobHelperService.AddJobEventRecord(job, true);

            if (job.IrrigationSectionId == null)
                return;

            var ruleSet = _ruleSetService.GetRuleSetBySectionId(job.IrrigationSectionId.Value);

            if (ruleSet == null)
            {
                _logger.LogError($"RuleSet for section {job.IrrigationSectionId} not found");
                return;
            }

            if (ruleSet.IsEnabled == false)
            {
                _logger.LogInformation($"RuleSet for section {job.IrrigationSectionId} is disabled");
                return;
            }

            var result = ShouldIrrigationStart(job, ruleSet);

            if (result.IsSuccess == false)
            {
                _logger.LogError(result.ErrorMessage);
                return;
            }

            var durationInMinutes = result.Data.Duration;

            var section = result.Data.SectionDto;

            var url = $"http://{section.DeviceUri}/start-irrigation/{durationInMinutes}/{ruleSet.OptimalSoilHumidity}";

            var response = _httpService.Get<ArduinoResponseDTO>(url);

            if (response == null || response.IsSuccess == false)
            {
                _logger.LogError($"Irrigation for section {job.IrrigationSectionId} failed");
                return;
            }

            BackgroundJob.Schedule(() => StopIrrigation(job), TimeSpan.FromSeconds(durationInMinutes * 60 + 10));

            _sectionService.CreateIrrigationEvent(new IrrigationEventDTO
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddMinutes(durationInMinutes),
                WaterUsed = 0,
                SectionId = job.IrrigationSectionId.Value,
                IrrigationRulesetId = ruleSet.Id,
                IsStopped = false,
            });

            _logger.LogInformation($"Irrigation for section {job.IrrigationSectionId} started");
            _jobHelperService.AddJobEventRecord(job, false);
        }
        catch (Exception e)
        {
            try
            {
                _jobHelperService.AddJobEventRecord(job, false, true);
                _logger.LogError(e.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }
        }
    }


    public void StopIrrigation(BackgroundJobDTO job)
    {
        try
        {
            _jobHelperService.AddJobEventRecord(job, true);

            if (job.IrrigationSectionId == null)
                return;

            var section = _sectionService.GetSectionById(job.IrrigationSectionId.Value);

            if (section == null)
            {
                _jobHelperService.AddJobEventRecord(job, false, true);
                return;
            }

            var irrigationEvents = _sectionService.GetOngoingIrrigationEventBySectionId(section.Id);

            if (irrigationEvents == null)
            {
                _jobHelperService.AddJobEventRecord(job, false, true);
                return;
            }

            foreach (var irrigationEvent in irrigationEvents)
            {
                var isTimeToStop = DateTime.Now >= irrigationEvent.EndTime;

                if (isTimeToStop == false)
                {
                    continue;
                }

                var url = $"http://{section.DeviceUri}/stop-irrigation";

                var response = _httpService.Get<ArduinoResponseDTO>(url);

                if (response == null || response.IsSuccess == false)
                {
                    _logger.LogError($"Irrigation for section {job.IrrigationSectionId} failed");
                }

                irrigationEvent.IsStopped = true;

                _sectionService.UpdateIrrigationEvent(irrigationEvent);
            }

            _jobHelperService.AddJobEventRecord(job, false);
        }
        catch (Exception e)
        {
            try
            {
                _jobHelperService.AddJobEventRecord(job, false, true);
                _logger.LogError(e.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }
        }
    }

    public void GetControllerData(BackgroundJobDTO job)
    {
        try
        {
            _jobHelperService.AddJobEventRecord(job, true);

            var section = _sectionService.GetSectionById(job.IrrigationSectionId.Value);

            if (section == null)
            {
                _jobHelperService.AddJobEventRecord(job, false, true);
                return;
            }

            var url = $"http://{section.DeviceUri}/get-data";

            var response = _httpService.Get<SensorDataDTO>(url);

            if (response == null)
            {
                _jobHelperService.AddJobEventRecord(job, false, true);
                return;
            }

            WriteSensorData(section, response, job.Id);

            _jobHelperService.AddJobEventRecord(job, false);
        }
        catch (Exception e)
        {
            try
            {
                _jobHelperService.AddJobEventRecord(job, false, true);
                _logger.LogError(e.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }
        }
    }

    private ServiceResult<StartWateringCommandDTO> ShouldIrrigationStart(BackgroundJobDTO job,
        RuleSetDetailedDTO ruleSet)
    {
        try
        {
            var sectionRecentSensorData = _unitOfWork.SensorDataRepository
                .GetAllByCondition(x => x.IrrigationSectionId == job.IrrigationSectionId)
                .OrderByDescending(x => x.DateCreated)
                .FirstOrDefault();

            if (sectionRecentSensorData == null)
                throw new Exception("Sensor data for section not found");

            if (sectionRecentSensorData.Humidity >= ruleSet.MinSoilHumidityThreshold)
                throw new Exception($"Humidity for section {job.IrrigationSectionId} is more than threshold");

            if (sectionRecentSensorData.Temperature <= ruleSet.TemperatureThreshold)
                throw new Exception($"Temperature for section {job.IrrigationSectionId} is less than threshold");

            if (sectionRecentSensorData.Humidity >= ruleSet.OptimalSoilHumidity)
                throw new Exception($"Humidity for section {job.IrrigationSectionId} is more than threshold");

            var section = _sectionService.GetSectionById(job.IrrigationSectionId.Value);

            if (section == null)
                throw new Exception($"IrrigationSection {job.IrrigationSectionId} not found");

            int? locationId = null;

            if (section.LocationId == null)
            {
                var rootSection = _sectionService.GetRootSection();
                if (rootSection.IsSuccess && rootSection.Data.LocationId != null)
                    locationId = rootSection.Data.LocationId.Value;
            }
            else
            {
                locationId = section.LocationId.Value;
            }

            if (locationId == null)
                throw new Exception($"Location for section {job.IrrigationSectionId} not found");

            var isWeatherForecastEnabled = _settingsService.GetBoolSetting("IS_WEATHER_FORECAST_ENABLED");

            double durationInMinutes = 0;

            if (isWeatherForecastEnabled.Value)
            {
                var durationResult = GetDecisionBasedOnWeatherForecast(ruleSet, locationId.Value);
                if (durationResult.IsSuccess == false)
                    throw new Exception("Error while getting decision based on weather forecast");

                if (durationResult.Data == 0)
                    throw new Exception("Weather forecast decision is not to irrigate");

                durationInMinutes = durationResult.Data;
            }
            else
            {
                var humidityDifference = ruleSet.OptimalSoilHumidity - sectionRecentSensorData.Humidity;

                var calculatedAmountOfWater = humidityDifference * ruleSet.HumidityGrowthPerLiterConsumed;

                durationInMinutes = calculatedAmountOfWater / ruleSet.WaterConsumptionPerMinute;

                if (durationInMinutes > ruleSet.MaxIrrigationDuration.TotalMinutes &&
                    calculatedAmountOfWater > ruleSet.WaterConsumptionThreshold)
                    throw new Exception("Calculated duration is more than max irrigation duration");
            }

            var duration = Convert.ToInt32(durationInMinutes);

            return new ServiceResult<StartWateringCommandDTO>(new StartWateringCommandDTO
            {
                Duration = duration,
                SectionDto = section
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return new ServiceResult<StartWateringCommandDTO>(e.Message);
        }
    }


    private ServiceResult<double> GetDecisionBasedOnWeatherForecast(RuleSetDetailedDTO ruleSet, int locationId)
    {
        try
        {
            var weatherForecast = _weatherForecastService.GetWeather(new GetWeatherRequest
            {
                IsForecast = true,
                IsFromRecentRecord = true,
                LocationId = locationId,
            });

            if (weatherForecast.IsSuccess == false)
            {
                _logger.LogError($"Weather forecast for location {locationId} not found");
                return new ServiceResult<double>("Weather forecast for location not found");
            }

            //if in forecast rain probability is more than threshold within rain avoidance threshold
            if (ruleSet.RainAvoidanceEnabled)
            {
                var rainAvoidanceTime = DateTime.Now.Add(ruleSet.RainAvoidanceDurationThreshold);

                var isRainProbabilityMoreThanThreshold = weatherForecast.Data
                    .Where(x => x.Time >= DateTime.Now && x.Time <= rainAvoidanceTime)
                    .Any(x => x.PrecipitationProbability >= ruleSet.RainProbabilityThreshold &&
                              x.Precipitation >= ruleSet.RainAmountThreshold);

                if (isRainProbabilityMoreThanThreshold)
                {
                    _logger.LogInformation($"Rain probability for location {locationId} is more than threshold");
                    return new ServiceResult<double>("Rain probability for location is more than threshold");
                }
            }

            var rainAmount = weatherForecast.Data
                .Where(x => x.Time >= DateTime.Now && x.Time <= DateTime.Now.Add(ruleSet.MaxIrrigationDuration))
                .Sum(x => x.Precipitation);

            var calculatedSoilHumidity = rainAmount * ruleSet.HumidityGrowthPerRainMm;

            var humidityDifference = ruleSet.OptimalSoilHumidity - calculatedSoilHumidity;

            var calculatedAmountOfWater = humidityDifference * ruleSet.HumidityGrowthPerLiterConsumed;

            var duration = calculatedAmountOfWater / ruleSet.WaterConsumptionPerMinute;

            if (duration > ruleSet.MaxIrrigationDuration.TotalMinutes &&
                calculatedAmountOfWater > ruleSet.WaterConsumptionThreshold)
            {
                return new ServiceResult<double>(0);
            }

            return new ServiceResult<double>(duration);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return new ServiceResult<double>(e.Message);
        }
    }

    private void WriteSensorData(IrrigationSectionDTO section, SensorDataDTO data, int jobId)
    {
        var sensorData = new SensorData
        {
            Humidity = data.Humidity,
            Temperature = data.Temperature,
            IrrigationSectionId = section.Id,
            BackgroundJobId = jobId,
        };

        _unitOfWork.SensorDataRepository.Add(sensorData);

        _unitOfWork.Save();
    }
}