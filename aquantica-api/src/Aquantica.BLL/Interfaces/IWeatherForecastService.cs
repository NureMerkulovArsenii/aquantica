using Aquantica.Contracts.Requests.Weather;
using Aquantica.Contracts.Responses.Weather;
using Aquantica.Core.DTOs.Weather;
using Aquantica.Core.ServiceResult;

namespace Aquantica.BLL.Interfaces;

public interface IWeatherForecastService
{
    Task<List<WeatherResponse>> GetWeatherAsync(GetWeatherRequest request);
    Task<ServiceResult<bool>> GetWeatherForecastsFromApiAsync(int? sectionId = null);
    
}