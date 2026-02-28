using System.ComponentModel.DataAnnotations;

namespace user_api_csharp.src.Models;

public class User
{
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
