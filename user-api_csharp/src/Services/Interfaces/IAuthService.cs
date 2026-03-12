using user_api_csharp.src.Models;

namespace user_api_csharp.src.Services.Interfaces;

public interface IAuthService
{
  Task<AuthTokensDto?> LoginAsync(LoginRequestDto request);
  Task<AuthTokensDto?> RefreshAsync(string refreshToken);
}
