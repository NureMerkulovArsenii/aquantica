using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
using Aquantica.Contracts.Requests.Weather;
using Aquantica.Contracts.Responses.Weather;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WeatherController : Controller
{
    private readonly IWeatherForecastService _weatherService;

    public WeatherController(IWeatherForecastService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpPost("get")]
    public async Task<IActionResult> GetWeatherAsync(GetWeatherRequest request)
    {
        try
        {
            var result = await _weatherService.GetWeatherAsync(request);

            var response = result.Select(x => new WeatherResponse
            {
                Id = x.Id,
                Time = x.Time,
                Temperature = x.Temperature,
                RelativeHumidity = x.RelativeHumidity,
                PrecipitationProbability = x.PrecipitationProbability,
                Precipitation = x.Precipitation,
                SoilMoisture = x.SoilMoisture,
                LocationId = x.LocationId,
                WeatherRecordId = x.WeatherRecordId
            }).ToList();

            return Ok(response.ToApiListResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }
}