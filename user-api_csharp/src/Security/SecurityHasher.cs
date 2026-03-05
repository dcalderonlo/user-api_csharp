using System.Security.Cryptography;
using System.Text;

namespace user_api_csharp.src.Security;

public static class SecurityHasher
{
  public static string ComputeSha256(string value)
  {
    var bytes = Encoding.UTF8.GetBytes(value);
    var hash = SHA256.HashData(bytes);
    return Convert.ToHexString(hash).ToLowerInvariant();
  }

  public static string GenerateTokenValue(int sizeInBytes = 32)
  {
    var randomBytes = RandomNumberGenerator.GetBytes(sizeInBytes);
    return Convert.ToHexString(randomBytes).ToLowerInvariant();
  }
}
