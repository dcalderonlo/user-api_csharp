using user_api_csharp.src.Common;
using user_api_csharp.src.Models;

namespace user_api_csharp.src.Services.Interfaces;

public interface IProductService
{
  Task<IReadOnlyList<Product>> GetAllAsync();
  Task<Product?> GetByIdAsync(int id);
  Task<ServiceResult<Product>> CreateAsync(Product product);
  Task<ServiceResult> UpdateAsync(int id, Product product);
  Task<ServiceResult> DeleteAsync(int id);

  Task<IReadOnlyList<Product>> GetByCategoryAsync(int categoryId);
  Task<IReadOnlyList<Product>> GetBySupplierAsync(int supplierId);
  Task<int> GetTotalCountAsync();
  Task<(Product? HighestPriceProduct, Product? LowestPriceProduct, decimal TotalPrice, decimal AveragePrice)> GetPriceSummaryAsync();
}
