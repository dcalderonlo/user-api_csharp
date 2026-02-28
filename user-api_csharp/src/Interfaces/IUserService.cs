using user_api_csharp.src.Models;

namespace user_api_csharp.src.Interfaces;

public interface IUserService
{
  Task<IReadOnlyList<User>> GetAllAsync();
  Task<User?> GetByIdAsync(int id);
  Task<ServiceResult<User>> CreateAsync(User user);
  Task<ServiceResult> UpdateAsync(int id, User user);
  Task<ServiceResult> DeleteAsync(int id);
}

public static class UserServiceErrorCodes
{
  public const string NotFound = "NotFound";
  public const string DuplicateEmail = "DuplicateEmail";
}

public sealed record ServiceResult(string? ErrorCode = null, string? ErrorMessage = null)
{
  public bool IsSuccess => ErrorCode is null;

  public static ServiceResult Ok() => new();

  public static ServiceResult Fail(string errorCode, string errorMessage) => new(errorCode, errorMessage);
}

public sealed record ServiceResult<T>(T? Data, string? ErrorCode = null, string? ErrorMessage = null)
{
  public bool IsSuccess => ErrorCode is null;

  public static ServiceResult<T> Ok(T data) => new(data);

  public static ServiceResult<T> Fail(string errorCode, string errorMessage) => new(default, errorCode, errorMessage);
}
