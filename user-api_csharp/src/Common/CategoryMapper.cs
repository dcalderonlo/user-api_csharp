using user_api_csharp.src.Models;

namespace user_api_csharp.src.Common;

public static class CategoryMapper
{
  public static CategoryResponseDto ToResponse(Category category)
  {
    return new CategoryResponseDto
    {
      Id = category.Id,
      Name = category.Name
    };
  }

  public static Category ToEntity(CategoryCreateRequestDto request)
  {
    return new Category
    {
      Name = request.Name
    };
  }

  public static Category ToEntity(CategoryUpdateRequestDto request)
  {
    return new Category
    {
      Id = request.Id,
      Name = request.Name
    };
  }
}
