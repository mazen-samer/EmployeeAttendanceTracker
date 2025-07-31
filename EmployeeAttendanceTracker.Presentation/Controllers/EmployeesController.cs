using EmployeeAttendanceTracker.Business.Services;
using EmployeeAttendanceTracker.Data.Models;
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
            var employeeSummaries = await _employeeService.GetEmployeeSummariesAsync();
            return View(employeeSummaries);
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