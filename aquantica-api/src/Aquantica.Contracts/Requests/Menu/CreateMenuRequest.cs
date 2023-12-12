
namespace Aquantica.Contracts.Requests.Menu;

public class CreateMenuRequest
{
    public string Name { get; set; }
    public string? Icon { get; set; }
    public string? Url { get; set; }
    public int Order { get; set; }
    public int? ParentId { get; set; }
    public int? AccessActionId { get; set; }
}