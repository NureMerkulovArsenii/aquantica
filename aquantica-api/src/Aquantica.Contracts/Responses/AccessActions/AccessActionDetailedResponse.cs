using Aquantica.Core.DTOs.Role;

namespace Aquantica.Contracts.Responses.AccessActions;

public class AccessActionDetailedResponse
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
    public List<RoleDTO> Roles { get; set; }
}