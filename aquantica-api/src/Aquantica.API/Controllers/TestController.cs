using Aquantica.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IWeatherForecastService _weatherForecastService;
    private readonly IJobControlService _jobControlService;

    public TestController(
        IWeatherForecastService weatherForecastService,
        IJobControlService jobControlService
    )
    {
        _weatherForecastService = weatherForecastService;
        _jobControlService = jobControlService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok("Hello world");
    }
}