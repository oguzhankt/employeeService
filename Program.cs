namespace EmployeeService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        
        app.UseRouting();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllerRoute(
            name: "default",
            pattern: "");

        app.Run();
    }
}