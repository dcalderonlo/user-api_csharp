using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user_api_csharp.src.DTOs;
using user_api_csharp.src.Interfaces;

namespace user_api_csharp.src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
  [AllowAnonymous]
  [HttpPost("login")]
  public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
  {
    var authResult = await authService.LoginAsync(request);
    if (authResult is null)
    {
      return Unauthorized(new { message = "Invalid credentials." });
    }

    return Ok(authResult);
  }

  [AllowAnonymous]
  [HttpPost("refresh")]
  public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] RefreshTokenRequestDto request)
  {
    var authResult = await authService.RefreshAsync(request);
    if (authResult is null)
    {
      return Unauthorized(new { message = "Invalid or expired refresh token." });
    }

    return Ok(authResult);
  }
}
