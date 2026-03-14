using user_api_csharp.src.Models;

namespace user_api_csharp.src.Common;

public static class SupplierMapper
{
  public static SupplierResponseDto ToResponse(Supplier supplier)
  {
    return new SupplierResponseDto
    {
      Id = supplier.Id,
      Name = supplier.Name,
      Contact = supplier.Contact
    };
  }

  public static Supplier ToEntity(SupplierCreateRequestDto request)
  {
    return new Supplier
    {
      Name = request.Name,
      Contact = request.Contact
    };
  }

  public static Supplier ToEntity(SupplierUpdateRequestDto request)
  {
    return new Supplier
    {
      Id = request.Id,
      Name = request.Name,
      Contact = request.Contact
    };
  }
}
