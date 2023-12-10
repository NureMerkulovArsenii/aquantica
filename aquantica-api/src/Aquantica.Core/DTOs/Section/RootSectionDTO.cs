namespace Aquantica.Core.DTOs.Section;

public class RootSectionDTO
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string Name { get; set; }
    public bool IsEnabled { get; set; }
    public int? ParentId { get; set; }
}