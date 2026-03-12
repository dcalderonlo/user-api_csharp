using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using user_api_csharp.src.Configuration;
using user_api_csharp.src.Data;
using user_api_csharp.src.Models;
using user_api_csharp.src.Services.Interfaces;

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

    var user = await context.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail);
    if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
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

    var refreshTokenHash = RefreshTokenHash(refreshToken);

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

    user.RefreshTokenHash = RefreshTokenHash(refreshToken);
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

  private static string RefreshTokenHash(string value)
  {
    var bytes = Encoding.UTF8.GetBytes(value);
    var hash = SHA256.HashData(bytes);
    return Convert.ToHexString(hash).ToLowerInvariant();
  }
}
