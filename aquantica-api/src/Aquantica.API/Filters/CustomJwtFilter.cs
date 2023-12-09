using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Aquantica.BLL.Interfaces;
using Aquantica.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aquantica.API.Filters;

public class CustomJwtAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            var user = context.HttpContext.User;
            if (user.Identity?.IsAuthenticated != true)
            {
                context.ModelState.AddModelError("Unauthorized", "You are not authorized");
                context.Result = new UnauthorizedObjectResult(context.ModelState);
            }

            var tokenService = context.HttpContext.RequestServices.GetService<ITokenService>();

            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var principal = tokenService?.GetPrincipalFromToken(token, true);

            if (principal == null)
            {
                context.ModelState.AddModelError("Unauthorized", "You are not authorized");
                context.Result = new UnauthorizedObjectResult(context.ModelState);
            }
            else
            {
                var userId = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                if (userId == null)
                {
                    context.ModelState.AddModelError("Unauthorized", "You are not authorized");
                    context.Result = new UnauthorizedObjectResult(context.ModelState);
                }

                var customUserManager = context.HttpContext.RequestServices.GetService<CustomUserManager>();

                if (customUserManager != null)
                    customUserManager.UserId = int.TryParse(userId, out var id) ? id : -1;
            }
        }
        catch (Exception e)
        {
            context.ModelState.AddModelError("Unauthorized", "You are not authorized");
            context.Result = new UnauthorizedObjectResult(context.ModelState);
        }
    }
}