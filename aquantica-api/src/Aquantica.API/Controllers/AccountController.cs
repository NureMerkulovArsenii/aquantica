using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
using Aquantica.Contracts.Requests.Account;
using Aquantica.Contracts.Responses;
using Aquantica.Contracts.Responses.User;
using Aquantica.Core.DTOs.User;
using Aquantica.Core.Entities;
using Microsoft.AspNetCore.Authorization;
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
        try
        {
            var responseDto = await _accountService.LoginAsync(request, cancellationToken);

            if (responseDto == null)
            {
                return Unauthorized("Failed to login".ToApiErrorResponse());
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
            return BadRequest("Invalid password or email".ToApiErrorResponse());
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _accountService.RegisterAsync(request, cancellationToken);

            return Ok(result.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest("Something went wrong".ToApiErrorResponse());
        }
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        try
        {
            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var responseDto = await _accountService
                .RefreshAuth(token,
                    Request.Cookies["refreshToken"],
                    cancellationToken);

            if (responseDto == null)
            {
                return Unauthorized("Failed to login".ToApiErrorResponse());
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
            return BadRequest("Something went wrong".ToApiErrorResponse());
        }
    }
    
    [Authorize]
    [HttpGet("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        try
        {
            var token = Request.Headers.Authorization;
            var refreshToken = Request.Cookies["refreshToken"];
            await _accountService.LogoutAsync(refreshToken, cancellationToken);
            Response.Cookies.Delete("refreshToken");
            return Ok("Logged out successfully".ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest("Something went wrong".ToApiErrorResponse());
        }
    }
    
    [Authorize]
    [HttpGet("userInfo")]
    public async Task<IActionResult> GetUserInfo(CancellationToken cancellationToken)
    {
        try
        {
            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var response = await _accountService.GetUserInfoAsync(token, cancellationToken);

            return Ok(response.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest("Something went wrong".ToApiErrorResponse());
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
    // public async Task<IActionResult> GetUserInfo(string token, CancellationToken cancellationToken)
    // {
    //     var responseDto = await _accountService.GetUserInfo(token, cancellationToken);
    //     if (responseDto == null)
    //     {
    //         return Unauthorized("Failed to login".ToApiErrorResponse());
    //     }
    //
    //     var response = new AuthResponse
    //     {
    //         Role = responseDto.Role,
    //         UserId = responseDto.UserId,
    //         Email = responseDto.Email,
    //         AccessToken = responseDto.AccessToken
    //     };
    //
    //     return Ok(response.ToApiResponse());
    // }

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