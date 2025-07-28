using EmployeeAttendanceTracker.Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeAttendanceTracker.Presentation.Controllers
{
    public class AttendancesController : Controller
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IEmployeeService _employeeService;

        public AttendancesController(IAttendanceService attendanceService, IEmployeeService employeeService)
        {
            _attendanceService = attendanceService;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            ViewBag.Employees = new SelectList(employees, "EmployeeCode", "FullName");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendanceStatus(int employeeId, DateTime date)
        {
            if (date.Date > DateTime.Today)
            {
                return Json(new { status = "Future Date" });
            }

            var attendance = await _attendanceService.GetAttendanceStatusAsync(employeeId, date);
            if (attendance == null)
            {
                return Json(new { status = "Not Marked" });
            }

            return Json(new { status = attendance.IsPresent ? "Present" : "Absent" });
        }

        [HttpPost]
        public async Task<IActionResult> RecordAttendance(int employeeId, DateTime date, bool isPresent)
        {
            var result = await _attendanceService.RecordAttendanceAsync(employeeId, date, isPresent);
            if (!result.Success)
            {
                return Json(new { success = false, message = result.ErrorMessage });
            }

            return Json(new { success = true });
        }
    }
}