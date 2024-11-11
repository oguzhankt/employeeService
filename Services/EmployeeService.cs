using EmployeeService.Controllers;
using EmployeeService.Models;
using EmployeeService.Models.Requests;

namespace EmployeeService.Services;

public class EmployeeService : IEmployeeService
{
    private readonly EmployeeDb _dbContext;

    public EmployeeService( EmployeeDb dbContext )
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Employee> GetEmployees()
    {
        var employees = _dbContext.Employee;
        return employees;
    }
    

    public async Task<bool> DeleteEmployeeAsync(Guid id)
    {
        var employee = await _dbContext.Employee.FindAsync(id);

        if (employee == null)
        {
            return false;
        }
            
        _dbContext.Remove(employee);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<Employee> AddEmployeeAsync(Employee employee)
    {
        _dbContext.Add(employee);
        await _dbContext.SaveChangesAsync();

        return employee;
    }

    public async Task<Employee?> UpdateEmployeeAsync(Guid id,PatchEmployeeRequest employee)
    {
        var findEmployee = await _dbContext.Employee.FindAsync(id);
        if (findEmployee == null)
        {
            return null;
        }
            
        findEmployee.FirstName = employee.FirstName ?? findEmployee.FirstName;
        findEmployee.LastName = employee.LastName ?? findEmployee.LastName;
        findEmployee.Email = employee.Email ?? findEmployee.Email;
        findEmployee.PhoneNumber = employee.PhoneNumber ?? findEmployee.PhoneNumber;
        await _dbContext.SaveChangesAsync();

        return findEmployee;
    }
    
    public async Task<Employee?> ToggleActiveEmployeeAsync(Guid id)
    {
        var findEmployee = await _dbContext.Employee.FindAsync(id);
        if (findEmployee == null)
        {
            return null;
        }
        
        findEmployee.Active = !findEmployee.Active;
        
        await _dbContext.SaveChangesAsync();

        return findEmployee;
    }
    
}