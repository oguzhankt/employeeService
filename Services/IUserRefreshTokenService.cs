using EmployeeService.Models;

namespace EmployeeService.Services;

public interface IUserRefreshTokenService
{
    public UserRefreshToken? GetUserRefreshTokenById(string id);
    public UserRefreshToken? GetUserRefreshTokenByCode(string code);
    public Task<bool> AddUserRefreshToken(UserRefreshToken userRefreshToken);
    public bool RemoveUserRefreshTokenById(string id);
}