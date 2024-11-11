using System.ComponentModel.DataAnnotations;

namespace EmployeeService.Models.Requests;

public class PatchEmployeeRequest
{
    [EmailAddress]
    public string? Email { get; set; }
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    [Phone]
    public string? PhoneNumber { get; set; }
}