using Aquantica.Core.DTOs.User;

namespace Aquantica.Core.DTOs.Role;

public class RoleDetailedDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsDefault { get; set; }
    public List<AccessActionDTO>? AccessActions { get; set; }
    public List<int>? UserIds { get; set; }
}