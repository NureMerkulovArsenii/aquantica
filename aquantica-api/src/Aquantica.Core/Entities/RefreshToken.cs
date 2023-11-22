namespace Aquantica.Core.Entities;
public class RefreshToken : BaseEntity
{
    public virtual User User { get; set; }
    public string Token { get; set; }
    public DateTime ExpireDate { get; set; }
}
