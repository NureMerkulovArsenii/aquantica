namespace Aquantica.Core.DTOs;

public class AccessActionDTO
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
}