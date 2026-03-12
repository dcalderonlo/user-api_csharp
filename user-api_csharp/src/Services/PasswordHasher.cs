using user_api_csharp.src.Services.Interfaces;

namespace user_api_csharp.src.Services;

public class PasswordHasher : IPasswordHasher
{
  public string Hash(string value)
  {
    return BCrypt.Net.BCrypt.HashPassword(value, workFactor: 12);
  }

  public bool Verify(string value, string hash)
  {
    return BCrypt.Net.BCrypt.Verify(value, hash);
  }
}
