namespace Aquantica.Contracts.Responses;

public class SectionResponse
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string? Name { get; set; }
    public int? ParentId { get; set; }
    public bool IsEnabled { get; set; }
    public string DeviceUri { get; set; }
    public int? SectionRulesetId { get; set; }
    public int? ParentNumber { get; set; }
}