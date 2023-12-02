namespace Aquantica.Contracts.Responses;

public class IrrigationHistoryResponse
{
    public int SectionId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration { get; set; }
    public double WaterUsed { get; set; }
}