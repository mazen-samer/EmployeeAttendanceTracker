namespace EmployeeAttendanceTracker.Business.DTOs
{
    public class EmployeeSummaryDto
    {
        public int EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string DepartmentName { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public double AttendancePercentage { get; set; }
    }
}