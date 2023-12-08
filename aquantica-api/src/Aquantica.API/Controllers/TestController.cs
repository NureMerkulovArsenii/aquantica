using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Requests.JobControl;
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
        var s = await _weatherForecastService.GetWeatherForecastsFromApiAsync();

        return Ok("Hello World!");
    }

    [HttpGet("create-job")]
    public async Task<IActionResult> Test()
    {
        var request = new CreateJobRequest();

        var s = await _jobControlService.CreateJobAsync(request);

        return Ok("Hello World!");
    }
    
    [HttpGet("stop-job")]
    public async Task<IActionResult> StopJob()
    {
        var s = await _jobControlService.StopJobAsync(1);

        return Ok("Hello World!");
    }
}