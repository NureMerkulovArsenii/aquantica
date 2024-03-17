using System.Globalization;
using Aquantica.BLL.Interfaces;
using Aquantica.BLL.Proxies;
using Aquantica.Contracts.Requests.Weather;
using Aquantica.Core.DTOs;
using Aquantica.Core.DTOs.Weather;
using Aquantica.Core.Entities;
using Aquantica.Core.ServiceResult;
using Aquantica.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Aquantica.BLL.Services;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly ISectionService _sectionService;
    private readonly IHttpService _httpService;
    private readonly IWeatherAPIProxyService _weatherApiProxyService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJobHelperService _jobHelperService;
    private readonly ILogger<WeatherForecastService> _logger;

    public WeatherForecastService(
        ISectionService sectionService,
        IHttpService httpService,
        IWeatherAPIProxyService weatherApiProxyService,
        IJobHelperService jobHelperService,
        IUnitOfWork unitOfWork,
        ILogger<WeatherForecastService> logger)
    {
        _sectionService = sectionService;
        _httpService = httpService;
        _weatherApiProxyService = weatherApiProxyService;
        _unitOfWork = unitOfWork;
        _jobHelperService = jobHelperService;
        _logger = logger;
    }

    public async Task<List<WeatherDTO>> GetWeatherAsync(GetWeatherRequest request)
    {
        int? locationId = null;

        if (request.LocationId != null && request.LocationId != 0)
        {
            var location = await _unitOfWork.LocationRepository
                .GetAllByCondition(x => x.Id == request.LocationId)
                .FirstOrDefaultAsync();
            if (location != null)
                locationId = location.Id;
        }
        else
        {
            var rootSection = _sectionService.GetRootSection();
            if (rootSection?.Data.LocationId != null)
                locationId = rootSection.Data.LocationId.Value;
        }

        if (locationId == null)
            throw new Exception("Location not found");

        var weather = _unitOfWork.WeatherForecastRepository
            .GetAllByCondition(x =>
                x.WeatherRecord.IsForecast == request.IsForecast
                && x.LocationId == locationId);

        if (request.IsFromRecentRecord)
        {
            var recordId = await _unitOfWork.WeatherRecordRepository
                .GetAllByCondition(x => x.IsForecast == request.IsForecast)
                .Where(x => x.Time >= request.TimeFrom && x.Time <= request.TimeTo)
                .OrderByDescending(x => x.Time)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            weather = weather.Where(x => x.WeatherRecordId == recordId);
        }
        else
        {
            weather = weather.Where(x => x.Time >= request.TimeFrom && x.Time <= request.TimeTo).Distinct();
        }

        var result = await weather
            .Select(x => new WeatherDTO()
            {
                Id = x.Id,
                Time = x.Time,
                Temperature = x.Temperature,
                RelativeHumidity = x.RelativeHumidity,
                PrecipitationProbability = x.PrecipitationProbability,
                Precipitation = x.Precipitation,
                SoilMoisture = x.SoilMoisture,
                LocationId = x.LocationId,
                WeatherRecordId = x.WeatherRecordId,
            })
            .ToListAsync();

        return result;
    }

    public ServiceResult<List<WeatherDTO>> GetWeather(GetWeatherRequest request)
    {
        try
        {
            int? locationId = null;

            if (request.LocationId != null && request.LocationId != 0)
            {
                var location = _unitOfWork.LocationRepository
                    .GetAllByCondition(x => x.Id == request.LocationId)
                    .FirstOrDefault();
                if (location != null)
                    locationId = location.Id;
            }
            else
            {
                var rootSection = _sectionService.GetRootSection();
                if (rootSection?.Data.LocationId != null)
                    locationId = rootSection.Data.LocationId.Value;
            }

            if (locationId == null)
                throw new Exception("Location not found");

            var weather = _unitOfWork.WeatherForecastRepository
                .GetAllByCondition(x =>
                    x.WeatherRecord.IsForecast == request.IsForecast
                    && x.LocationId == locationId);

            if (request.IsFromRecentRecord)
            {
                var recordId = _unitOfWork.WeatherRecordRepository
                    .GetAllByCondition(x => x.IsForecast == request.IsForecast)
                    .OrderByDescending(x => x.Time)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                weather = weather.Where(x => x.WeatherRecordId == recordId);
            }
            else
            {
                weather = weather.Where(x => x.Time >= request.TimeFrom && x.Time <= request.TimeTo);
            }

            var result = weather
                .Select(x => new WeatherDTO()
                {
                    Id = x.Id,
                    Time = x.Time,
                    Temperature = x.Temperature,
                    RelativeHumidity = x.RelativeHumidity,
                    PrecipitationProbability = x.PrecipitationProbability,
                    Precipitation = x.Precipitation,
                    SoilMoisture = x.SoilMoisture,
                    LocationId = x.LocationId,
                    WeatherRecordId = x.WeatherRecordId,
                })
                .ToList();

            return new ServiceResult<List<WeatherDTO>>(result);
        }
        catch (Exception e)
        {
            return new ServiceResult<List<WeatherDTO>>(e.Message);
        }
    }


    public ServiceResult<bool> GetWeatherForecastsFromApi(BackgroundJobDTO job)
    {
        try
        {
            _jobHelperService.AddJobEventRecord(job, true);

            IrrigationSection section;

            if (job.IrrigationSectionId == null)
            {
                var rootSection = _sectionService.GetRootSection();
                section = rootSection.Data;
            }
            else
            {
                section = _unitOfWork.SectionRepository
                    .GetAllByCondition(x => x.Id == job.IrrigationSectionId)
                    .FirstOrDefault();
            }

            var url = BuildUrl(section);

            // _httpService.Get<WeatherFromApiDTO>(url);
            var weatherForecast = AsyncHelper.RunSync(() => _weatherApiProxyService.GetWeatherForecastAsync(section));

            var numberOfRecords = weatherForecast.Hourly.Time.Count;

            using var transaction = _unitOfWork.CreateTransaction();

            var weatherRecord = new WeatherRecord
            {
                Time = DateTime.Now,
                IsForecast = true,
            };

            var forecasts = new List<WeatherForecast>();

            for (var i = 0; i < numberOfRecords; i++)
            {
                var forecast = new WeatherForecast
                {
                    Time = ParseJsonDate(weatherForecast.Hourly.Time[i]),
                    Temperature = weatherForecast.Hourly.Temperature2m[i],
                    RelativeHumidity = weatherForecast.Hourly.RelativeHumidity2m[i],
                    PrecipitationProbability = weatherForecast.Hourly.PrecipitationProbability[i],
                    Precipitation = weatherForecast.Hourly.Precipitation[i],
                    SoilMoisture = weatherForecast.Hourly.SoilMoisture3To9cm[i],
                    Location = section.Location,
                    WeatherRecord = weatherRecord,
                };

                forecasts.Add(forecast);
            }

            _unitOfWork.WeatherRecordRepository.Add(weatherRecord);
            _unitOfWork.WeatherForecastRepository.AddRange(forecasts);
            _unitOfWork.CommitTransaction();
            _unitOfWork.Save();

            _jobHelperService.AddJobEventRecord(job, false);

            return new ServiceResult<bool>(true);
        }
        catch (Exception e)
        {
            try
            {
                _unitOfWork.RollbackTransaction();
                _jobHelperService.AddJobEventRecord(job, false, true);
                return new ServiceResult<bool>(e.Message);
            }
            catch (Exception exception)
            {
                return new ServiceResult<bool>(exception.Message);
            }
        }
    }

    private DateTime ParseJsonDate(string jsonDate)
    {
        var result = DateTime.ParseExact(jsonDate, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);

        return result;
    }

    private string BuildUrl(IrrigationSection section)
    {
        var url =
            @"https://api.open-meteo.com/v1/forecast?latitude={0}&longitude={1}&hourly=temperature_2m,relative_humidity_2m,precipitation_probability,precipitation,soil_moisture_3_to_9cm";
        return string.Format(url, section.Location.Latitude.ToString(CultureInfo.InvariantCulture),
            section.Location.Longitude.ToString(CultureInfo.InvariantCulture));
    }
}