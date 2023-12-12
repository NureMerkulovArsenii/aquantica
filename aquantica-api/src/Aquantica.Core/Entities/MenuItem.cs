using System.ComponentModel.DataAnnotations.Schema;

namespace Aquantica.Core.Entities;

[Table("MenuItems")]
public class MenuItem : BaseEntity
{
    public string Name { get; set; }
    public string? Icon { get; set; }
    public string? Url { get; set; }

    public int Order { get; set; }
    public int? ParentId { get; set; }
    public virtual MenuItem? ParentMenuItem { get; set; }
    public int? AccessActionId { get; set; }
    
    public virtual AccessAction? AccessAction { get; set; }
}