using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Requests.Roles;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        var roles = await _roleService.GetAllRolesAsync();
        return Ok(roles);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleById(int id)
    {
        var role = await _roleService.GetRoleByIdAsync(id);
        return Ok(role);
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
    {
        var result = await _roleService.CreateRoleAsync(request);
        return Ok(result);
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleRequest request)
    {
        var result = await _roleService.UpdateRoleAsync(request);
        return Ok(result);
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        var result = await _roleService.DeleteRoleAsync(id);
        return Ok(result);
    }
}