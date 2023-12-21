using Aquantica.Core.DTOs.Role;

namespace Aquantica.Core.DTOs.User;

public class UserWithAccessActionsDTO
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public RoleDTO Role { get; set; }
    public List<AccessActionDTO> AccessActions { get; set; }
}