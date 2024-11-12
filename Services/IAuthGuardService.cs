using EmployeeService.Models;

namespace EmployeeService.Services;

public interface IAuthGuardService
{
    Task<Token?> CreateTokenAsync(Login login);

    Task<Token?> CreateTokenByRefreshToken(string refreshToken);
    
    bool RevokeRefreshToken(string refreshToken);

    ClientToken? CreateTokenByClient(ClientLogin clientLogin);
}