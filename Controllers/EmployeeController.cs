using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.Controllers;

/// <summary>
/// Controller for employee CRUD operations
/// </summary>
public class EmployeeController : Controller
{
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(ILogger<EmployeeController> logger)
    {
        _logger = logger;
    }
    
}