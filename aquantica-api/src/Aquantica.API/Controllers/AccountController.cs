using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
using Aquantica.Contracts.Requests;
using Aquantica.Contracts.Responses;
using Aquantica.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Aquantica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var responseDto = await _accountService.LoginAsync(request, cancellationToken);

        if (responseDto == null)
        {
            return Unauthorized("Failed to login".ToErrorResponse());
        }


        SetRefreshTokenCookie(responseDto.RefreshToken);

        var response = new AuthResponse
        {
            UserId = responseDto.UserId,
            AccessToken = responseDto.AccessToken
        };

        return Ok(response.ToApiResponse());
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var regDto = await _accountService.RegisterAsync(request, cancellationToken);

        if (regDto == null)
        {
            return Unauthorized($"User with such email {request.Email} already exists.".ToErrorResponse());
        }

        SetRefreshTokenCookie(regDto.RefreshToken);

        var response = new AuthResponse
        {
            UserId = regDto.UserId,           
            AccessToken = regDto.AccessToken,
        };

        return Ok(response.ToApiResponse());
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        try
        {
            var token = Request.Headers.Authorization;
            var responseDto =
                await _accountService.RefreshAuth(token, Request.Cookies["refreshToken"], cancellationToken);

            if (responseDto == null)
            {
                return Unauthorized("Failed to login".ToErrorResponse());
            }

            SetRefreshTokenCookie(responseDto.RefreshToken);

            var response = new AuthResponse
            {
                UserId = responseDto.UserId,
                AccessToken = responseDto.AccessToken
            };

            return Ok(response.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest("Something went wrong".ToErrorResponse());
        }
    }

    //ToDo: Implement logout
    //public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    //{
    //    var token = Request.Headers.Authorization;
    //    var refreshToken = Request.Cookies["refreshToken"];
    //    await _accountService.LogoutAsync(token, refreshToken, cancellationToken);
    //    Response.Cookies.Delete("refreshToken");
    //    return Ok("Logged out successfully".ToApiResponse());
    //}

    //ToDo: Implement get user info
    //public async Task<IActionResult> GetUserInfo(string token, CancellationToken cancellationToken)
    //{
    //    var responseDto = await _accountService.GetUserInfo(token, cancellationToken);
    //    if (responseDto == null)
    //    {
    //        return Unauthorized("Failed to login".ToErrorResponse());
    //    }

    //    var response = new AuthResponse
    //    {
    //        Role = responseDto.Role,
    //        UserId = responseDto.UserId,
    //        Email = responseDto.Email,
    //        AccessToken = responseDto.AccessToken
    //    };

    //    return Ok(response.ToApiResponse());

    //}

    private void SetRefreshTokenCookie(RefreshToken newRefreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.ExpireDate
        };

        Response.Cookies.Delete("refreshToken");
        Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
    }

}
