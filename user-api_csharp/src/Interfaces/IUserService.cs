using user_api_csharp.src.Models;

namespace user_api_csharp.src.Interfaces;

public interface IUserService
{
  Task<IReadOnlyList<User>> GetAllAsync();
  Task<User?> GetByIdAsync(int id);
  Task<ServiceResult<User>> CreateAsync(User user, string rawPassword);
  Task<ServiceResult> UpdateAsync(int id, User user);
  Task<ServiceResult> DeleteAsync(int id);
}
