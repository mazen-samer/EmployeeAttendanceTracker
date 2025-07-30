using EmployeeAttendanceTracker.Data.Models;

namespace EmployeeAttendanceTracker.Business.Services
{
    public interface IAttendanceService
    {
        Task<Attendance?> GetAttendanceStatusAsync(int employeeId, DateTime date);
        Task<(bool Success, string ErrorMessage)> RecordAttendanceAsync(int employeeId, DateTime date, bool isPresent);
        Task<(bool Success, string ErrorMessage)> DeleteAttendanceAsync(int employeeId, DateTime date);
        Task<IEnumerable<Attendance>> GetFilteredAttendanceAsync(int? departmentId, int? employeeId, DateTime? startDate, DateTime? endDate); // <-- ADD THIS


    }
}