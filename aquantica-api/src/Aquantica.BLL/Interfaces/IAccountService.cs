using Aquantica.Contracts.Requests;
using Aquantica.Contracts.Requests.Account;
using Aquantica.Contracts.Responses.User;
using Aquantica.Core.DTOs;

namespace Aquantica.BLL.Interfaces;

public interface IAccountService
{
    Task<AuthDTO> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken);

    Task<bool> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken);

    Task<AuthDTO> RefreshAuth(string accessToken, string refreshToken, CancellationToken cancellationToken);
    
    Task<bool> LogoutAsync(string refreshToken, CancellationToken cancellationToken);
    
    Task<UserInfoResponse> GetUserInfoAsync(string token, CancellationToken cancellationToken);
}