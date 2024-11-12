using EmployeeService.Models;

namespace EmployeeService.Services;

public interface ITokenService
{
    Token CreateToken(AuthGuardUser user);

    ClientToken CreateTokenByClient(Client client);
}