using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user_api_csharp.src.Common;
using user_api_csharp.src.Models;
using user_api_csharp.src.Services.Interfaces;

namespace user_api_csharp.src.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController(IProductService productService) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProducts()
  {
    var products = await productService.GetAllAsync();
    return Ok(products.Select(ProductMapper.ToResponse));
  }

  [HttpGet("{id:int}")]
  public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
  {
    var product = await productService.GetByIdAsync(id);
    return product is null ? NotFound(new { message = "Product not found." }) : Ok(ProductMapper.ToResponse(product));
  }

  [HttpPost]
  public async Task<ActionResult<ProductResponseDto>> PostProduct([FromBody] ProductCreateRequestDto request)
  {
    var product = ProductMapper.ToEntity(request);

    var result = await productService.CreateAsync(product);
    if (!result.IsSuccess && result.ErrorCode == ProductServiceErrorCodes.SupplierNotFound)
    {
      return BadRequest(new { message = "Supplier not found." });
    }

    if (!result.IsSuccess && result.ErrorCode == ProductServiceErrorCodes.CategoryNotFound)
    {
      return BadRequest(new { message = "Category not found." });
    }

    return CreatedAtAction(nameof(GetProduct), new { id = result.Data!.Id }, ProductMapper.ToResponse(result.Data));
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> PutProduct(int id, [FromBody] ProductUpdateRequestDto request)
  {
    if (id != request.Id)
    {
      return BadRequest(new { message = "The ID in the route does not match the ID of the product." });
    }

    var product = ProductMapper.ToEntity(request);
    var result = await productService.UpdateAsync(id, product);

    if (!result.IsSuccess && result.ErrorCode == ProductServiceErrorCodes.NotFound)
    {
      return NotFound(new { message = "Product not found." });
    }

    if (!result.IsSuccess && result.ErrorCode == ProductServiceErrorCodes.SupplierNotFound)
    {
      return BadRequest(new { message = "Supplier not found." });
    }

    if (!result.IsSuccess && result.ErrorCode == ProductServiceErrorCodes.CategoryNotFound)
    {
      return BadRequest(new { message = "Category not found." });
    }

    return NoContent();
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> DeleteProduct(int id)
  {
    var result = await productService.DeleteAsync(id);
    if (!result.IsSuccess && result.ErrorCode == ProductServiceErrorCodes.NotFound)
    {
      return NotFound(new { message = "Product not found." });
    }

    return NoContent();
  }

  [HttpGet("price-summary")]
  public async Task<ActionResult<ProductPriceSummaryDto>> GetPriceSummary()
  {
    var summary = await productService.GetPriceSummaryAsync();

    return Ok(new ProductPriceSummaryDto
    {
      HighestPriceProduct = summary.HighestPriceProduct is null ? null : ProductMapper.ToResponse(summary.HighestPriceProduct),
      LowestPriceProduct = summary.LowestPriceProduct is null ? null : ProductMapper.ToResponse(summary.LowestPriceProduct),
      TotalPrice = summary.TotalPrice,
      AveragePrice = summary.AveragePrice
    });
  }

  [HttpGet("by-category/{categoryId:int}")]
  public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetByCategory(int categoryId)
  {
    var products = await productService.GetByCategoryAsync(categoryId);
    return Ok(products.Select(ProductMapper.ToResponse));
  }

  [HttpGet("by-supplier/{supplierId:int}")]
  public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetBySupplier(int supplierId)
  {
    var products = await productService.GetBySupplierAsync(supplierId);
    return Ok(products.Select(ProductMapper.ToResponse));
  }

  [HttpGet("total-count")]
  public async Task<ActionResult<object>> GetTotalCount()
  {
    var totalCount = await productService.GetTotalCountAsync();
    return Ok(new { totalCount });
  }
}
