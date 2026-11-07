namespace ServerMonitor.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using ServerMonitor.Application.DTOs;
using ServerMonitor.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(IDashboardService dashboardService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<DashboardDto>> Get()
    {
        return await dashboardService.GetDashboardAsync();
    }
}
