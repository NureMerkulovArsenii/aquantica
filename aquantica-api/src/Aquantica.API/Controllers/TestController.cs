using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World!");
    }

}
