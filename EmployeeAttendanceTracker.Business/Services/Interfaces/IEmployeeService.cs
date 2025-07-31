using EmployeeAttendanceTracker.Business.DTOs;
using EmployeeAttendanceTracker.Data.Models;

namespace EmployeeAttendanceTracker.Business.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeSummaryDto>> GetEmployeeSummariesAsync();
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int employeeCode);
        Task<(bool Success, string ErrorMessage)> CreateEmployeeAsync(Employee employee);
        Task<(bool Success, string ErrorMessage)> UpdateEmployeeAsync(Employee employee);
        Task<(bool Success, string ErrorMessage)> DeleteEmployeeAsync(int employeeCode);
    }
}