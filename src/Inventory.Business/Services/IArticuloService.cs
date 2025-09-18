using Inventory.Entities.DTOs;

namespace Inventory.Business.Services;

public interface IArticuloService
{
    Task<IEnumerable<ArticuloListItemDto>> GetAllAsync();
    Task<ArticuloDetailDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(ArticuloCreateDto dto);
    Task UpdateAsync(int id, ArticuloUpdateDto dto);
    Task DeleteAsync(int id);
}
