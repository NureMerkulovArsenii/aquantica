using System.ComponentModel.DataAnnotations.Schema;

namespace Aquantica.Core.Entities;

[Table("Users")]
public class User : BaseEntity
{
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string FirstName{ get; set; }
    public string LastName { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsBlocked { get; set; }
    public int RoleId { get; set; }
    public virtual Role Role { get; set; }
    public int? RefreshTokenId { get; set; }
    public virtual RefreshToken RefreshToken { get; set; }
}
