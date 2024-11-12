using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeService.Helpers;

public class SignHelper
{
    public static SecurityKey GetSymmetricSecurityKey(string securityKey)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    }
}