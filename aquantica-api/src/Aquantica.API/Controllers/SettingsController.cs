using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
using Aquantica.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SettingsController : ControllerBase
{
    private readonly ISettingsService _settingsService;

    public SettingsController(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }


    [HttpGet("bool/{id}")]
    public async Task<IActionResult> GetBoolSettingAsync(int id)
    {
        try
        {
            var res = await _settingsService.GetBoolSettingAsync(id);
            return Ok(res);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToErrorResponse());
        }
    }

    [HttpGet("number/{id}")]
    public async Task<IActionResult> GetNumberSettingAsync(int id)
    {
        try
        {
            var res = await _settingsService.GetNumberSettingAsync(id);
            return Ok(res);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToErrorResponse());
        }
    }

    [HttpGet("string/{id}")]
    public async Task<IActionResult> GetStringSettingAsync(int id)
    {
        try
        {
            var res = await _settingsService.GetStringSettingAsync(id);
            return Ok(res);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToErrorResponse());
        }
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllSettingsAsync()
    {
        try
        {
            var res = await _settingsService.GetAllSettingsAsync();
            return Ok(res);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToErrorResponse());
        }
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateSettingAsync([FromBody] SetSettingRequest request)
    {
        try
        {
            var res = await _settingsService.CreateSettingAsync(request);
            return Ok(res);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToErrorResponse());
        }
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> UpdateSettingAsync([FromBody] SetSettingRequest request)
    {
        try
        {
            var res = await _settingsService.UpdateSettingAsync(request);
            return Ok(res);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToErrorResponse());
        }
    }
}