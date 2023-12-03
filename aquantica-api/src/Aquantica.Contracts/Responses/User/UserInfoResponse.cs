using Aquantica.Contracts.Responses.Roles;
using Aquantica.Core.DTOs;

namespace Aquantica.Contracts.Responses.User;

public class UserInfoResponse
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public RoleResponse Role { get; set; }
    public List<AccessActionDTO> AccessActions { get; set; }
}