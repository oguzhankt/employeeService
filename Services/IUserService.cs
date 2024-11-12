using EmployeeService.Models;
using EmployeeService.Models.Requests;

namespace EmployeeService.Services;

public interface IUserService
{
    Task<AuthGuardUser?> CreateUserAsync(CreateUser createUser);

    Task<AuthGuardUser?> GetUserByNameAsync(string userName);
}