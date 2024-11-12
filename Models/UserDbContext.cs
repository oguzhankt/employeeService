using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace EmployeeService.Models;

public class UserDb : IdentityDbContext<AuthGuardUser, IdentityRole, string>
{
    public UserDb(DbContextOptions<UserDb> options) : base(options)

    {
    }
    
    public DbSet<UserRefreshToken> UserRefreshToken { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(builder);
    }
}