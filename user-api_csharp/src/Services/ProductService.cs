using Microsoft.EntityFrameworkCore;
using user_api_csharp.src.Common;
using user_api_csharp.src.Data;
using user_api_csharp.src.Models;
using user_api_csharp.src.Services.Interfaces;

namespace user_api_csharp.src.Services;

public class ProductService(AppDbContext context) : IProductService
{
  public async Task<IReadOnlyList<Product>> GetAllAsync()
  {
    return await context.Products
      .AsNoTracking()
      .Include(p => p.Supplier)
      .Include(p => p.Category)
      .ToListAsync();
  }

  public async Task<Product?> GetByIdAsync(int id)
  {
    return await context.Products
      .AsNoTracking()
      .Include(p => p.Supplier)
      .Include(p => p.Category)
      .FirstOrDefaultAsync(p => p.Id == id);
  }

  public async Task<ServiceResult<Product>> CreateAsync(Product product)
  {
    var supplierExists = await context.Suppliers.AnyAsync(s => s.Id == product.SupplierId);
    if (!supplierExists)
    {
      return ServiceResult<Product>.Fail(ProductServiceErrorCodes.SupplierNotFound, "Supplier not found.");
    }

    var categoryExists = await context.Categories.AnyAsync(c => c.Id == product.CategoryId);
    if (!categoryExists)
    {
      return ServiceResult<Product>.Fail(ProductServiceErrorCodes.CategoryNotFound, "Category not found.");
    }

    context.Products.Add(product);
    await context.SaveChangesAsync();

    product.Supplier = await context.Suppliers.AsNoTracking().FirstOrDefaultAsync(s => s.Id == product.SupplierId);
    product.Category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == product.CategoryId);

    return ServiceResult<Product>.Ok(product);
  }

  public async Task<ServiceResult> UpdateAsync(int id, Product product)
  {
    var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
    if (existingProduct is null)
    {
      return ServiceResult.Fail(ProductServiceErrorCodes.NotFound, "Product not found.");
    }

    var supplierExists = await context.Suppliers.AnyAsync(s => s.Id == product.SupplierId);
    if (!supplierExists)
    {
      return ServiceResult.Fail(ProductServiceErrorCodes.SupplierNotFound, "Supplier not found.");
    }

    var categoryExists = await context.Categories.AnyAsync(c => c.Id == product.CategoryId);
    if (!categoryExists)
    {
      return ServiceResult.Fail(ProductServiceErrorCodes.CategoryNotFound, "Category not found.");
    }

    existingProduct.Name = product.Name;
    existingProduct.Price = product.Price;
    existingProduct.Stock = product.Stock;
    existingProduct.SupplierId = product.SupplierId;
    existingProduct.CategoryId = product.CategoryId;

    await context.SaveChangesAsync();

    return ServiceResult.Ok();
  }

  public async Task<ServiceResult> DeleteAsync(int id)
  {
    var product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
    if (product is null)
    {
      return ServiceResult.Fail(ProductServiceErrorCodes.NotFound, "Product not found.");
    }

    context.Products.Remove(product);
    await context.SaveChangesAsync();

    return ServiceResult.Ok();
  }

  public async Task<IReadOnlyList<Product>> GetByCategoryAsync(int categoryId)
  {
    return await context.Products
      .AsNoTracking()
      .Include(p => p.Supplier)
      .Include(p => p.Category)
      .Where(p => p.CategoryId == categoryId)
      .ToListAsync();
  }

  public async Task<IReadOnlyList<Product>> GetBySupplierAsync(int supplierId)
  {
    return await context.Products
      .AsNoTracking()
      .Include(p => p.Supplier)
      .Include(p => p.Category)
      .Where(p => p.SupplierId == supplierId)
      .ToListAsync();
  }

  public async Task<int> GetTotalCountAsync()
  {
    return await context.Products.CountAsync();
  }

  public async Task<(Product? HighestPriceProduct, Product? LowestPriceProduct, decimal TotalPrice, decimal AveragePrice)> GetPriceSummaryAsync()
  {
    var count = await context.Products.CountAsync();
    if (count == 0)
    {
      return (null, null, 0m, 0m);
    }

    var total = await context.Products.SumAsync(p => p.Price);

    var highest = await context.Products
      .AsNoTracking()
      .Include(p => p.Supplier)
      .Include(p => p.Category)
      .OrderByDescending(p => p.Price)
      .FirstAsync();

    var lowest = await context.Products
      .AsNoTracking()
      .Include(p => p.Supplier)
      .Include(p => p.Category)
      .OrderBy(p => p.Price)
      .FirstAsync();

    return (highest, lowest, total, total / count);
  }
}
