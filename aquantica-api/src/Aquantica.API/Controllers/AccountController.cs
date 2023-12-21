using Aquantica.BLL.Interfaces;
using Aquantica.Contracts.Extensions;
using Aquantica.Contracts.Requests.Account;
using Aquantica.Contracts.Responses;
using Aquantica.Contracts.Responses.User;
using Aquantica.Core.DTOs.User;
using Aquantica.Core.Entities;
using Aquantica.Core.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

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
                return Unauthorized(Resources.Get("INVALID_PASSWORD_OR_EMAIL").ToApiErrorResponse());
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
            return BadRequest(Resources.Get("INVALID_PASSWORD_OR_EMAIL").ToApiErrorResponse());
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
            return BadRequest(Resources.Get("REGISTRATION_FAILED").ToApiErrorResponse());
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
                return Unauthorized(Resources.Get("FAILED_TO_RENEW_ACCESS").ToApiErrorResponse());
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
            return BadRequest(Resources.Get("FAILED_TO_RENEW_ACCESS").ToApiErrorResponse());
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
            return Ok(Resources.Get("SUCCESSFUL_LOGOUT").ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_LOGOUT").ToApiErrorResponse());
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
            return BadRequest(Resources.Get("FAILED_TO_GET_USER_INFO").ToApiErrorResponse());
        }
    }

    // [Authorize(Roles = "Admin")]
    [HttpGet("get-all-users")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var response = await _accountService.GetAllUsersAsync();

            return Ok(response.ToApiListResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_GET_ALL_USERS").ToApiErrorResponse());
        }
    }

    [HttpGet("user/{id}")]
    public IActionResult GetUserById(int id)
    {
        try
        {
            var response = _accountService.GetUserById(id);

            return Ok(response.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_GET_USER_INFO").ToApiErrorResponse());
        }
    }

    // [Authorize(Roles = "Admin")]
    [HttpPut("update-user")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
    {
        try
        {
            var response = await _accountService.UpdateUserAsync(request);

            return Ok(response.ToApiResponse());
        }
        catch (Exception e)
        {
            return BadRequest(Resources.Get("FAILED_TO_UPDATE_USER").ToApiErrorResponse());
        }
    }

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