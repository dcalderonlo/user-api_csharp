using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using user_api_csharp.src.Configuration;
using user_api_csharp.src.Data;
using user_api_csharp.src.DTOs;
using user_api_csharp.src.Interfaces;
using user_api_csharp.src.Models;

namespace user_api_csharp.src.Services;

public class AuthService(
  AppDbContext context,
  IPasswordHasher passwordHasher,
  IRefreshTokenFactory refreshTokenFactory,
  IJwtTokenService jwtTokenService,
  IOptions<JwtOptions> jwtOptions) : IAuthService
{
  public async Task<AuthTokensDto?> LoginAsync(LoginRequestDto request)
  {
    var normalizedEmail = request.Email.Trim().ToLowerInvariant();
    var passwordHash = passwordHasher.Hash(request.Password);

    var user = await context.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail);
    if (user is null || user.PasswordHash != passwordHash)
    {
      return null;
    }

    return await IssueTokensAsync(user);
  }

  public async Task<AuthTokensDto?> RefreshAsync(string refreshToken)
  {
    if (string.IsNullOrWhiteSpace(refreshToken))
    {
      return null;
    }

    var refreshTokenHash = passwordHasher.Hash(refreshToken);

    var user = await context.Users.FirstOrDefaultAsync(u =>
      u.RefreshTokenHash == refreshTokenHash
      && u.RefreshTokenExpiresAt.HasValue
      && u.RefreshTokenExpiresAt.Value > DateTime.UtcNow);

    if (user is null)
    {
      return null;
    }

    return await IssueTokensAsync(user);
  }

  private async Task<AuthTokensDto> IssueTokensAsync(User user)
  {
    var accessToken = jwtTokenService.CreateAccessToken(user);
    var refreshToken = refreshTokenFactory.Create();
    var refreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(jwtOptions.Value.RefreshTokenMinutes);

    user.RefreshTokenHash = passwordHasher.Hash(refreshToken);
    user.RefreshTokenExpiresAt = refreshTokenExpiresAt;

    await context.SaveChangesAsync();

    return new AuthTokensDto
    {
      AccessToken = accessToken.Token,
      RefreshToken = refreshToken,
      AccessTokenExpiresAtUtc = accessToken.ExpiresAtUtc,
      RefreshTokenExpiresAtUtc = refreshTokenExpiresAt
    };
  }
}
