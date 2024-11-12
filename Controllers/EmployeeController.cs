using System.Diagnostics;
using EmployeeService.Models;
using EmployeeService.Models.Requests;
using EmployeeService.Models.Responses;
using EmployeeService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Controllers;

/// <summary>
/// Controller for employee CRUD operations
/// </summary>
[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EmployeeController : Controller
{
    private readonly ILogger<EmployeeController> _logger;
    private readonly IEmployeeService _employeeService;

    public EmployeeController(
        ILogger<EmployeeController> logger,
        IEmployeeService employeeService
        )
    {
        _logger = logger;
        _employeeService = employeeService;
    }

    /// <summary>
    /// Creates employee
    /// </summary>
    /// <param name="request">CreateEmployeeRequest</param>
    /// <returns>CreateEmployeeResponse</returns>
    [Route("")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployee(CreateEmployeeRequest request)
    {
        try
        {
            var newEmployee = new Employee()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var createdEmployee = await _employeeService.AddEmployeeAsync(newEmployee);
            
            var location = Url.Action(nameof(CreateEmployee), new { id = newEmployee.Id }) ?? $"/{newEmployee.Id}";
            return Created(location,createdEmployee);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error" + ex.Message);
        }
    }
    
    /// <summary>
    /// Get a list of all employees
    /// </summary>
    /// <returns></returns>
    [Route("")]
    [HttpGet]
    public IActionResult GetEmployees()
    {
        try
        {
            var employees = _employeeService.GetEmployees();
            return Ok(employees);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error" + ex.Message);
        }
    }
    
    /// <summary>
    /// Deletes an employee permanently
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Route("{id:guid}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteEmployee(Guid id)
    {
        try
        {
            bool success = await _employeeService.DeleteEmployeeAsync(id);
            
            return success ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error" + ex.Message);
        }
    }
    
    /// <summary>
    /// Updates an employee data with requested fields
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [Route("{id:guid}")]
    [HttpPatch]
    public async Task<IActionResult> PatchEmployee(Guid id, PatchEmployeeRequest request)
    {
        try
        {
            var employee = await _employeeService.UpdateEmployeeAsync(id, request);
            
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error :" + ex.Message);
        }
    }
    
    /// <summary>
    /// Toggles an employees active status
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Route("{id:guid}/toggleActive")]
    [HttpPost]
    public async Task<IActionResult> ToggleActiveEmployee(Guid id)
    {
        try
        {
            var employee = await _employeeService.ToggleActiveEmployeeAsync(id);
            
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error" + ex.Message);
        }
    }
}