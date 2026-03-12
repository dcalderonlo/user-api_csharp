using System.ComponentModel.DataAnnotations;

namespace user_api_csharp.src.Models;

public class UserCreateRequestDto
{
  [Required]
  [MinLength(2)]
  [MaxLength(100)]
  public string Name { get; set; } = string.Empty;

  [Required]
  [EmailAddress]
  [MinLength(5)]
  [MaxLength(255)]
  public string Email { get; set; } = string.Empty;

  [Required]
  [MinLength(8)]
  [MaxLength(128)]
  public string Password { get; set; } = string.Empty;

  public DateTime DateOfBirth { get; set; }
}

public class UserUpdateRequestDto
{
  [Required]
  public int Id { get; set; }

  [Required]
  [MinLength(2)]
  [MaxLength(100)]
  public string Name { get; set; } = string.Empty;

  [Required]
  [EmailAddress]
  [MinLength(5)]
  [MaxLength(255)]
  public string Email { get; set; } = string.Empty;

  public DateTime DateOfBirth { get; set; }
}

public class UserResponseDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public DateTime DateOfBirth { get; set; }
}

public class LoginRequestDto
{
  [Required]
  [EmailAddress]
  [MinLength(5)]
  [MaxLength(255)]
  public string Email { get; set; } = string.Empty;

  [Required]
  [MinLength(8)]
  [MaxLength(128)]
  public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
  public string AccessToken { get; set; } = string.Empty;
  public DateTime AccessTokenExpiresAtUtc { get; set; }
}

public class AuthTokensDto
{
  public string AccessToken { get; set; } = string.Empty;
  public string RefreshToken { get; set; } = string.Empty;
  public DateTime AccessTokenExpiresAtUtc { get; set; }
  public DateTime RefreshTokenExpiresAtUtc { get; set; }
}
