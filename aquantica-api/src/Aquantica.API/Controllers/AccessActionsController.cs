using Aquantica.API.Filters;
using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
using Aquantica.Contracts.Requests.AccessActions;
using Aquantica.Core.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[CustomJwtAuthorize(Roles = "Admin")]
public class AccessActionsController : Controller
{
    private readonly IAccessActionService _accessActionService;

    public AccessActionsController(IAccessActionService accessActionService)
    {
        _accessActionService = accessActionService;
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetAccessActions()
    {
        try
        {
            var result = await _accessActionService.GetAccessActionsAsync();

            return Ok(result.ToApiListResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_GET_ACCESS_ACTIONS").ToApiErrorResponse());
        }
    }
    
    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetAccessActionById(int id)
    {
        try
        {
            var result = await _accessActionService.GetAccessActionByIdAsync(id);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_GET_ACCESS_ACTION").ToApiErrorResponse());
        }
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateAccessAction([FromBody] CreateAccessActionRequest request)
    {
        try
        {
            var result = await _accessActionService.CreateAccessActionAsync(request);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_CREATE_ACCESS_ACTION").ToApiErrorResponse());
        }
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> UpdateAccessAction([FromBody] UpdateAccessActionRequest request)
    {
        try
        {
            var result = await _accessActionService.UpdateAccessActionAsync(request);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_UPDATE_ACCESS_ACTION").ToApiErrorResponse());
        }
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteAccessAction(int id)
    {
        try
        {
            var result = await _accessActionService.DeleteAccessActionAsync(id);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_DELETE_ACCESS_ACTION").ToApiErrorResponse());
        }
    }
    
}