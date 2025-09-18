using Inventory.Data;
using Inventory.Entities.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Business.Services;

public class ClienteArticuloService(AppDbContext db) : IClienteArticuloService
{
    public async Task<IEnumerable<ClienteArticuloListItemDto>> GetByClienteAsync(int clienteId)
    {
        return await db.ClienteArticulos
            .Include(x => x.Cliente).Include(x => x.Articulo)
            .Where(x => x.ClienteId == clienteId)
            .OrderByDescending(x => x.Fecha)
            .Select(x => new ClienteArticuloListItemDto(
                x.ClienteId,
                x.Cliente.Nombre + " " + x.Cliente.Apellidos,
                x.ArticuloId,
                x.Articulo.Codigo,
                x.Articulo.Descripcion,
                x.Fecha
            )).ToListAsync();
    }

    public async Task<IEnumerable<ClienteArticuloListItemDto>> GetByArticuloAsync(int articuloId)
    {
        return await db.ClienteArticulos
            .Include(x => x.Cliente).Include(x => x.Articulo)
            .Where(x => x.ArticuloId == articuloId)
            .OrderByDescending(x => x.Fecha)
            .Select(x => new ClienteArticuloListItemDto(
                x.ClienteId,
                x.Cliente.Nombre + " " + x.Cliente.Apellidos,
                x.ArticuloId,
                x.Articulo.Codigo,
                x.Articulo.Descripcion,
                x.Fecha
            )).ToListAsync();
    }

    public async Task CreateAsync(ClienteArticuloCreateDto dto)
    {
        var entity = new Inventory.Entities.Models.ClienteArticulo
        {
            ClienteId = dto.ClienteId,
            ArticuloId = dto.ArticuloId,
            Fecha = dto.Fecha ?? DateTime.UtcNow
        };
        db.ClienteArticulos.Add(entity);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int clienteId, int articuloId, DateTime fecha)
    {
        var entity = await db.ClienteArticulos
            .FirstOrDefaultAsync(x => x.ClienteId == clienteId && x.ArticuloId == articuloId && x.Fecha == fecha)
            ?? throw new KeyNotFoundException("Registro Cliente-Articulo no encontrado.");
        db.ClienteArticulos.Remove(entity);
        await db.SaveChangesAsync();
    }
}
