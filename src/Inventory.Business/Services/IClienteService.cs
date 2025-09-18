using Inventory.Entities.DTOs;

namespace Inventory.Business.Services;

public interface IClienteService
{
    Task<IEnumerable<ClienteListItemDto>> GetAllAsync();
    Task<ClienteDetailDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(ClienteCreateDto dto);
    Task UpdateAsync(int id, ClienteUpdateDto dto);
    Task DeleteAsync(int id);
}
