using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Models;

[PrimaryKey("UserId")]
public class UserRefreshToken
{
    [Required]
    public string UserId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Code { get; set; }
    public DateTime Expiration { get; set; }
}