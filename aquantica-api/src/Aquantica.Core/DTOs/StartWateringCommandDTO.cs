namespace Aquantica.Core.DTOs;

public class StartWateringCommandDTO
{
    public double Duration { get; set; }

    public GetIrrigationSectionDTO SectionDto { get; set; }
    
}