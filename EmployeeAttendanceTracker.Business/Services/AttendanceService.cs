﻿using EmployeeAttendanceTracker.Data.Models;
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
        public async Task<(bool Success, string ErrorMessage)> DeleteAttendanceAsync(int employeeId, DateTime date)
        {
            if (date.Date > DateTime.Today)
            {
                return (false, "Cannot modify attendance for a future date.");
            }

            var existingAttendance = await _attendanceRepository.GetByEmployeeAndDateAsync(employeeId, date);

            if (existingAttendance != null)
            {
                await _attendanceRepository.DeleteAsync(existingAttendance);
            }
            // If it doesn't exist, there's nothing to delete, so it's a success.
            return (true, string.Empty);
        }

        // ADD THIS METHOD to your AttendanceService class

        public async Task<IEnumerable<Attendance>> GetFilteredAttendanceAsync(int? departmentId, int? employeeId, DateTime? startDate, DateTime? endDate)
        {
            return await _attendanceRepository.GetFilteredAsync(departmentId, employeeId, startDate, endDate);
        }
    }
}
