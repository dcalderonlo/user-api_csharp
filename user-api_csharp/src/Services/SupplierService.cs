using Microsoft.EntityFrameworkCore;
using user_api_csharp.src.Common;
using user_api_csharp.src.Data;
using user_api_csharp.src.Models;
using user_api_csharp.src.Services.Interfaces;

namespace user_api_csharp.src.Services;

public class SupplierService(AppDbContext context) : ISupplierService
{
  public async Task<IReadOnlyList<Supplier>> GetAllAsync()
  {
    return await context.Suppliers.AsNoTracking().ToListAsync();
  }

  public async Task<Supplier?> GetByIdAsync(int id)
  {
    return await context.Suppliers.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
  }

  public async Task<ServiceResult<Supplier>> CreateAsync(Supplier supplier)
  {
    context.Suppliers.Add(supplier);
    await context.SaveChangesAsync();
    return ServiceResult<Supplier>.Ok(supplier);
  }

  public async Task<ServiceResult> UpdateAsync(int id, Supplier supplier)
  {
    var existingSupplier = await context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
    if (existingSupplier is null)
    {
      return ServiceResult.Fail(SupplierServiceErrorCodes.NotFound, "Supplier not found.");
    }

    existingSupplier.Name = supplier.Name;
    existingSupplier.Contact = supplier.Contact;

    await context.SaveChangesAsync();
    return ServiceResult.Ok();
  }

  public async Task<ServiceResult> DeleteAsync(int id)
  {
    var supplier = await context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
    if (supplier is null)
    {
      return ServiceResult.Fail(SupplierServiceErrorCodes.NotFound, "Supplier not found.");
    }

    context.Suppliers.Remove(supplier);
    await context.SaveChangesAsync();

    return ServiceResult.Ok();
  }
}
