using System.ComponentModel.DataAnnotations;

namespace user_api_csharp.src.DTOs;

public class UserCreateRequestDto
{
  [Required]
  [MaxLength(100)]
  public string Name { get; set; } = string.Empty;

  [Required]
  [EmailAddress]
  [MaxLength(255)]
  public string Email { get; set; } = string.Empty;

  public DateTime DateOfBirth { get; set; }
}

public class UserUpdateRequestDto
{
  [Required]
  public int Id { get; set; }

  [Required]
  [MaxLength(100)]
  public string Name { get; set; } = string.Empty;

  [Required]
  [EmailAddress]
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
