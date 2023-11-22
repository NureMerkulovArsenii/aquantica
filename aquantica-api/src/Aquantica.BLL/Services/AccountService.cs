using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Aquantica.BLL.Helpers;
using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Requests;
using Aquantica.Core.DTOs;
using Aquantica.Core.Entities;
using Aquantica.DAL.UnitOfWork;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Aquantica.BLL.Services;

public class AccountService : IAccountService
{
    private readonly IUnitOfWork _ouw;
    private readonly TokenHelper _tokenHelper;

    public AccountService(
        IUnitOfWork ouw,
        TokenHelper tokenHelper
    )
    {
        _ouw = ouw;
        _tokenHelper = tokenHelper;
    }

    public async Task<AuthDTO> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        var user = await _ouw.UserRepository.FirstOrDefaultAsync(u => u.Email == loginRequest.Email, cancellationToken);
        
        if (user == null)
        {
            throw new Exception();
        }
        
        var (salt, hash) = GetHash(loginRequest.Password);
        
        var verificationResult = hash == user.Password;
        
        if (!verificationResult)
        {
            throw new Exception();
        }
        
        //ToDo: remove all refresh tokens for this user
        
        
        var claims = GetUserClaims(user);
        var accessToken = _tokenHelper.GenerateAccessToken(claims);
        var refreshToken = _tokenHelper.GenerateRefreshToken(user);
        
        try
        {
            using var trans = _ouw.CreateTransactionAsync();

            user.RefreshToken = refreshToken;

            await _ouw.RefreshTokenRepository.AddAsync(refreshToken, cancellationToken);

            await _ouw.UserRepository.Update(user);

            await _ouw.CommitTransactionAsync();
            await _ouw.SaveAsync();

            var response = new AuthDTO()
            {
                Role = user.Role,
                UserId = user.Id,
                Email = user.Email,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            return response;
        }

        catch (Exception e)
        {
            await _ouw.RollbackTransactionAsync();
            return null;
        }
        
    }

    public Task<AuthDTO> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<AuthDTO> RefreshAuth(string accessToken, string refreshToken, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    
    private List<Claim> GetUserClaims(User user)
    {
        var result = new List<Claim>(new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user?.Email)
        });

        return result;
    }

    private (byte[], string) GetHash(string password)
    {
        var passBytes = Encoding.UTF8.GetBytes(password);

        using var hmac = new HMACSHA256();

        var hash = hmac.ComputeHash(passBytes);

        var res = Encoding.UTF8.GetString(hash);

        return (hmac.Key, res);
    }
}