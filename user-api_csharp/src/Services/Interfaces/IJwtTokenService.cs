using user_api_csharp.src.Models;

namespace user_api_csharp.src.Services.Interfaces;

public interface IJwtTokenService
{
  AccessTokenResult CreateAccessToken(User user);
}

public sealed record AccessTokenResult(string Token, DateTime ExpiresAtUtc);
