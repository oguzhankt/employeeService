using System.Reflection;
using EmployeeService.Helpers;
using EmployeeService.Models;
using EmployeeService.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<UserDb>();
        
        builder.Services.AddIdentity<AuthGuardUser, IdentityRole>(Opt =>
        {
            Opt.User.RequireUniqueEmail = true;
            Opt.Password.RequireNonAlphanumeric = false;
        }).AddEntityFrameworkStores<UserDb>().AddDefaultTokenProviders();
        
        builder.Services.Configure<CustomToken>(builder.Configuration.GetSection("TokenOption"));
        // Add jwt authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
        {
            var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomToken>();
            opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                ValidIssuer = tokenOptions.Issuer,
                ValidAudience = tokenOptions.Audience[0],
                IssuerSigningKey = SignHelper.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });
        
        // SQLite Database Setup
        string? connectionString = builder.Configuration.GetConnectionString("Employee") ?? "Data Source=Employee.db";
        builder.Services.AddSqlite<EmployeeDb>(connectionString);
        
        string? connectionStringRefreshToken = builder.Configuration.GetConnectionString("User") ?? "Data Source=User.db";
        builder.Services.AddSqlite<UserDb>(connectionStringRefreshToken);
        
        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddScoped<IEmployeeService, Services.EmployeeService>();
        builder.Services.AddScoped<IUserRefreshTokenService, UserRefreshTokenService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IAuthGuardService, AuthGuardService>();
        
        builder.Services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Employee Service API",
                Version = "v1",
                Description = "Employee CRUD API used to manage employee records",
            });
            
            config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            config.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });
            
            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            config.IncludeXmlComments(xmlPath);
        });

        var app = builder.Build();
        
        
        app.UseAuthentication();
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