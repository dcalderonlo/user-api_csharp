using user_api_csharp.src.Interfaces;
using user_api_csharp.src.Security;

namespace user_api_csharp.src.Services;

public class RefreshTokenFactory : IRefreshTokenFactory
{
  public string Create()
  {
    return SecurityHasher.GenerateTokenValue();
  }
}
