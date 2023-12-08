namespace Aquantica.Core.DTOs;

public class RuleSetDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
    public int? ParentId { get; set; }
}