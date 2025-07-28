using System.Text.RegularExpressions;
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
            // Business Rule: Full Name must be four names, each at least two characters, letters and spaces only. [cite: 27]
            if (!Regex.IsMatch(employee.FullName, @"^[a-zA-Z\s]+$"))
            {
                return (false, "Full Name must contain only letters and spaces.");
            }

            var nameParts = employee.FullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (nameParts.Length != 4)
            {
                return (false, "Full Name must consist of exactly four names.");
            }

            foreach (var part in nameParts)
            {
                if (part.Length < 2)
                {
                    return (false, "Each part of the Full Name must be at least two characters long.");
                }
            }

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