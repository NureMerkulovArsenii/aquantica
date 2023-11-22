using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Aquantica.Core.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Aquantica.BLL.Helpers;

public class TokenHelper
{
    public string GenerateAccessToken(IList<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.JwtSettings.Key));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _tokenSettings.JwtSettings.Issuer,
            Audience = _tokenSettings.JwtSettings.Audience,
            Expires = DateTime.UtcNow.Add(_tokenSettings.JwtSettings.AccessTokenLifetime),
            SigningCredentials = signinCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }

    /// <inheritdoc />
    public RefreshToken GenerateRefreshToken(User user)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomNumber);

        var res = new RefreshToken
        {
            Id = user.Id,
            User = user,
            Token = Convert.ToBase64String(randomNumber),
            ExpireDate = DateTime.Now.Add(_tokenSettings.JwtSettings.RefreshTokenLifeTime).ToUniversalTime(),
        };

        return res;
    }

    /// <inheritdoc />
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.JwtSettings.Key)),
            ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
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