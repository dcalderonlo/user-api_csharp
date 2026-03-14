using System.ComponentModel.DataAnnotations;

namespace user_api_csharp.src.Models;

public class Supplier
{
  public int Id { get; set; }

  [Required]
  [MinLength(2)]
  [MaxLength(120)]
  public string Name { get; set; } = string.Empty;

  [Required]
  [MinLength(2)]
  [MaxLength(200)]
  public string Contact { get; set; } = string.Empty;

  public ICollection<Product> Products { get; set; } = [];
}
