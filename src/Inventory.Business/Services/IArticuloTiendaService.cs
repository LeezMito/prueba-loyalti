using Inventory.Entities.DTOs;

namespace Inventory.Business.Services;

public interface IArticuloTiendaService
{
    Task<IEnumerable<ArticuloTiendaListItemDto>> GetAsync(int? tiendaId = null, int? articuloId = null);
    Task<ArticuloTiendaListItemDto?> GetOneAsync(int articuloId, int tiendaId);
    Task CreateAsync(ArticuloTiendaCreateDto dto);
    Task UpdateAsync(int articuloId, int tiendaId, ArticuloTiendaUpdateDto dto);
    Task DeleteAsync(int articuloId, int tiendaId);
}
