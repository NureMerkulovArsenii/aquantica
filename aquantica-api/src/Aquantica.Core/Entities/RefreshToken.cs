using System.ComponentModel.DataAnnotations.Schema;

namespace Aquantica.Core.Entities;

[Table("RefreshTokens")]
public class RefreshToken : BaseEntity
{
    public virtual User User { get; set; }
    public string Token { get; set; }
    public DateTime ExpireDate { get; set; }
}