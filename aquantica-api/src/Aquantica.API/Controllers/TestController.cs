using Aquantica.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IWeatherForecastService _weatherForecastService;

    public TestController(IWeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var s  = await _weatherForecastService.GetWeatherForecastsAsync();
        
        return Ok("Hello World!");
    }

}
