using System.ComponentModel.DataAnnotations.Schema;

namespace Aquantica.Core.Entities;

[Table("AccessActions")]
public class AccessAction : BaseEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Role> Roles { get; set; }
    
}
