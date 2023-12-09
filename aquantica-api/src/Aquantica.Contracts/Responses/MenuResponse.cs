namespace Aquantica.Contracts.Responses;

public class MenuResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Icon { get; set; }
    public string? Url { get; set; }
    public int? ParentId { get; set; }
    public int? AccessActionId { get; set; }
}