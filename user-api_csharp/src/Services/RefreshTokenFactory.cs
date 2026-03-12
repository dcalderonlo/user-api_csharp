using user_api_csharp.src.Services.Interfaces;
using System.Security.Cryptography;

namespace user_api_csharp.src.Services;

public class RefreshTokenFactory : IRefreshTokenFactory
{
  public string Create()
  {
    var randomBytes = RandomNumberGenerator.GetBytes(32);
    return Convert.ToHexString(randomBytes).ToLowerInvariant();
  }
}
