using System.Globalization;
using Aquantica.BLL.Interfaces;
using Aquantica.Core.DTOs;
using Aquantica.Core.DTOs.Weather;
using Aquantica.Core.Entities;
using Aquantica.Core.Enums;
using Aquantica.Core.ServiceResult;
using Aquantica.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Aquantica.BLL.Services;



public class WeatherForecastService : IWeatherForecastService
{
    private readonly ISectionService _sectionService;
    private readonly IHttpService _httpService;
    private readonly IUnitOfWork _unitOfWork;

    public WeatherForecastService(
        ISectionService sectionService,
        IHttpService httpService,
        IUnitOfWork unitOfWork)
    {
        _sectionService = sectionService;
        _httpService = httpService;
        _unitOfWork = unitOfWork;
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
            
            for (var i = 0; i < numberOfRecords ; i++)
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
