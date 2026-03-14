using System.ComponentModel.DataAnnotations;

namespace user_api_csharp.src.Models;

public class ProductCreateRequestDto
{
  [Required]
  [MinLength(2)]
  [MaxLength(150)]
  public string Name { get; set; } = string.Empty;

  [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
  public decimal Price { get; set; }

  [Range(0, int.MaxValue)]
  public int Stock { get; set; }

  [Required]
  public int SupplierId { get; set; }

  [Required]
  public int CategoryId { get; set; }
}

public class ProductUpdateRequestDto
{
  [Required]
  public int Id { get; set; }

  [Required]
  [MinLength(2)]
  [MaxLength(150)]
  public string Name { get; set; } = string.Empty;

  [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
  public decimal Price { get; set; }

  [Range(0, int.MaxValue)]
  public int Stock { get; set; }

  [Required]
  public int SupplierId { get; set; }

  [Required]
  public int CategoryId { get; set; }
}

public class ProductResponseDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public decimal Price { get; set; }
  public int Stock { get; set; }
  public int SupplierId { get; set; }
  public string? SupplierName { get; set; }
  public int CategoryId { get; set; }
  public string? CategoryName { get; set; }
}
