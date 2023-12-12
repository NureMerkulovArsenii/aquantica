namespace Aquantica.Core.DTOs.Section;

public class SectionWithLocationDTO
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string Name { get; set; }
    public bool IsEnabled { get; set; }
    public int? ParentId { get; set; }
    public string? DeviceUri { get; set; }
    
    public int? SectionRulesetId { get; set; }
    
    public LocationDto? Location { get; set; }
}
