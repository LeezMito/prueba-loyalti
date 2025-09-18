using Inventory.Data;
using Inventory.Entities.DTOs;
using Inventory.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Business.Services;

public class ClienteArticuloService(AppDbContext db) : IClienteArticuloService
{
    public async Task<IEnumerable<ClienteArticuloListItemDto>> GetByClienteAsync(int clienteId)
    {
        return await db.ClienteArticulos
            .AsNoTracking()
            .Include(x => x.Cliente)
            .Include(x => x.Articulo)
            .Where(x => x.ClienteId == clienteId)
            .OrderByDescending(x => x.ArticuloId)
            .Select(x => new ClienteArticuloListItemDto(
                x.ClienteId,
                x.Cliente.Nombre + " " + x.Cliente.Apellidos,
                x.ArticuloId,
                x.Articulo.Codigo,
                x.Articulo.Descripcion,
                x.Fecha,
                db.ArticuloTiendas
                    .Where(at => at.ArticuloId == x.ArticuloId)
                    .OrderByDescending(at => at.Articulo.Stock)
                    .ThenBy(at => at.Tienda.Sucursal)
                    .Select(at => (int?)at.TiendaId)
                    .FirstOrDefault(),
                db.ArticuloTiendas
                    .Where(at => at.ArticuloId == x.ArticuloId)
                    .OrderByDescending(at => at.Articulo.Stock)
                    .ThenBy(at => at.Tienda.Sucursal)
                    .Select(at => at.Tienda.Sucursal)
                    .FirstOrDefault(),
                x.Articulo.Precio,
                x.Articulo.ImagenUrl,
                db.ArticuloTiendas
                    .Where(at => at.ArticuloId == x.ArticuloId)
                    .OrderByDescending(at => at.Articulo.Stock)
                    .ThenBy(at => at.Tienda.Sucursal)
                    .Select(at => at.Articulo.Stock)
                    .FirstOrDefault()
            ))
            .ToListAsync();
    }

    public async Task CreateAsync(ClienteArticuloCreateDto dto)
    {
        var entity = new ClienteArticulo
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
