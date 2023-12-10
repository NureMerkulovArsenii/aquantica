
using Aquantica.Core.DTOs.Role;

namespace Aquantica.Core.DTOs.User;

public class UserDTO
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsBlocked { get; set; }
    public RoleDTO Role { get; set; }
}