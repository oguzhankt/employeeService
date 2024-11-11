using EmployeeService.Models;
using EmployeeService.Models.Requests;

namespace EmployeeService.Services;

public interface IEmployeeService
{
    public IEnumerable<Employee> GetEmployees();
    public Task<bool> DeleteEmployeeAsync(Guid id);
    public Task<Employee> AddEmployeeAsync(Employee employee);
    public Task<Employee?> UpdateEmployeeAsync(Guid id,PatchEmployeeRequest employee);
    
    public Task<Employee?> ToggleActiveEmployeeAsync(Guid id);
    
}