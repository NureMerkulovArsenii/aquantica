using Aquantica.Core.DTOs.Section;

namespace Aquantica.Core.DTOs;

public class StartWateringCommandDTO
{
    public double Duration { get; set; }

    public IrrigationSectionDTO SectionDto { get; set; }
    
}