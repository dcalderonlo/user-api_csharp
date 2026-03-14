using user_api_csharp.src.Models;

namespace user_api_csharp.src.Common;

public static class ProductMapper
{
  public static ProductResponseDto ToResponse(Product product)
  {
    return new ProductResponseDto
    {
      Id = product.Id,
      Name = product.Name,
      Price = product.Price,
      Stock = product.Stock,
      SupplierId = product.SupplierId,
      SupplierName = product.Supplier?.Name,
      CategoryId = product.CategoryId,
      CategoryName = product.Category?.Name
    };
  }

  public static Product ToEntity(ProductCreateRequestDto request)
  {
    return new Product
    {
      Name = request.Name,
      Price = request.Price,
      Stock = request.Stock,
      SupplierId = request.SupplierId,
      CategoryId = request.CategoryId
    };
  }

  public static Product ToEntity(ProductUpdateRequestDto request)
  {
    return new Product
    {
      Id = request.Id,
      Name = request.Name,
      Price = request.Price,
      Stock = request.Stock,
      SupplierId = request.SupplierId,
      CategoryId = request.CategoryId
    };
  }
}
