
namespace Aquantica.Core.Entities;

public class IrrigationEvent : BaseEntity
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double WaterUsed { get; set; }
    public int SectionId { get; set; }
    public virtual IrrigationSection Section { get; set; }
    public int IrrigationRulesetId { get; set; }
    public virtual IrrigationRuleset IrrigationRuleset { get; set; }
}