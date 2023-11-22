using Aquantica.Core.Entities;

namespace Aquantica.Core.DTOs;

public class AuthDTO
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string AccessToken { get; set; }
    public RefreshToken RefreshToken { get; set; }
    public Role Role { get; set; }
}
