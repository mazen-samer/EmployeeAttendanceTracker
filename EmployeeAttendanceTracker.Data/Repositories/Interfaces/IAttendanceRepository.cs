using EmployeeAttendanceTracker.Data.Models;

namespace EmployeeAttendanceTracker.Data.Repositories
{
    public interface IAttendanceRepository
    {
        Task<Attendance?> GetByEmployeeAndDateAsync(int employeeId, DateTime date);
        Task<IEnumerable<Attendance>> GetFilteredAsync(int? departmentId, int? employeeId, DateTime? startDate, DateTime? endDate);
        Task AddAsync(Attendance attendance);
        Task UpdateAsync(Attendance attendance);
        Task DeleteAsync(Attendance attendance);
    }
}