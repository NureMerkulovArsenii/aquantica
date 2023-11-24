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
using Microsoft.IdentityModel.Tokens;

namespace Aquantica.BLL.Services;

public class AccountService : IAccountService
{
    private readonly IUnitOfWork _uow;
    private readonly ITokenService _tokenService;

    public AccountService(
        IUnitOfWork uow,
        ITokenService tokenService
    )
    {
        _uow = uow;
        _tokenService = tokenService;
    }

    public async Task<AuthDTO> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        var user = await _uow.UserRepository
            .FirstOrDefaultAsync(u => u.Email == loginRequest.Email, cancellationToken);

        if (user == null)
        {
            throw new Exception();
        }

        var verificationResult = VerifyPassword(loginRequest.Password, user.PasswordHash, user.PasswordSalt);

        if (!verificationResult)
        {
            throw new Exception();
        }

        var removedRefreshTokensCount = await _uow.RefreshTokenRepository
            .DeleteAsync(x => x.User.Id == user.Id, cancellationToken);

        var claims = GetUserClaims(user);
        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken(user);

        try
        {
            using var trans = _uow.CreateTransactionAsync();

            user.RefreshToken = refreshToken;

            await _uow.RefreshTokenRepository.AddAsync(refreshToken, cancellationToken);

            _uow.UserRepository.Update(user);

            await _uow.CommitTransactionAsync();
            await _uow.SaveAsync();

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
            await _uow.RollbackTransactionAsync();
            return null;
        }
    }

    public async Task<bool> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken)
    {
        var isUserExist =
            await _uow.UserRepository.ExistAsync(u => u.Email == registerRequest.Email, cancellationToken);

        if (isUserExist)
        {
            return false;
        }

        var (passwordSalt, passwordHash) = GetHash(registerRequest.Password);

        var role = await _uow.RoleRepository
            .FirstOrDefaultAsync(r => r.IsEnabled && !r.IsBlocked && r.IsDefault, cancellationToken);

        var user = new User()
        {
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            IsEnabled = false,
            IsBlocked = false,
            Email = registerRequest.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = role
        };

        await _uow.UserRepository.AddAsync(user, cancellationToken);
        
        await _uow.SaveAsync();
        
        return true;
    }


    public async Task<AuthDTO> RefreshAuth(string accessToken, string refreshToken, CancellationToken cancellationToken)
    {
        try
        {
            var inputToken = accessToken.Replace("Bearer", "").Trim();
            var principal = _tokenService.GetPrincipalFromToken(inputToken);

            var userIdClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            var userIdString = userIdClaim?.Value;

            if (userIdString == null)
            {
                return null;
            }

            var userId = int.Parse(userIdString);

            var user = await _uow.UserRepository
                .FirstOrDefaultAsync(x => x.Id == userId,
                    cancellationToken: cancellationToken);
            
            if (user == null)
            {
                return null;
            }

            var refreshTokens = await _uow.RefreshTokenRepository
                .GetByConditionAsync(x => x.UserId == user.Id,cancellationToken: cancellationToken);

            if (refreshTokens.Count != 0)
            {
                var refreshTokenOld = refreshTokens.FirstOrDefault();

                if (refreshTokenOld?.Token != refreshToken)
                    return null;
                
                var now = DateTime.UtcNow;

                if (refreshTokenOld?.ExpireDate < now)
                {
                    _uow.RefreshTokenRepository.Delete(refreshTokenOld);
                    await _uow.SaveAsync();
                    return null;
                }

                foreach (var token in refreshTokens)
                {
                    _uow.RefreshTokenRepository.Delete(token);
                }

                await _uow.SaveAsync();
            }

            var newRefreshToken = _tokenService.GenerateRefreshToken(user);

            var claims = GetUserClaims(user);

            var newAccessToken = _tokenService.GenerateAccessToken(claims);

            user.RefreshToken = newRefreshToken;

            try
            {
                var trans = await _uow.CreateTransactionAsync();

                await _uow.RefreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);

                _uow.UserRepository.Update(user);

                await _uow.CommitTransactionAsync();
                await _uow.SaveAsync();

                var res = new AuthDTO()
                {
                    Email = user.Email,
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    UserId = user.Id,
                    Role = user.Role
                };

                return res;
            }
            catch (Exception e)
            {
                await _uow.RollbackTransactionAsync();
                return null;
            }
        }
        catch (SecurityTokenException securityException)
        {
            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }


    public async Task<bool> GetActionAccess(string accessToken, string actionName, CancellationToken cancellationToken)
    {
        var inputToken = accessToken.Replace("Bearer", "").Trim();
        var principal = _tokenService.GetPrincipalFromToken(inputToken, true);

        var userIdString = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        if (userIdString == null)
        {
            return false;
        }

        var userId = int.Parse(userIdString);

        var user = await _uow.UserRepository
            .FirstOrDefaultAsync(
                u => u.Id == userId && u.IsEnabled && !u.IsBlocked &&
                     u.Role.AccessActions.Any(x => x.Code == actionName && x.IsEnabled),
                cancellationToken);

        return user != null;
    }


    private List<Claim> GetUserClaims(User user)
    {
        var result = new List<Claim>(new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user?.Id.ToString()),
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