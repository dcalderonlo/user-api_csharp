using System.ComponentModel.DataAnnotations;

namespace user_api_csharp.src.Models;

public class CategoryCreateRequestDto
{
  [Required]
  [MinLength(2)]
  [MaxLength(100)]
  public string Name { get; set; } = string.Empty;
}

public class CategoryUpdateRequestDto
{
  [Required]
  public int Id { get; set; }

  [Required]
  [MinLength(2)]
  [MaxLength(100)]
  public string Name { get; set; } = string.Empty;
}

public class CategoryResponseDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
}
