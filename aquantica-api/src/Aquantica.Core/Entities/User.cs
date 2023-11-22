using System.ComponentModel.DataAnnotations.Schema;

namespace Aquantica.Core.Entities;

[Table("Users")]
public class User : BaseEntity
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string FirstName{ get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public byte[] PasswordSalt { get; set; }
    public virtual Role Role { get; set; }
    public virtual RefreshToken RefreshToken { get; set; }
}
