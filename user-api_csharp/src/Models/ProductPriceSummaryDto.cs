namespace user_api_csharp.src.Models;

public class ProductPriceSummaryDto
{
  public ProductResponseDto? HighestPriceProduct { get; set; }
  public ProductResponseDto? LowestPriceProduct { get; set; }
  public decimal TotalPrice { get; set; }
  public decimal AveragePrice { get; set; }
}
