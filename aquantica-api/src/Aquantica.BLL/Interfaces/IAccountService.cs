using Aquantica.Contracts.Requests;
using Aquantica.Core.DTOs;

namespace Aquantica.BLL.Interfaces;

public interface IAccountService
{
    Task<AuthDTO> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken);

    Task<bool> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken);

    Task<AuthDTO> RefreshAuth(string accessToken, string refreshToken, CancellationToken cancellationToken);
}