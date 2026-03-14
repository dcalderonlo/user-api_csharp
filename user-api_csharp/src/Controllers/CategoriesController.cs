using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user_api_csharp.src.Common;
using user_api_csharp.src.Models;
using user_api_csharp.src.Services.Interfaces;

namespace user_api_csharp.src.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetCategories()
  {
    var categories = await categoryService.GetAllAsync();
    return Ok(categories.Select(CategoryMapper.ToResponse));
  }

  [HttpGet("{id:int}")]
  public async Task<ActionResult<CategoryResponseDto>> GetCategory(int id)
  {
    var category = await categoryService.GetByIdAsync(id);
    return category is null ? NotFound(new { message = "Category not found." }) : Ok(CategoryMapper.ToResponse(category));
  }

  [HttpPost]
  public async Task<ActionResult<CategoryResponseDto>> PostCategory([FromBody] CategoryCreateRequestDto request)
  {
    var category = CategoryMapper.ToEntity(request);
    var result = await categoryService.CreateAsync(category);

    return CreatedAtAction(nameof(GetCategory), new { id = result.Data!.Id }, CategoryMapper.ToResponse(result.Data));
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> PutCategory(int id, [FromBody] CategoryUpdateRequestDto request)
  {
    if (id != request.Id)
    {
      return BadRequest(new { message = "The ID in the route does not match the ID of the category." });
    }

    var category = CategoryMapper.ToEntity(request);
    var result = await categoryService.UpdateAsync(id, category);

    if (!result.IsSuccess && result.ErrorCode == CategoryServiceErrorCodes.NotFound)
    {
      return NotFound(new { message = "Category not found." });
    }

    return NoContent();
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> DeleteCategory(int id)
  {
    var result = await categoryService.DeleteAsync(id);
    if (!result.IsSuccess && result.ErrorCode == CategoryServiceErrorCodes.NotFound)
    {
      return NotFound(new { message = "Category not found." });
    }

    return NoContent();
  }
}
