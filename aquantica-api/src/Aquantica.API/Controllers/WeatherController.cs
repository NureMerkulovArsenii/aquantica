using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
using Aquantica.Contracts.Requests.Weather;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
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
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToErrorResponse());
        }
    }
}
