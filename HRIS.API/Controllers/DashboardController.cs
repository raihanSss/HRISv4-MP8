using HRIS.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRIS.API.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [Authorize]
        [HttpGet("Getdashboard")]
        public async Task<IActionResult> GetDashboard(int actorId)
        {
            var dashboardData = await _dashboardService.GetDashboardDataAsync(actorId);
            return Ok(dashboardData);
        }
    }
}
