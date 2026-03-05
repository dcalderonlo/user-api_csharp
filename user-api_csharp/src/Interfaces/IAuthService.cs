using user_api_csharp.src.DTOs;

namespace user_api_csharp.src.Interfaces;

public interface IAuthService
{
  Task<AuthResponseDto?> LoginAsync(LoginRequestDto request);
  Task<AuthResponseDto?> RefreshAsync(RefreshTokenRequestDto request);
}
