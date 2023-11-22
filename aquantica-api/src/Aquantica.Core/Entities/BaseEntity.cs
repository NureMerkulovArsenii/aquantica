using System.ComponentModel.DataAnnotations.Schema;

namespace Aquantica.Core.Entities;

public class BaseEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
}
