using System.Security.Claims;
using Aquantica.Core.Entities;

namespace Aquantica.BLL.Interfaces;

public interface ITokenService
{
    /// <summary>
    /// Generates acess token.
    /// </summary>
    /// <param name="claims">List of claims.</param>
    /// <returns></returns>
    string GenerateAccessToken(IList<Claim> claims);

    /// <summary>
    /// Generates refresh token.
    /// </summary>
    /// <returns></returns>
    RefreshToken GenerateRefreshToken(User user);

    /// <summary>
    /// Gets principal form expired token.
    /// </summary>
    /// <param name="token">The expired token.</param>
    /// <returns></returns>
    ClaimsPrincipal GetPrincipalFromToken(string token, bool validateLifetime = false);
}