using Microsoft.AspNetCore.Identity;
using OrderManagementSystem.Dtos;
using OrderManagementSystem.Entities;

namespace OrderManagementSystem.Services;

using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class UserService
{
    private readonly UserManager<User> _userManager;
    //dodaje jeszcze dodatkowo _roleManager 
    //🔹 RoleManager<TRole> To serwis, który zarządza rolami jako bytami w systemie: Sprawdza, czy rola istnieje (RoleExistsAsync) Tworzy nowe role (CreateAsync) Usuwa i modyfikuje role
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public UserService(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<User> CreateUserAsync(CreateUserDto userDto)
    {
        User user = new User() 
        { 
            UserName = userDto.UserName, 
            Age = userDto.Age 
        };

        IdentityResult result = await _userManager.CreateAsync(user, userDto.Password);//hasło jest hashowane wiec nie podaje hasła w obiekcie user 

        if (result.Succeeded)
        {
            return user;
        }
        var errors = string.Join("; ", result.Errors.Select(e => e.Description));
        throw new InvalidOperationException(errors);


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

