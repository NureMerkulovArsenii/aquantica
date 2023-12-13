using Aquantica.BLL.Interfaces;
using Aquantica.Core.DTOs.User;

namespace Aquantica.BLL.Services;

public class CustomUserManager
{
    private readonly IAccountService _accountService;

    public CustomUserManager(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public int UserId { get; set; }

    public UserDTO GetCurrentUser()
    {
        try
        {
            var user = _accountService.GetUserById(UserId);

            return user;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public List<AccessActionDTO> GetCurrentUserAccessAction()
    {
        try
        {
            var accessAction = _accountService.GetUserAccessActions(UserId);

            return accessAction;
        }
        catch (Exception e)
        {
            return null;
        }
    }
}