using System.Globalization;
using Aquantica.BLL.Interfaces;
using Aquantica.Core.DTOs.Weather;
using Aquantica.Core.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Aquantica.BLL.Proxies;

public class WeatherAPIProxyService : IWeatherAPIProxyService
{
    private readonly IHttpService _httpClient;
    private readonly IMemoryCache _cache;

    public WeatherAPIProxyService(IMemoryCache memoryCache, IHttpService httpService)
    {
        _httpClient = httpService;
        _cache = memoryCache;
    }

    public async Task<WeatherFromApiDTO> GetWeatherForecastAsync(IrrigationSection section)
    {
        WeatherFromApiDTO weatherForecast = null;
        
        var cacheKey = $"WeatherForecast_{section.Id}";
        
        if (_cache.Get(cacheKey) is WeatherFromApiDTO cachedForecast)
        {
            weatherForecast =  cachedForecast;
        }
        else
        {
            var url = BuildUrl(section);
            var response = await _httpClient.GetAsync<WeatherFromApiDTO>(url);
            weatherForecast = response;

            // Cache the forecast for 6 hours
            _cache.Set(cacheKey, response, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(3)));
        }
        
        return weatherForecast;
    }

    private string BuildUrl(IrrigationSection section)
    {
        var url =
            @"https://api.open-meteo.com/v1/forecast?latitude={0}&longitude={1}&hourly=temperature_2m,relative_humidity_2m,precipitation_probability,precipitation,soil_moisture_3_to_9cm";
        return string.Format(url, section.Location.Latitude.ToString(CultureInfo.InvariantCulture),
            section.Location.Longitude.ToString(CultureInfo.InvariantCulture));
    }
    
}