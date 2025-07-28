using EmployeeAttendanceTracker.Business.Services;
using EmployeeAttendanceTracker.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAttendanceTracker.Presentation.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: /Departments
        public async Task<IActionResult> Index()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return View(departments);
        }

        // GET: /Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                var result = await _departmentService.CreateDepartmentAsync(department);
                if (result.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                // Add the error from the service to the model state to display to the user
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }
            return View(department);
        }

        // GET: /Departments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: /Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var result = await _departmentService.UpdateDepartmentAsync(department);
                if (result.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }
            return View(department);
        }

        // GET: /Departments/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: /Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _departmentService.DeleteDepartmentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}