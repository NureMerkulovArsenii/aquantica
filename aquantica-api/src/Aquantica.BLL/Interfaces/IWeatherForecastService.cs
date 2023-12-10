using Aquantica.Contracts.Requests.Weather;
using Aquantica.Core.DTOs;
using Aquantica.Core.DTOs.Weather;
using Aquantica.Core.Entities;
using Aquantica.Core.ServiceResult;

namespace Aquantica.BLL.Interfaces;

public interface IWeatherForecastService
{
    Task<List<WeatherDTO>> GetWeatherAsync(GetWeatherRequest request);
    
    ServiceResult<List<WeatherDTO>> GetWeather(GetWeatherRequest request);
    
    ServiceResult<bool> GetWeatherForecastsFromApi(BackgroundJobDTO job);
}