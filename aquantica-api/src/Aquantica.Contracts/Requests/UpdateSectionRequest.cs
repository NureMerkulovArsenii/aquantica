namespace Aquantica.Contracts.Requests;

public class UpdateSectionRequest
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string? Name { get; set; }
    public int? ParentId { get; set; }
    public bool IsEnabled { get; set; }
    public int? SectionRulesetId { get; set; }
    public int SectionTypeId { get; set; }
}