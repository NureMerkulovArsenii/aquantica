using System.Globalization;
using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Requests.Weather;
using Aquantica.Contracts.Responses.Weather;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WeatherForecastService> _logger;

    public WeatherForecastService(
        ISectionService sectionService,
        IHttpService httpService,
        IUnitOfWork unitOfWork,
        ILogger<WeatherForecastService> logger)
    {
        _sectionService = sectionService;
        _httpService = httpService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<WeatherResponse>> GetWeatherAsync(GetWeatherRequest request)
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
            var rootSection = await _sectionService.GetRootSection();
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
                .OrderByDescending(x => x.Time)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            weather = weather.Where(x => x.WeatherRecordId == recordId);
        }
        else
        {
            weather = weather.Where(x => x.Time >= request.TimeFrom && x.Time <= request.TimeTo);
        }

        var result = await weather
            .Select(x => new WeatherResponse()
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

    public void GetWeatherForecastsFromApi(int? sectionId = null)
    {
        try
        {
            Task.Run(async () => await GetWeatherForecastsFromApiAsync(sectionId));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }


    public async Task<ServiceResult<bool>> GetWeatherForecastsFromApiAsync(int? sectionId = null)
    {
        try
        {
            IrrigationSection section;

            if (sectionId == null)
            {
                var rootSection = await _sectionService.GetRootSection();
                section = rootSection.Data;
            }
            else
            {
                section = await _unitOfWork.SectionRepository
                    .GetAllByCondition(x => x.Id == sectionId)
                    .FirstOrDefaultAsync();
            }

            var url = BuildUrl(section);

            var weatherForecast = await _httpService.GetAsync<WeatherDTO>(url);

            var numberOfRecords = weatherForecast.Hourly.Time.Count;

            await using var transaction = await _unitOfWork.CreateTransactionAsync();

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

            await _unitOfWork.WeatherRecordRepository.AddAsync(weatherRecord);
            await _unitOfWork.WeatherForecastRepository.AddRangeAsync(forecasts);
            await _unitOfWork.CommitTransactionAsync();
            await _unitOfWork.SaveAsync();

            return new ServiceResult<bool>(true);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ServiceResult<bool>(e.Message);
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