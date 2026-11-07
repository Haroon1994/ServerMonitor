namespace ServerMonitor.Application.Interfaces;

using ServerMonitor.Application.DTOs;

public interface IServerService
{
    Task<List<ServerDto>> GetAllAsync();
    Task<ServerDto?> GetByIdAsync(Guid id);
    Task<ServerDto> CreateAsync(CreateServerDto dto);
    Task<ServerDto?> UpdateAsync(Guid id, UpdateServerDto dto);
    Task<bool> DeleteAsync(Guid id);
}
