using Aquantica.Core.DTOs.Weather;
using Aquantica.Core.Entities;

namespace Aquantica.BLL.Proxies;

public interface IWeatherAPIProxyService
{
    Task<WeatherFromApiDTO> GetWeatherForecastAsync(IrrigationSection section);
}