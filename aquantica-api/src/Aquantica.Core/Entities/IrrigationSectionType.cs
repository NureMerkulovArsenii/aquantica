
namespace Aquantica.Core.Entities;

public class IrrigationSectionType : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual ICollection<IrrigationSection> IrrigationSections { get; set; }
    
}