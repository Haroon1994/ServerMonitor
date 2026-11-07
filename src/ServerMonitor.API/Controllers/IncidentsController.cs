namespace ServerMonitor.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using ServerMonitor.Application.DTOs;
using ServerMonitor.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class IncidentsController(IIncidentService incidentService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<IncidentDto>>> GetAll([FromQuery] int limit = 20)
    {
        return await incidentService.GetAllAsync(limit);
    }

    [HttpGet("server/{serverId:guid}")]
    public async Task<ActionResult<List<IncidentDto>>> GetByServerId(Guid serverId)
    {
        return await incidentService.GetByServerIdAsync(serverId);
    }
}
