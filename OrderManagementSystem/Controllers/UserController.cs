using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Dtos;
using OrderManagementSystem.Services;

namespace OrderManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{

    //logowaniem i autoryzacją, natomiast middleware (UseAuthentication() i UseAuthorization()) uruchamia te mechanizmy w pipeline aplikacji, zapewniając ich działanie dla wszystkich żądań.
    
    
    
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    // POST: api/user
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto userDto)
    {
        try
        {
            await _userService.CreateUserAsync(userDto,userDto.Role);
            return Ok("User registered successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("login")]
    public IActionResult Login()
    {
        return Unauthorized("Musisz się zalogować.");
    }
    
    [AllowAnonymous] //i to jest tak że w momencie gdy  wpisze np to http://localhost:8080/api/user/4 to wyskoczy komunikat ze musze sie zalogować 
    //wiec wchodze na login i po zalogowaniu moge juz skakać miedzy endpointami
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            await _userService.LoginAsync(dto);
            return Ok("Login successful.");
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
    

    [HttpGet("denied")]
    public IActionResult AccessDenied()
    {
        return Content("Brak dostępu.");
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
    
    
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _userService.LogoutAsync();
        return Ok("Logged out.");
    }
    
    
    
    
    
    
    
    

    [HttpGet("{id}")]
    [Authorize(Roles = "User")]//musze być zalogowany Oraz posiadać  role Admin
     //[Authorize]//musze być zalogowany 
    public async Task<IActionResult> GetUserByIdAsync([FromRoute] int id)
    {
        //ten kawałek kodu pobiera username uzytkownika wiec na jego podstawie moge reszte rzeczy pobrać z bazy danych ale pamietaj 2 rzeczy
        //1 username jest zawsze unikalny
        //2 gdzieś słyszałem że trzeba uzyć app.useAuthentication zeby dało sie sprawdzać dane uzytkownika zzalogowanego ale ja uzywał tylko addIdentit i to jekims cudem
        //działa wiec nie wiem
        var username = User.Identity.Name;
        
        
        
      Console.BackgroundColor = ConsoleColor.Green;
      Console.Write(username);
        
        
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