using Microsoft.EntityFrameworkCore;
using user_api_csharp.src.Common;
using user_api_csharp.src.Data;
using user_api_csharp.src.Models;
using user_api_csharp.src.Services.Interfaces;

namespace user_api_csharp.src.Services;

public class CategoryService(AppDbContext context) : ICategoryService
{
  public async Task<IReadOnlyList<Category>> GetAllAsync()
  {
    return await context.Categories.AsNoTracking().ToListAsync();
  }

  public async Task<Category?> GetByIdAsync(int id)
  {
    return await context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
  }

  public async Task<ServiceResult<Category>> CreateAsync(Category category)
  {
    context.Categories.Add(category);
    await context.SaveChangesAsync();
    return ServiceResult<Category>.Ok(category);
  }

  public async Task<ServiceResult> UpdateAsync(int id, Category category)
  {
    var existingCategory = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
    if (existingCategory is null)
    {
      return ServiceResult.Fail(CategoryServiceErrorCodes.NotFound, "Category not found.");
    }

    existingCategory.Name = category.Name;

    await context.SaveChangesAsync();
    return ServiceResult.Ok();
  }

  public async Task<ServiceResult> DeleteAsync(int id)
  {
    var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
    if (category is null)
    {
      return ServiceResult.Fail(CategoryServiceErrorCodes.NotFound, "Category not found.");
    }

    context.Categories.Remove(category);
    await context.SaveChangesAsync();

    return ServiceResult.Ok();
  }
}
