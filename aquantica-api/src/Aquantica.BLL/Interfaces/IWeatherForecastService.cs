using Aquantica.Core.DTOs.Weather;
using Aquantica.Core.ServiceResult;

namespace Aquantica.BLL.Interfaces;

public interface IWeatherForecastService
{
    Task<ServiceResult<WeatherForecastDTO>> GetWeatherForecastsAsync(int? sectionId = null);
    
}