using Aquantica.API.Filters;
using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
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

            if (result.IsSuccess)
                return Ok(result.Data.ToApiListResponse());

            return BadRequest(result.ErrorMessage?.ToApiErrorResponse());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message.ToApiErrorResponse());
        }
    }
}
