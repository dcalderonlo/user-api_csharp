using user_api_csharp.src.Models;

namespace user_api_csharp.src.Common;

public static class UserMapper
{
  public static UserResponseDto ToResponse(User user)
  {
    return new UserResponseDto
    {
      Id = user.Id,
      Name = user.Name,
      Email = user.Email,
      DateOfBirth = user.DateOfBirth
    };
  }

  public static User ToEntity(UserCreateRequestDto request)
  {
    return new User
    {
      Name = request.Name,
      Email = request.Email,
      PasswordHash = string.Empty,
      DateOfBirth = request.DateOfBirth
    };
  }

  public static User ToEntity(UserUpdateRequestDto request)
  {
    return new User
    {
      Id = request.Id,
      Name = request.Name,
      Email = request.Email,
      DateOfBirth = request.DateOfBirth
    };
  }
}
