    namespace Aquantica.Contracts.Requests.Account;

public class UpdateUserRequest
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsBlocked { get; set; }
    public int RoleId { get; set; }
}