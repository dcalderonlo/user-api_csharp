using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using user_api_csharp.src.Data;
using user_api_csharp.src.DTOs;
using user_api_csharp.src.Interfaces;
using user_api_csharp.src.Models;
using user_api_csharp.src.Security;

namespace user_api_csharp.src.Services;

public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
{
  public async Task<AuthTokensDto?> LoginAsync(LoginRequestDto request)
  {
    var normalizedEmail = request.Email.Trim().ToLowerInvariant();
    var passwordHash = SecurityHasher.ComputeSha256(request.Password);

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

    var refreshTokenHash = SecurityHasher.ComputeSha256(refreshToken);

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
    var jwtSection = configuration.GetSection("Jwt");
    var issuer = jwtSection["Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is not configured.");
    var audience = jwtSection["Audience"] ?? throw new InvalidOperationException("Jwt:Audience is not configured.");
    var key = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");

    var accessTokenMinutes = ReadPositiveInt(jwtSection["AccessTokenMinutes"], 15);
    var refreshTokenMinutes = ReadPositiveInt(jwtSection["RefreshTokenMinutes"], 120);

    var accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(accessTokenMinutes);
    var refreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(refreshTokenMinutes);

    var accessToken = BuildAccessToken(user, issuer, audience, key, accessTokenExpiresAt);
    var refreshToken = SecurityHasher.GenerateTokenValue();

    user.RefreshTokenHash = SecurityHasher.ComputeSha256(refreshToken);
    user.RefreshTokenExpiresAt = refreshTokenExpiresAt;

    await context.SaveChangesAsync();

    return new AuthTokensDto
    {
      AccessToken = accessToken,
      RefreshToken = refreshToken,
      AccessTokenExpiresAtUtc = accessTokenExpiresAt,
      RefreshTokenExpiresAtUtc = refreshTokenExpiresAt
    };
  }

  private static string BuildAccessToken(User user, string issuer, string audience, string key, DateTime expiresAt)
  {
    var claims = new[]
    {
      new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
      new Claim(JwtRegisteredClaimNames.Email, user.Email),
      new Claim(JwtRegisteredClaimNames.UniqueName, user.Name),
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
      issuer: issuer,
      audience: audience,
      claims: claims,
      expires: expiresAt,
      signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  private static int ReadPositiveInt(string? value, int fallback)
  {
    return int.TryParse(value, out var parsed) && parsed > 0 ? parsed : fallback;
  }
}
