using Aquantica.API.Filters;
using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
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
    [CustomJwtAuthorize(Roles = "Admin")]
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
}