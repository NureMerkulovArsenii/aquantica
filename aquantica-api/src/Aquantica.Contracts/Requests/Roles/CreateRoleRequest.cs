namespace Aquantica.Contracts.Requests.Roles;

public class CreateRoleRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsDefault { get; set; }
    public List<int>? AccessActionsIds { get; set; }
    public List<int>? UserIds { get; set; }
}