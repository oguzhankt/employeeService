using System.ComponentModel.DataAnnotations;

namespace EmployeeService.Models.Requests;

public class CreateEmployeeRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
}