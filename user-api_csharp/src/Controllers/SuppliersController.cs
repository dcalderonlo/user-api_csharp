using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user_api_csharp.src.Common;
using user_api_csharp.src.Models;
using user_api_csharp.src.Services.Interfaces;

namespace user_api_csharp.src.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SuppliersController(ISupplierService supplierService) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<IEnumerable<SupplierResponseDto>>> GetSuppliers()
  {
    var suppliers = await supplierService.GetAllAsync();
    return Ok(suppliers.Select(SupplierMapper.ToResponse));
  }

  [HttpGet("{id:int}")]
  public async Task<ActionResult<SupplierResponseDto>> GetSupplier(int id)
  {
    var supplier = await supplierService.GetByIdAsync(id);
    return supplier is null ? NotFound(new { message = "Supplier not found." }) : Ok(SupplierMapper.ToResponse(supplier));
  }

  [HttpPost]
  public async Task<ActionResult<SupplierResponseDto>> PostSupplier([FromBody] SupplierCreateRequestDto request)
  {
    var supplier = SupplierMapper.ToEntity(request);
    var result = await supplierService.CreateAsync(supplier);

    return CreatedAtAction(nameof(GetSupplier), new { id = result.Data!.Id }, SupplierMapper.ToResponse(result.Data));
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> PutSupplier(int id, [FromBody] SupplierUpdateRequestDto request)
  {
    if (id != request.Id)
    {
      return BadRequest(new { message = "The ID in the route does not match the ID of the supplier." });
    }

    var supplier = SupplierMapper.ToEntity(request);
    var result = await supplierService.UpdateAsync(id, supplier);

    if (!result.IsSuccess && result.ErrorCode == SupplierServiceErrorCodes.NotFound)
    {
      return NotFound(new { message = "Supplier not found." });
    }

    return NoContent();
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> DeleteSupplier(int id)
  {
    var result = await supplierService.DeleteAsync(id);
    if (!result.IsSuccess && result.ErrorCode == SupplierServiceErrorCodes.NotFound)
    {
      return NotFound(new { message = "Supplier not found." });
    }

    return NoContent();
  }
}
