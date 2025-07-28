using EmployeeAttendanceTracker.Data.Models;

namespace EmployeeAttendanceTracker.Data.Repositories
{
    public interface IAttendanceRepository
    {
        Task<Attendance?> GetByEmployeeAndDateAsync(int employeeId, DateTime date);
        Task AddAsync(Attendance attendance);
        Task UpdateAsync(Attendance attendance);
        Task DeleteAsync(Attendance attendance);
    }
}