using System.ComponentModel.DataAnnotations;

namespace user_api_csharp.src.Models;

public class SupplierCreateRequestDto
{
  [Required]
  [MinLength(2)]
  [MaxLength(120)]
  public string Name { get; set; } = string.Empty;

  [Required]
  [MinLength(2)]
  [MaxLength(200)]
  public string Contact { get; set; } = string.Empty;
}

public class SupplierUpdateRequestDto
{
  [Required]
  public int Id { get; set; }

  [Required]
  [MinLength(2)]
  [MaxLength(120)]
  public string Name { get; set; } = string.Empty;

  [Required]
  [MinLength(2)]
  [MaxLength(200)]
  public string Contact { get; set; } = string.Empty;
}

public class SupplierResponseDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Contact { get; set; } = string.Empty;
}
