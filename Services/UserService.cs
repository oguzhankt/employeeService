using EmployeeService.Models;
using EmployeeService.Models.Requests;
using Microsoft.AspNetCore.Identity;

namespace EmployeeService.Services;

public class UserService : IUserService
{
    private readonly UserManager<AuthGuardUser> _userManager;

    public UserService(UserManager<AuthGuardUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AuthGuardUser?> CreateUserAsync(CreateUser createUser)
    {
        var user = new AuthGuardUser { Email = createUser.Email, UserName = createUser.UserName };

        var result = await _userManager.CreateAsync(user, createUser.Password);

        if (!result.Succeeded)
        {
            return null;
        }
        return user;
    }

    public async Task<AuthGuardUser?> GetUserByNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        
        return user;
    }
}