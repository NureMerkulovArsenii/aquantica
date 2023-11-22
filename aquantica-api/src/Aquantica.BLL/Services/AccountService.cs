using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Requests;
using Aquantica.Core.DTOs;
using Aquantica.DAL.UnitOfWork;

namespace Aquantica.BLL.Services;

public class AccountService : IAccountService
{
    private readonly IUnitOfWork _ouw;

    public AccountService(IUnitOfWork ouw)
    {
        _ouw = ouw;
    }

    public Task<AuthDTO> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        //var user = 
        throw new NotImplementedException();
    }

    public Task<AuthDTO> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<AuthDTO> RefreshAuth(string accessToken, string refreshToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}