using user_api_csharp.src.Common;
using user_api_csharp.src.Models;

namespace user_api_csharp.src.Services.Interfaces;

public interface ICategoryService
{
  Task<IReadOnlyList<Category>> GetAllAsync();
  Task<Category?> GetByIdAsync(int id);
  Task<ServiceResult<Category>> CreateAsync(Category category);
  Task<ServiceResult> UpdateAsync(int id, Category category);
  Task<ServiceResult> DeleteAsync(int id);
}
