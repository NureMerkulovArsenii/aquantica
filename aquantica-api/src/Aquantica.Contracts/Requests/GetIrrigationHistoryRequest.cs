namespace Aquantica.Contracts.Requests;

public class GetIrrigationHistoryRequest
{
    public int SectionId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}