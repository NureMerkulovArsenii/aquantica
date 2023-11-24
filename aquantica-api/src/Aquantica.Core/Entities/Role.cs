using System.ComponentModel.DataAnnotations.Schema;

namespace Aquantica.Core.Entities;

[Table("Roles")]
public class Role : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsDefault { get; set; }
    public virtual ICollection<AccessAction> AccessActions { get; set; }
    public virtual ICollection<User> Users { get; set; }

}
