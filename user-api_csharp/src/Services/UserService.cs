using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using user_api_csharp.src.Data;
using user_api_csharp.src.Models;
using user_api_csharp.src.Interfaces;

namespace user_api_csharp.src.Services;

public class UserService(AppDbContext context) : IUserService
{
  public async Task<IReadOnlyList<User>> GetAllAsync()
  {
    return await context.Users.AsNoTracking().ToListAsync();
  }

  public async Task<User?> GetByIdAsync(int id)
  {
    return await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
  }

  public async Task<ServiceResult<User>> CreateAsync(User user)
  {
    var emailNormalized = NormalizeEmail(user.Email);

    var emailInUse = await context.Users
      .AnyAsync(u => u.Email == emailNormalized);

    if (emailInUse)
    {
      return ServiceResult<User>.Fail(UserServiceErrorCodes.DuplicateEmail, "The email is already in use.");
    }

    user.Email = emailNormalized;
    context.Users.Add(user);

    try
    {
      await context.SaveChangesAsync();
      return ServiceResult<User>.Ok(user);
    }
    catch (DbUpdateException ex) when (IsUniqueEmailViolation(ex))
    {
      return ServiceResult<User>.Fail(UserServiceErrorCodes.DuplicateEmail, "The email is already in use.");
    }
  }

  public async Task<ServiceResult> UpdateAsync(int id, User user)
  {
    var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    if (existingUser is null)
    {
      return ServiceResult.Fail(UserServiceErrorCodes.NotFound, "User not found.");
    }

    var emailNormalized = NormalizeEmail(user.Email);

    var emailInUseByAnother = await context.Users
      .AnyAsync(u => u.Id != id && u.Email == emailNormalized);

    if (emailInUseByAnother)
    {
      return ServiceResult.Fail(UserServiceErrorCodes.DuplicateEmail, "The email is already in use.");
    }

    existingUser.Name = user.Name;
    existingUser.Email = emailNormalized;
    existingUser.DateOfBirth = user.DateOfBirth;

    try
    {
      await context.SaveChangesAsync();
      return ServiceResult.Ok();
    }
    catch (DbUpdateException ex) when (IsUniqueEmailViolation(ex))
    {
      return ServiceResult.Fail(UserServiceErrorCodes.DuplicateEmail, "The email is already in use.");
    }
  }

  public async Task<ServiceResult> DeleteAsync(int id)
  {
    var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    if (user is null)
    {
      return ServiceResult.Fail(UserServiceErrorCodes.NotFound, "User not found.");
    }

    context.Users.Remove(user);
    await context.SaveChangesAsync();

    return ServiceResult.Ok();
  }

  private static string NormalizeEmail(string email)
  {
    return email.Trim().ToLowerInvariant();
  }

  private static bool IsUniqueEmailViolation(DbUpdateException ex)
  {
    return ex.InnerException is SqliteException sqliteException && sqliteException.SqliteErrorCode == 19;
  }
}
