using System.Globalization;
using Aquantica.BLL.Interfaces;
using Aquantica.Core.DTOs;
using Aquantica.Core.DTOs.Weather;
using Aquantica.Core.Enums;
using Aquantica.Core.ServiceResult;
using Aquantica.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Aquantica.BLL.Services;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly ISectionService _sectionService;
    private readonly IUnitOfWork _unitOfWork;

    public WeatherForecastService(ISectionService sectionService, IUnitOfWork unitOfWork)
    {
        _sectionService = sectionService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<WeatherForecastDTO>> GetWeatherForecastsAsync(int? sectionId = null)
    {
        try
        {
            var section = new SectionDTO();

            if (sectionId == null)
            {
                var rootSection = await _sectionService.GetRootSection();
                section = rootSection.Data;
            }
            else
            {
                section = await _unitOfWork.SectionRepository
                    .GetAllByCondition(x => x.Id == sectionId)
                    .Select(x => new SectionDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Number = x.Number,
                        ParentId = x.ParentId,
                        IsEnabled = x.IsEnabled,
                        Location = new LocationDto
                        {
                            Name = x.Location.Name,
                            Latitude = x.Location.Latitude,
                            Longitude = x.Location.Longitude
                        }
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }
            
            var url = BuildUrl(section);

            using var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            using (var response = await httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var weatherForecast = JsonConvert.DeserializeObject(body);
            }

            var result = new ServiceResult<WeatherForecastDTO>();
            return result;
        }
        catch (Exception e)
        {
            return new ServiceResult<WeatherForecastDTO>(e.Message);
        }
    }
    
    private string BuildUrl(SectionDTO section)
    {
        var url = @"https://api.open-meteo.com/v1/forecast?latitude={0}&longitude={1}&hourly=temperature_2m,relative_humidity_2m,precipitation_probability,precipitation,soil_moisture_3_to_9cm";
        return string.Format(url, section.Location.Latitude.ToString(CultureInfo.InvariantCulture), section.Location.Longitude.ToString(CultureInfo.InvariantCulture));
    }
    
    // private WeatherForecastDTO MapWeatherForecast(string json, ForecastType forecastType)
    // {
    //     switch (forecastType)
    //     {
    //         case ForecastType.Full:
    //             r
    //         default:
    //             throw new ArgumentOutOfRangeException(nameof(forecastType), forecastType, null);
    //     }
    //     
    //     
    // }
}