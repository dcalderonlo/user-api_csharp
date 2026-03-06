using user_api_csharp.src.Interfaces;
using user_api_csharp.src.Security;

namespace user_api_csharp.src.Services;

public class Sha256PasswordHasher : IPasswordHasher
{
  public string Hash(string value)
  {
    return SecurityHasher.ComputeSha256(value);
  }
}
