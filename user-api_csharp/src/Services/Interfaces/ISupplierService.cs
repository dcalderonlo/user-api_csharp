using user_api_csharp.src.Common;
using user_api_csharp.src.Models;

namespace user_api_csharp.src.Services.Interfaces;

public interface ISupplierService
{
  Task<IReadOnlyList<Supplier>> GetAllAsync();
  Task<Supplier?> GetByIdAsync(int id);
  Task<ServiceResult<Supplier>> CreateAsync(Supplier supplier);
  Task<ServiceResult> UpdateAsync(int id, Supplier supplier);
  Task<ServiceResult> DeleteAsync(int id);
}
