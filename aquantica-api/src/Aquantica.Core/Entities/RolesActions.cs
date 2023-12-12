namespace Aquantica.Core.Entities;

public class RolesActions : BaseEntity
{
    public int RoleId { get; set; }
    public virtual Role Role { get; set; }
    public int AccessActionId { get; set; }
    public virtual AccessAction AccessAction { get; set; }
}