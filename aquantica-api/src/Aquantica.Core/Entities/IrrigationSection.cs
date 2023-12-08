using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Aquantica.Core.Entities;

[Table("IrrigationSections")]
public class IrrigationSection : BaseEntity
{
    public int Number { get; set; }
    public string? Name { get; set; }
    public int? ParentId { get; set; }
    public bool IsEnabled { get; set; }
    public int? LocationId { get; set; }
    public virtual Location? Location { get; set; }
    public virtual IrrigationSection? ParentSection { get; set; }
    public virtual ICollection<IrrigationEvent>? IrrigationEvents { get; set; }
    public int? SectionRulesetId { get; set; }
    public virtual IrrigationRuleset? IrrigationRuleset { get; set; }
    public int? IrrigationSectionTypeId { get; set; }
    public virtual IrrigationSectionType? IrrigationSectionType { get; set; }
    
    public virtual ICollection<BackgroundJob>? BackgroundJobs { get; set; }
    
}