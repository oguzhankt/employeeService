using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Models;

[PrimaryKey("Id")]
public class Employee
{
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();
    
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

    [Required]
    public bool Active { get; set; } = true;
    
}

