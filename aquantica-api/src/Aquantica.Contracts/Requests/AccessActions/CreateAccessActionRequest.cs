namespace Aquantica.Contracts.Requests.AccessActions;

public class CreateAccessActionRequest
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
    public List<int> RoleIds { get; set; }
}