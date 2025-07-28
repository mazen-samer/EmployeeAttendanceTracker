using EmployeeAttendanceTracker.Data.Models;

namespace EmployeeAttendanceTracker.Data.Repositories
{
    public interface IDepartmentRepository
    {
        Task<Department?> GetDepartmentByIdAsync(int id);
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task AddDepartmentAsync(Department department);
        Task UpdateDepartmentAsync(Department department);
        Task DeleteDepartmentAsync(Department department);
        Task<bool> DepartmentExistsAsync(string name, string code, int? excludeId = null);
    }
}