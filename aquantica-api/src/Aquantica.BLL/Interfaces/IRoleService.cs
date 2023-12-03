using Aquantica.Contracts.Requests;
using Aquantica.Contracts.Requests.Roles;
using Aquantica.Contracts.Responses.Roles;

namespace Aquantica.BLL.Interfaces;

public interface IRoleService
{
    Task<List<RoleResponse>> GetAllRolesAsync();
    Task<RoleDetailedResponse> GetRoleByIdAsync(int id);
    Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request);
    Task<RoleResponse> UpdateRoleAsync(UpdateRoleRequest request);
    Task<bool> DeleteRoleAsync(int id);
}