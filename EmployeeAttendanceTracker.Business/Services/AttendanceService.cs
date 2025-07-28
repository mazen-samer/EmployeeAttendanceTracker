using EmployeeAttendanceTracker.Data.Models;
using EmployeeAttendanceTracker.Data.Repositories;

namespace EmployeeAttendanceTracker.Business.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceService(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task<Attendance?> GetAttendanceStatusAsync(int employeeId, DateTime date)
        {
            return await _attendanceRepository.GetByEmployeeAndDateAsync(employeeId, date);
        }

        public async Task<(bool Success, string ErrorMessage)> RecordAttendanceAsync(int employeeId, DateTime date, bool isPresent)
        {
            // Business Rule: Attendance cannot be marked for future dates. [cite: 34]
            if (date.Date > DateTime.Today)
            {
                return (false, "Cannot record attendance for a future date.");
            }

            // Business Rule: Each employee has only one attendance per the same day. [cite: 33]
            var existingAttendance = await _attendanceRepository.GetByEmployeeAndDateAsync(employeeId, date);

            if (existingAttendance != null)
            {
                // Update existing record
                existingAttendance.IsPresent = isPresent;
                await _attendanceRepository.UpdateAsync(existingAttendance);
            }
            else
            {
                // Create new record
                var newAttendance = new Attendance
                {
                    EmployeeId = employeeId,
                    Date = date.Date,
                    IsPresent = isPresent
                };
                await _attendanceRepository.AddAsync(newAttendance);
            }

            return (true, string.Empty);
        }
    }
}