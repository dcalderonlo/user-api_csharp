using System.ComponentModel.DataAnnotations;

namespace user_api_csharp.src.Models;

public class Category
{
  public int Id { get; set; }

  [Required]
  [MinLength(2)]
  [MaxLength(100)]
  public string Name { get; set; } = string.Empty;

  public ICollection<Product> Products { get; set; } = [];
}
