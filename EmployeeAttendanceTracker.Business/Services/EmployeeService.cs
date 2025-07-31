using EmployeeAttendanceTracker.Business.DTOs;
using EmployeeAttendanceTracker.Data.Models;
using EmployeeAttendanceTracker.Data.Repositories;

namespace EmployeeAttendanceTracker.Business.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<EmployeeSummaryDto>> GetEmployeeSummariesAsync()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();

            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var summaries = employees.Select(e =>
            {
                var monthAttendance = e.Attendances
                    .Where(a => a.Date.Date >= firstDayOfMonth && a.Date.Date <= lastDayOfMonth)
                    .ToList();

                var presentDays = monthAttendance.Count(a => a.IsPresent);
                var absentDays = monthAttendance.Count(a => !a.IsPresent);
                var totalDays = presentDays + absentDays;
                var attendancePercentage = totalDays == 0 ? 0 : (double)presentDays / totalDays * 100;

                return new EmployeeSummaryDto
                {
                    EmployeeCode = e.EmployeeCode,
                    FullName = e.FullName,
                    Email = e.Email,
                    DepartmentName = e.Department?.DepartmentName ?? "N/A",
                    PresentDays = presentDays,
                    AbsentDays = absentDays,
                    AttendancePercentage = Math.Round(attendancePercentage, 2)
                };
            }).ToList();

            return summaries;
        }
        public async Task<(bool Success, string ErrorMessage)> CreateEmployeeAsync(Employee employee)
        {
            var validationResult = await ValidateEmployee(employee);
            if (!validationResult.Success)
            {
                return validationResult;
            }

            await _employeeRepository.AddEmployeeAsync(employee);
            return (true, string.Empty);
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteEmployeeAsync(int employeeCode)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeCode);
            if (employee == null)
            {
                return (false, "Employee not found.");
            }

            await _employeeRepository.DeleteEmployeeAsync(employee);
            return (true, string.Empty);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllEmployeesAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeCode)
        {
            return await _employeeRepository.GetEmployeeByIdAsync(employeeCode);
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateEmployeeAsync(Employee employee)
        {
            var validationResult = await ValidateEmployee(employee, isUpdate: true);
            if (!validationResult.Success)
            {
                return validationResult;
            }

            await _employeeRepository.UpdateEmployeeAsync(employee);
            return (true, string.Empty);
        }

        private async Task<(bool Success, string ErrorMessage)> ValidateEmployee(Employee employee, bool isUpdate = false)
        {
            // Business Rule: Prevent duplicate email addresses. [cite: 29]
            int? employeeCodeToExclude = isUpdate ? employee.EmployeeCode : null;
            if (await _employeeRepository.EmailExistsAsync(employee.Email, employeeCodeToExclude))
            {
                return (false, "An employee with this email already exists.");
            }

            return (true, string.Empty);
        }
    }
}