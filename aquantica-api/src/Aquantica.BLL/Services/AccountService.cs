using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Requests;
using Aquantica.Core.DTOs;
using Aquantica.Core.Entities;
using Aquantica.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Aquantica.BLL.Services;

public class AccountService : IAccountService
{
    private readonly IUnitOfWork _ouw;
    private readonly ITokenService _tokenService;

    public AccountService(
        IUnitOfWork ouw,
        ITokenService tokenService
    )
    {
        _ouw = ouw;
        _tokenService = tokenService;
    }

    public async Task<AuthDTO> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        var user = await _ouw.UserRepository
            .GetAllByCondition(u => u.Email == loginRequest.Email)
            .Include(x => x.Role)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new Exception();
        }

        var verificationResult = VerifyPassword(loginRequest.Password, user.PasswordHash, user.PasswordSalt);

        if (!verificationResult)
        {
            throw new Exception();
        }

        var removedRefreshTokensCount = await _ouw.RefreshTokenRepository
            .DeleteAsync(x => x.User.Id == user.Id, cancellationToken);

        var claims = GetUserClaims(user);
        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken(user);

        try
        {
            using var trans = _ouw.CreateTransactionAsync();

            user.RefreshToken = refreshToken;

            await _ouw.RefreshTokenRepository.AddAsync(refreshToken, cancellationToken);

            _ouw.UserRepository.Update(user);

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

    public async Task<AuthDTO> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken)
    {
        var isUserExist =
            await _ouw.UserRepository.ExistAsync(u => u.Email == registerRequest.Email, cancellationToken);

        if (isUserExist)
        {
            throw new Exception();
        }

        var (passwordSalt, passwordHash) = GetHash(registerRequest.Password);

        var role = await _ouw.RoleRepository
            .FirstOrDefaultAsync(r => r.IsEnabled && !r.IsBlocked && r.IsDefault, cancellationToken);

        var user = new User()
        {
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,

            Email = registerRequest.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = role
        };

        var claims = GetUserClaims(user);
        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken(user);

        user.RefreshToken = refreshToken;

        try
        {
            using var trans = _ouw.CreateTransactionAsync();

            await _ouw.UserRepository.AddAsync(user, cancellationToken);

            await _ouw.RefreshTokenRepository.AddAsync(refreshToken, cancellationToken);

            await _ouw.CommitTransactionAsync();
            await _ouw.SaveAsync();
        }
        catch (Exception e)
        {
            await _ouw.RollbackTransactionAsync();
            return null;
        }

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

    private (string, string) GetHash(string password)
    {
        using var hmac = new HMACSHA256();

        var passwordSalt = Convert.ToBase64String(hmac.Key);
        var passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));

        return (passwordSalt, passwordHash);
    }

    private bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        var storedHashBytes = Convert.FromBase64String(storedHash);
        var storedSaltBytes = Convert.FromBase64String(storedSalt);

        using var hmac = new HMACSHA256(storedSaltBytes);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != storedHashBytes[i])
            {
                return false;
            }
        }

        return true;
    }
}