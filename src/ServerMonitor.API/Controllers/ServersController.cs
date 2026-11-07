namespace ServerMonitor.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using ServerMonitor.Application.DTOs;
using ServerMonitor.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class ServersController(IServerService serverService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ServerDto>>> GetAll()
    {
        return await serverService.GetAllAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServerDto>> GetById(Guid id)
    {
        var server = await serverService.GetByIdAsync(id);
        return server is null ? NotFound() : server;
    }

    [HttpPost]
    public async Task<ActionResult<ServerDto>> Create(CreateServerDto dto)
    {
        var server = await serverService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = server.Id }, server);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ServerDto>> Update(Guid id, UpdateServerDto dto)
    {
        var server = await serverService.UpdateAsync(id, dto);
        return server is null ? NotFound() : server;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await serverService.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
