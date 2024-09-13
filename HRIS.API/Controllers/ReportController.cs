using HRIS.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRIS.API.Controllers
{
    [Route("api/report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public ReportController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        [HttpGet("empperdept")]
        public async Task<IActionResult> GenerateEmployeeReportPdf(int departmentId, int pageNumber)
        {
            var pdfBytes = await _employeeService.GenerateEmployeeReportPdfAsync(departmentId, pageNumber);
            return File(pdfBytes, "application/pdf", $"EmployeeReport_Department{departmentId}_Page{pageNumber}.pdf");
        }

        [HttpGet("leave-report")]
        public async Task<IActionResult> GenerateLeaveReport(DateOnly startDate, DateOnly endDate)
        {
            var pdfBytes = await _employeeService.GenerateLeaveReportPdfAsync(startDate, endDate);
            return File(pdfBytes, "application/pdf", $"LeaveReport_{startDate}_to_{endDate}.pdf");
        }

        [HttpGet("project-report")]
        public async Task<IActionResult> GenerateProjectReport(int projectId)
        {
            var pdfBytes = await _employeeService.GenerateProjectReportPdfAsync(projectId);
            return File(pdfBytes, "application/pdf", $"ProjectReport_{projectId}.pdf");
        }
    }
}
