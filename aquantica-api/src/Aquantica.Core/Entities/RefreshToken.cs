namespace Aquantica.Core.Entities;
public class RefreshToken : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    public string Token { get; set; }
    public DateTime ExpireDate { get; set; }
}
