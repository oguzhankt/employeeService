using System.Reflection;
using EmployeeService.Models;
using EmployeeService.Services;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // SQLite Database Provider Setup
        string? connectionString = builder.Configuration.GetConnectionString("Employee") ?? "Data Source=Employee.db";
        builder.Services.AddSqlite<EmployeeDb>(connectionString);
        
        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddScoped<IEmployeeService, Services.EmployeeService>();
        
        builder.Services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Employee Service API",
                Version = "v1",
                Description = "Employee CRUD API used to manage employee records",
            });
            
            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            config.IncludeXmlComments(xmlPath);
        });

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