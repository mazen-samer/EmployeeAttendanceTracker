using EmployeeAttendanceTracker.Data.Models;

namespace EmployeeAttendanceTracker.Data.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetEmployeeByIdAsync(int employeeCode);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task AddEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(Employee employee);
        Task<bool> EmailExistsAsync(string email, int? excludeEmployeeCode = null);
    }
}