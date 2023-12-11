using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
using Aquantica.Contracts.Requests.Roles;
using Aquantica.Contracts.Responses.Roles;
using Aquantica.Core.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoleController : Controller
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllRoles()
    {
        try
        {
            var roles = await _roleService.GetAllRolesAsync();

            if (roles == null)
            {
                return NotFound(Resources.Get("ROLES_NOT_FOUND").ToApiErrorResponse());
            }

            var result = roles.Select(x => new RoleResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsEnabled = x.IsEnabled,
                IsBlocked = x.IsBlocked,
                IsDefault = x.IsDefault,
            });

            return Ok(result.ToApiListResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_GET_ROLES").ToApiErrorResponse());
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleById(int id)
    {
        try
        {
            var role = await _roleService.GetRoleByIdAsync(id);

            if (role == null)
            {
                return NotFound(Resources.Get("ROLE_NOT_FOUND").ToApiErrorResponse());
            }

            var result = new RoleDetailedResponse
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                IsEnabled = role.IsEnabled,
                IsBlocked = role.IsBlocked,
                IsDefault = role.IsDefault,
            };

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_GET_ROLE").ToApiErrorResponse());
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
    {
        try
        {
            var result = await _roleService.CreateRoleAsync(request);
            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_CREATE_ROLE").ToApiErrorResponse());
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleRequest request)
    {
        try
        {
            var result = await _roleService.UpdateRoleAsync(request);

            if (result == null)
            {
                return NotFound(Resources.Get("ROLE_NOT_FOUND").ToApiErrorResponse());
            }

            var response = new RoleResponse
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                IsEnabled = result.IsEnabled,
                IsBlocked = result.IsBlocked,
                IsDefault = result.IsDefault,
            };

            return Ok(response.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_UPDATE_ROLE").ToApiErrorResponse());
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        try
        {
            var result = await _roleService.DeleteRoleAsync(id);
            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_DELETE_ROLE").ToApiErrorResponse());
        }
    }
}