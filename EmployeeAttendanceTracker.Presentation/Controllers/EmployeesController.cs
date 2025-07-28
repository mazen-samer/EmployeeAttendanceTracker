using EmployeeAttendanceTracker.Business.Services;
using EmployeeAttendanceTracker.Data.Models;
using EmployeeAttendanceTracker.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeAttendanceTracker.Presentation.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        public EmployeesController(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var viewModels = employees.Select(e =>
            {
                var monthAttendance = e.Attendances
                                       .Where(a => a.Date.Date >= firstDayOfMonth && a.Date.Date <= lastDayOfMonth)
                                       .ToList();

                var presentDays = monthAttendance.Count(a => a.IsPresent);
                var absentDays = monthAttendance.Count(a => !a.IsPresent);
                var totalDays = presentDays + absentDays;
                var attendancePercentage = totalDays == 0 ? 0 : (double)presentDays / totalDays * 100;

                return new EmployeeListViewModel
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

            return View(viewModels);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDepartmentsDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var result = await _employeeService.CreateEmployeeAsync(employee);
                if (result.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }
            await PopulateDepartmentsDropDownList(employee.DepartmentId);
            return View(employee);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            await PopulateDepartmentsDropDownList(employee.DepartmentId);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.EmployeeCode)
            {
                return BadRequest();
            }

            // Remove validation for EmployeeCode as it's not posted from the form
            ModelState.Remove("EmployeeCode");

            if (ModelState.IsValid)
            {
                var result = await _employeeService.UpdateEmployeeAsync(employee);
                if (result.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }
            await PopulateDepartmentsDropDownList(employee.DepartmentId);
            return View(employee);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDepartmentsDropDownList(object? selectedDepartment = null)
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            ViewBag.Departments = new SelectList(departments, "Id", "DepartmentName", selectedDepartment);
        }
    }
}