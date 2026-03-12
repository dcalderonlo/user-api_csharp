namespace user_api_csharp.src.Services.Interfaces;

public interface IPasswordHasher
{
  string Hash(string value);
  bool Verify(string value, string hash);
}
