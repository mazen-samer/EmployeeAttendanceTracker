using EmployeeAttendanceTracker.Data.Models;

namespace EmployeeAttendanceTracker.Business.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<Department?> GetDepartmentByIdAsync(int id);
        Task<(bool Success, string ErrorMessage)> CreateDepartmentAsync(Department department);
        Task<(bool Success, string ErrorMessage)> UpdateDepartmentAsync(Department department);
        Task<(bool Success, string ErrorMessage)> DeleteDepartmentAsync(int id);
    }
}