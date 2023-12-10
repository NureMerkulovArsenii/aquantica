using Aquantica.Contracts.Requests;
using Aquantica.Contracts.Requests.Roles;
using Aquantica.Core.DTOs.Role;

namespace Aquantica.BLL.Interfaces;

public interface IRoleService
{
    Task<List<RoleDTO>> GetAllRolesAsync();
    Task<RoleDetailedDTO> GetRoleByIdAsync(int id);
    Task<RoleDTO> CreateRoleAsync(CreateRoleRequest request);
    Task<RoleDTO> UpdateRoleAsync(UpdateRoleRequest request);
    Task<bool> DeleteRoleAsync(int id);
}