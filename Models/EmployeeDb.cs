using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Models;

public class EmployeeDb(DbContextOptions options) : DbContext(options)
{
    public DbSet<Employee> Employee { get; set; } = null!;
}