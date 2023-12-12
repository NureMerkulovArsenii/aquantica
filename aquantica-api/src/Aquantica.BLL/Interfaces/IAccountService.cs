using Aquantica.Contracts.Requests;
using Aquantica.Contracts.Requests.Account;
using Aquantica.Core.DTOs;
using Aquantica.Core.DTOs.User;

namespace Aquantica.BLL.Interfaces;

public interface IAccountService
{
    Task<AuthDTO> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken);

    Task<bool> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken);

    Task<AuthDTO> RefreshAuth(string accessToken, string refreshToken, CancellationToken cancellationToken);
    
    Task<bool> LogoutAsync(string refreshToken, CancellationToken cancellationToken);
    
    Task<UserWithAccessActionsDTO> GetUserInfoAsync(string token, CancellationToken cancellationToken);
    
    UserDTO GetUserById(int id);

    Task<List<UserDTO>> GetAllUsersAsync();
    
    List<AccessActionDTO> GetUserAccessActions(int userId);
}