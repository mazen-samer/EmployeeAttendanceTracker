using EmployeeAttendanceTracker.Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeAttendanceTracker.Presentation.Controllers
{
    public class AttendanceReportController : Controller
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IDepartmentService _departmentService;
        private readonly IEmployeeService _employeeService;

        public AttendanceReportController(IAttendanceService attendanceService, IDepartmentService departmentService, IEmployeeService employeeService)
        {
            _attendanceService = attendanceService;
            _departmentService = departmentService;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index(int? departmentId, int? employeeId, DateTime? startDate, DateTime? endDate)
        {
            var filteredAttendance = await _attendanceService.GetFilteredAttendanceAsync(departmentId, employeeId, startDate, endDate);

            // Populate dropdowns for the filter form
            ViewBag.Departments = new SelectList(await _departmentService.GetAllDepartmentsAsync(), "Id", "DepartmentName", departmentId);
            ViewBag.Employees = new SelectList(await _employeeService.GetAllEmployeesAsync(), "EmployeeCode", "FullName", employeeId);

            // Store current filter values to re-populate the form
            ViewData["currentDepartmentId"] = departmentId;
            ViewData["currentEmployeeId"] = employeeId;
            ViewData["currentStartDate"] = startDate?.ToString("yyyy-MM-dd");
            ViewData["currentEndDate"] = endDate?.ToString("yyyy-MM-dd");

            return View(filteredAttendance);
        }
    }
}