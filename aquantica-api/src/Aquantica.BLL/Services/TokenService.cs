using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Aquantica.BLL.Interfaces;
using Aquantica.Core.Entities;
using Aquantica.Core.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Aquantica.BLL.Services;

public class TokenService : ITokenService
{
    private readonly IOptions<AppSettings> _appSettings;

    public TokenService(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings;
    }
    
    public string GenerateAccessToken(IList<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Value.Key));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _appSettings.Value.Issuer,
            Audience = _appSettings.Value.Audience,
            Expires = DateTime.UtcNow.Add(_appSettings.Value.AccessTokenLifetime),
            SigningCredentials = signinCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }

    
    public RefreshToken GenerateRefreshToken(User user)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomNumber);

        var res = new RefreshToken
        {
            UserId = user.Id,
            User = user,
            Token = Convert.ToBase64String(randomNumber),
            ExpireDate = DateTime.Now.Add(_appSettings.Value.RefreshTokenLifeTime).ToUniversalTime()
        };

        return res;
    }
    
    
    public ClaimsPrincipal GetPrincipalFromToken(string token, bool validateLifetime = false)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Value.Key)),
            ValidateLifetime = validateLifetime
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}