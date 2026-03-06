namespace user_api_csharp.src.Interfaces;

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
