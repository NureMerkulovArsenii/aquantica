using Aquantica.Contracts.Requests.Weather;
using Aquantica.Contracts.Responses.Weather;
using Aquantica.Core.Entities;
using Aquantica.Core.ServiceResult;

namespace Aquantica.BLL.Interfaces;

public interface IWeatherForecastService
{
    Task<List<WeatherResponse>> GetWeatherAsync(GetWeatherRequest request);
    
    ServiceResult<List<WeatherResponse>> GetWeather(GetWeatherRequest request);
    
    ServiceResult<bool> GetWeatherForecastsFromApi(BackgroundJob job);
}