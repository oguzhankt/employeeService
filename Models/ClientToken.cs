namespace EmployeeService.Models;

public class ClientToken
{
    public string AccessToken { get; set; }

    public DateTime AccessTokenExpiration { get; set; }
}