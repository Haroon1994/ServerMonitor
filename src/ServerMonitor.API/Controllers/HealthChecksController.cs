namespace ServerMonitor.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using ServerMonitor.Application.DTOs;
using ServerMonitor.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class HealthChecksController(IHealthCheckService healthCheckService) : ControllerBase
{
    [HttpGet("server/{serverId:guid}")]
    public async Task<ActionResult<List<HealthCheckDto>>> GetByServerId(Guid serverId, [FromQuery] int limit = 50)
    {
        return await healthCheckService.GetByServerIdAsync(serverId, limit);
    }

    [HttpPost("server/{serverId:guid}")]
    public async Task<ActionResult<HealthCheckDto>> RecordCheck(Guid serverId)
    {
        return await healthCheckService.RecordCheckAsync(serverId);
    }
}
