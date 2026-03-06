using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user_api_csharp.src.DTOs;
using user_api_csharp.src.Interfaces;

namespace user_api_csharp.src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
  private const string RefreshCookieName = "refreshToken";

  [AllowAnonymous]
  [HttpPost("login")]
  public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
  {
    var tokens = await authService.LoginAsync(request);
    if (tokens is null)
    {
      return Unauthorized(new { message = "Invalid credentials." });
    }

    SetRefreshTokenCookie(tokens.RefreshToken, tokens.RefreshTokenExpiresAtUtc);

    return Ok(new AuthResponseDto
    {
      AccessToken = tokens.AccessToken,
      AccessTokenExpiresAtUtc = tokens.AccessTokenExpiresAtUtc
    });
  }

  [AllowAnonymous]
  [HttpPost("refresh")]
  public async Task<ActionResult<AuthResponseDto>> Refresh()
  {
    if (!Request.Cookies.TryGetValue(RefreshCookieName, out var refreshToken) || string.IsNullOrWhiteSpace(refreshToken))
    {
      return Unauthorized(new { message = "Refresh token cookie is missing." });
    }

    var tokens = await authService.RefreshAsync(refreshToken);
    if (tokens is null)
    {
      return Unauthorized(new { message = "Invalid or expired refresh token." });
    }

    SetRefreshTokenCookie(tokens.RefreshToken, tokens.RefreshTokenExpiresAtUtc);

    return Ok(new AuthResponseDto
    {
      AccessToken = tokens.AccessToken,
      AccessTokenExpiresAtUtc = tokens.AccessTokenExpiresAtUtc
    });
  }

  private void SetRefreshTokenCookie(string refreshToken, DateTime refreshTokenExpiresAtUtc)
  {
    Response.Cookies.Append(RefreshCookieName, refreshToken, new CookieOptions
    {
      HttpOnly = true,
      Secure = Request.IsHttps,
      SameSite = SameSiteMode.Strict,
      Expires = new DateTimeOffset(refreshTokenExpiresAtUtc),
      Path = "/api/auth"
    });
  }
}
