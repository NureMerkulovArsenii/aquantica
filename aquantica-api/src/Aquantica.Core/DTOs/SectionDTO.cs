namespace Aquantica.Core.DTOs;

public class SectionDTO
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string Name { get; set; }
    public int? ParentId { get; set; }
    public SectionDTO? ParentSection { get; set; }
}