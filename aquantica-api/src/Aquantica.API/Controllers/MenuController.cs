using Aquantica.API.Filters;
using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
using Aquantica.Contracts.Requests.Menu;
using Aquantica.Core.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet("get")]
    [CustomJwtAuthorize]
    public async Task<IActionResult> GetMenu()
    {
        try
        {
            var result = await _menuService.GetMenu();

            return Ok(result.Data.ToApiListResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_GET_MENU").ToApiErrorResponse());
        }
    }
    
    [HttpGet("get/{id}")]
    [CustomJwtAuthorize]
    public async Task<IActionResult> GetMenuById(int id)
    {
        try
        {
            var result = await _menuService.GetMenuById(id);

            return Ok(result.Data.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_GET_MENU").ToApiErrorResponse());
        }
    }
    
    [HttpPost("create")]
    [CustomJwtAuthorize]
    public async Task<IActionResult> CreateMenu([FromBody] CreateMenuRequest request)
    {
        try
        {
            var result = await _menuService.CreateMenu(request);

            return Ok(result.Data.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_CREATE_MENU").ToApiErrorResponse());
        }
    }
    
    [HttpPut("update")]
    [CustomJwtAuthorize]
    public async Task<IActionResult> UpdateMenu([FromBody] UpdateMenuRequest request)
    {
        try
        {
            var result = await _menuService.UpdateMenu(request);

            return Ok(result.Data.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_UPDATE_MENU").ToApiErrorResponse());
        }
    }
    
    [HttpDelete("delete/{id}")]
    [CustomJwtAuthorize]
    public async Task<IActionResult> DeleteMenu(int id)
    {
        try
        {
            var result = await _menuService.DeleteMenu(id);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_DELETE_MENU").ToApiErrorResponse());
        }
    }
    
    
}