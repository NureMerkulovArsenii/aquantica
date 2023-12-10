namespace Aquantica.Core.DTOs;

public class IrrigationEventDTO
{
    public int? Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double WaterUsed { get; set; }

    public bool IsStopped { get; set; }
    public int SectionId { get; set; }
    public int IrrigationRulesetId { get; set; }
}