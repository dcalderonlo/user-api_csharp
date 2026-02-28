using Microsoft.AspNetCore.Mvc;
using user_api_csharp.src.DTOs;
using user_api_csharp.src.Interfaces;
using user_api_csharp.src.Mappers;

namespace user_api_csharp.src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
  {
    var users = await userService.GetAllAsync();
    return Ok(users.Select(UserMapper.ToResponse));
  }

  [HttpGet("{id:int}")]
  public async Task<ActionResult<UserResponseDto>> GetUser(int id)
  {
    var user = await userService.GetByIdAsync(id);
    return user is null ? NotFound(new { message = "User not found." }) : Ok(UserMapper.ToResponse(user));
  }

  [HttpPost]
  public async Task<ActionResult<UserResponseDto>> PostUser([FromBody] UserCreateRequestDto request)
  {
    var user = UserMapper.ToEntity(request);

    var result = await userService.CreateAsync(user);
    if (!result.IsSuccess)
    {
      return BadRequest(new { message = result.ErrorMessage });
    }

    return CreatedAtAction(nameof(GetUser), new { id = result.Data!.Id }, UserMapper.ToResponse(result.Data));
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> PutUser(int id, [FromBody] UserUpdateRequestDto request)
  {
    if (id != request.Id)
    {
      return BadRequest(new { message = "The ID in the route does not match the ID of the user." });
    }

    var updatedUser = UserMapper.ToEntity(request);

    var result = await userService.UpdateAsync(id, updatedUser);
    if (!result.IsSuccess && result.ErrorCode == UserServiceErrorCodes.NotFound)
    {
      return NotFound(new { message = "User not found." });
    }

    if (!result.IsSuccess && result.ErrorCode == UserServiceErrorCodes.DuplicateEmail)
    {
      return BadRequest(new { message = result.ErrorMessage });
    }

    return NoContent();
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> DeleteUser(int id)
  {
    var result = await userService.DeleteAsync(id);
    if (!result.IsSuccess && result.ErrorCode == UserServiceErrorCodes.NotFound)
    {
      return NotFound(new { message = "User not found." });
    }

    return NoContent();
  }
}
