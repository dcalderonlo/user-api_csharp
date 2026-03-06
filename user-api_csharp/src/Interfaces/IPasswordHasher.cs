namespace user_api_csharp.src.Interfaces;

public interface IPasswordHasher
{
  string Hash(string value);
}
