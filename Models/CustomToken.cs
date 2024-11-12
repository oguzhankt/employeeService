namespace EmployeeService.Models;

public class CustomToken
{
    public List<String> Audience { get; set; }
    public string Issuer { get; set; }

    public int AccessTokenExpiration { get; set; }
    public int RefreshTokenExpiration { get; set; }

    public string SecurityKey { get; set; }
}