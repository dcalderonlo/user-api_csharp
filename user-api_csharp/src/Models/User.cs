using System.ComponentModel.DataAnnotations;

namespace user_api_csharp.src.Models;

public class User
{
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

  [Required]
  [MinLength(64)]
  [MaxLength(64)]
  public string PasswordHash { get; set; } = string.Empty;

  [MinLength(64)]
  [MaxLength(64)]
  public string? RefreshTokenHash { get; set; }

  public DateTime? RefreshTokenExpiresAt { get; set; }

  public DateTime DateOfBirth { get; set; }
}
