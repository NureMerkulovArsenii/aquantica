using Aquantica.Core.DTOs.Section;

namespace Aquantica.Contracts.Responses.IrrigationSection;

public class IrrigationSectionResponse
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string? Name { get; set; }
    public int? ParentId { get; set; }
    public bool IsEnabled { get; set; }
    public string DeviceUri { get; set; }
    public int? SectionRulesetId { get; set; }
    public LocationDto? Location { get; set; }
}