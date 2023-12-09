using Aquantica.API.Filters;
using Aquantica.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IWeatherForecastService _weatherForecastService;
    private readonly IJobControlService _jobControlService;
    private readonly IHttpService _httpService;

    public TestController(
        IWeatherForecastService weatherForecastService,
        IJobControlService jobControlService,
        IHttpService httpService
    )
    {
        _weatherForecastService = weatherForecastService;
        _jobControlService = jobControlService;
        _httpService = httpService;
    }

    [HttpGet]
    [CustomJwtAuthorize]
    public async Task<IActionResult> Get()
    {
        return Ok("Hello world");
    }
}