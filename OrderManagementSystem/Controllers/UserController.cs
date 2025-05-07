using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Dtos;
using OrderManagementSystem.Services;

namespace OrderManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    // POST: api/user
    [HttpPost("create")]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto userDto)
    {
        try
        {
            var user = await _userService.CreateUserAsync(userDto);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("update/{userId}")]
    public async Task<IActionResult> UpdateUserAsync([FromRoute] int userId, [FromBody] int age)
    {
        try
        {
            var updatedUser = await _userService.UpdateUserAsync(age, userId);
            return Ok(updatedUser);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("delete/{userId}")]
    public async Task<IActionResult> DeleteUserAsync([FromRoute] int userId)
    {
        try
        {
            var deletedUser = await _userService.DeleteUserAsync(userId);
            return Ok(deletedUser);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByIdAsync([FromRoute] int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}