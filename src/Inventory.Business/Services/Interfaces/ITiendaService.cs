using Inventory.Entities.DTOs;

namespace Inventory.Business.Services;

public interface ITiendaService
{
    Task<IEnumerable<TiendaListItemDto>> GetAllAsync();
    Task<TiendaDetailDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(TiendaCreateDto dto);
    Task UpdateAsync(int id, TiendaUpdateDto dto);
    Task DeleteAsync(int id);
}
