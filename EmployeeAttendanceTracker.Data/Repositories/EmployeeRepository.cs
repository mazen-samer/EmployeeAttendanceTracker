using EmployeeAttendanceTracker.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAttendanceTracker.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeEmployeeCode = null)
        {
            var query = _context.Employees.AsQueryable();

            if (excludeEmployeeCode.HasValue)
            {
                query = query.Where(e => e.EmployeeCode != excludeEmployeeCode.Value);
            }

            return await query.AnyAsync(e => e.Email == email);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            // Include Department to display the department name in the list
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Attendances)
                .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeCode)
        {
            return await _context.Employees.FindAsync(employeeCode);
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}