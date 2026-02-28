using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using user_api_csharp.src.Data;
using user_api_csharp.src.Models;

namespace user_api_csharp.src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(AppDbContext context) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<IEnumerable<User>>> GetUsers()
  {
    var users = await context.Users.AsNoTracking().ToListAsync();
    return Ok(users);
  }

  [HttpGet("{id:int}")]
  public async Task<ActionResult<User>> GetUser(int id)
  {
    var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    return user is null ? NotFound(new { message = "User not found." }) : Ok(user);
  }

  [HttpPost]
  public async Task<ActionResult<User>> PostUser([FromBody] User user)
  {
    var emailNormalized = user.Email.Trim().ToLower();

    var emailInUse = await context.Users
      .AnyAsync(u => u.Email.Equals(emailNormalized, StringComparison.CurrentCultureIgnoreCase));

    if (emailInUse)
    {
      return BadRequest(new { message = "The email is already in use." });
    }

    user.Email = emailNormalized;
    context.Users.Add(user);
    await context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> PutUser(int id, [FromBody] User updatedUser)
  {
    if (id != updatedUser.Id)
    {
      return BadRequest(new { message = "The ID in the route does not match the ID of the user." });
    }

    var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    if (existingUser is null)
    {
      return NotFound(new { message = "User not found." });
    }

    var emailNormalized = updatedUser.Email.Trim().ToLower();

    var emailInUseByAnother = await context.Users
      .AnyAsync(u => u.Id != id && u.Email.Equals(emailNormalized, StringComparison.CurrentCultureIgnoreCase));

    if (emailInUseByAnother)
    {
      return BadRequest(new { message = "The email is already in use." });
    }

    existingUser.Name = updatedUser.Name;
    existingUser.Email = emailNormalized;
    existingUser.DateOfBirth = updatedUser.DateOfBirth;

    await context.SaveChangesAsync();
    return NoContent();
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> DeleteUser(int id)
  {
    var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    if (user is null)
    {
      return NotFound(new { message = "User not found." });
    }

    context.Users.Remove(user);
    await context.SaveChangesAsync();

    return NoContent();
  }
}
