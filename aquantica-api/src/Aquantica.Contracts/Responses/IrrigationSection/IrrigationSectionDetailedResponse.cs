using Aquantica.Core.DTOs.Section;
using Aquantica.Core.Entities;

namespace Aquantica.Contracts.Responses.IrrigationSection;

public class IrrigationSectionDetailedResponse
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string? Name { get; set; }
    public int? ParentId { get; set; }
    public bool IsEnabled { get; set; }
    public string DeviceUri { get; set; }
    public int SectionTypeId { get; set; }
    public int? SectionRulesetId { get; set; }
    public LocationDto? Location { get; set; }
}