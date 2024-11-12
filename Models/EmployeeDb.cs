using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Models;

public class EmployeeDb(DbContextOptions<EmployeeDb> options) : DbContext(options)
{
    public DbSet<Employee> Employee { get; set; } = null!;
}