using Inventory.Entities.DTOs;

namespace Inventory.Business.Services;

public interface IClienteArticuloService
{
    Task<IEnumerable<ClienteArticuloListItemDto>> GetByClienteAsync(int clienteId);
    Task CreateAsync(ClienteArticuloCreateDto dto);
    Task DeleteAsync(int clienteId, int articuloId, DateTime fecha);
}
