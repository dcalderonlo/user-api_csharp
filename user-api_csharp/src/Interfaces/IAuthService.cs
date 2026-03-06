using user_api_csharp.src.DTOs;

namespace user_api_csharp.src.Interfaces;

public interface IAuthService
{
  Task<AuthTokensDto?> LoginAsync(LoginRequestDto request);
  Task<AuthTokensDto?> RefreshAsync(string refreshToken);
}
