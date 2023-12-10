using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
using Aquantica.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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

            if (res == null)
                return NotFound("Setting not found".ToApiErrorResponse());

            return Ok(res.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }

    [HttpGet("number/{id}")]
    public async Task<IActionResult> GetNumberSettingAsync(int id)
    {
        try
        {
            var res = await _settingsService.GetNumberSettingAsync(id);
            return Ok(res.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }

    [HttpGet("string/{id}")]
    public async Task<IActionResult> GetStringSettingAsync(int id)
    {
        try
        {
            var res = await _settingsService.GetStringSettingAsync(id);
            return Ok(res.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllSettingsAsync()
    {
        try
        {
            var res = await _settingsService.GetAllSettingsAsync();
            return Ok(res.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateSettingAsync([FromBody] SetSettingRequest request)
    {
        try
        {
            var res = await _settingsService.CreateSettingAsync(request);
            return Ok(res.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateSettingAsync([FromBody] SetSettingRequest request)
    {
        try
        {
            var res = await _settingsService.UpdateSettingAsync(request);
            return Ok(res.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteSettingAsync(int id)
    {
        try
        {
            var res = await _settingsService.DeleteSettingAsync(id);
            return Ok(res.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }
}