using Microsoft.AspNetCore.Identity;
using OrderManagementSystem.Dtos;
using OrderManagementSystem.Entities;

namespace OrderManagementSystem.Services;

using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class UserService
{
    private readonly SignInManager<User> _signInManager;
    
    private readonly UserManager<User> _userManager;

    
    //dodaje jeszcze dodatkowo _roleManager 
    //🔹 RoleManager<TRole> To serwis, który zarządza rolami jako bytami w systemie: Sprawdza, czy rola istnieje (RoleExistsAsync) Tworzy nowe role (CreateAsync) Usuwa i modyfikuje role
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public UserService(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }

    public async Task<User> CreateUserAsync(CreateUserDto userDto,string role)
    {
        User user = new User() 
        { 
            UserName = userDto.UserName, 
            Age = userDto.Age 
        };

        IdentityResult result = await _userManager.CreateAsync(user, userDto.Password);//hasło jest hashowane wiec nie podaje hasła w obiekcie user 

        if (result.Succeeded)
        {
            // Przypisanie roli do użytkownika
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)//Ogólnie żeby nadać role uzytkownikowi to trzeba najpierw ją zapisać do bazy danych IdentityRole wiec to nie jest jak w springu ze jaką chce daje
            //ogólnie najlepiej by było jakbym na początku dodał odpowiednie role w seed tutaj robie poprostu dynamicznie bo mi sie nie chce.
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole<int> { Name = role });
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException("Role creation failed");
                }
            }

            await _userManager.AddToRoleAsync(user, role);
            return user;
        }

        throw new InvalidOperationException("Failed to create user");
    
    }
    
    public async Task LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);
        if (user == null)
            throw new InvalidOperationException("Invalid username or password");

        var result = await _signInManager.PasswordSignInAsync(user, dto.Password, isPersistent: false, lockoutOnFailure: false);

        if (!result.Succeeded)
            throw new InvalidOperationException("Invalid username or password");
    }
    
    
    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
    

    public async Task<User> UpdateUserAsync(int age, int userId)
    {
        var existingUser = await _userManager.FindByIdAsync(userId.ToString());
        if (existingUser == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        existingUser.Age = age;
        IdentityResult result = await _userManager.UpdateAsync(existingUser);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException("Failed to update user");

        }

        return existingUser;

    }


    public async Task<User> DeleteUserAsync(int userId)
    {
        var existingUser = await _userManager.FindByIdAsync(userId.ToString());

        if (existingUser == null)
        {
            throw new InvalidOperationException("User not found.");
        }


        IdentityResult result = await _userManager.DeleteAsync(existingUser);
        if (result.Succeeded)
        {
            return existingUser;
        }

        throw new InvalidOperationException("Failed to delete user");
    }


    public async Task<User> GetUserByIdAsync(int id)
    {
        User? user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) throw new InvalidOperationException("User not found");

        return user;
    }
}

