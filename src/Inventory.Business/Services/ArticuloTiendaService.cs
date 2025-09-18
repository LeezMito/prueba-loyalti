using Inventory.Data;
using Inventory.Entities.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Business.Services;

public class ArticuloTiendaService(AppDbContext db) : IArticuloTiendaService
{
    public async Task<IEnumerable<ArticuloTiendaListItemDto>> GetAsync(int? tiendaId = null, int? articuloId = null)
    {
        var q = db.ArticuloTiendas
            .Include(x => x.Articulo)
            .Include(x => x.Tienda)
            .AsQueryable();

        if (tiendaId.HasValue) q = q.Where(x => x.TiendaId == tiendaId.Value);
        if (articuloId.HasValue) q = q.Where(x => x.ArticuloId == articuloId.Value);

        return await q
            .OrderBy(x => x.Tienda.Sucursal).ThenBy(x => x.Articulo.Codigo)
            .Select(x => new ArticuloTiendaListItemDto(
                x.ArticuloId,
                x.Articulo.Codigo,
                x.Articulo.Descripcion,
                x.TiendaId,
                x.Tienda.Sucursal,
                x.Stock,
                x.Articulo.Precio,
                x.FechaAlta
            ))
            .ToListAsync();
    }

    public async Task<ArticuloTiendaListItemDto?> GetOneAsync(int articuloId, int tiendaId)
    {
        return await db.ArticuloTiendas
            .Include(x => x.Articulo).Include(x => x.Tienda)
            .Where(x => x.ArticuloId == articuloId && x.TiendaId == tiendaId)
            .Select(x => new ArticuloTiendaListItemDto(
                x.ArticuloId, x.Articulo.Codigo, x.Articulo.Descripcion,
                x.TiendaId, x.Tienda.Sucursal,
                x.Stock, x.Articulo.Precio, x.FechaAlta
            )).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(ArticuloTiendaCreateDto dto)
    {
        var entity = new Inventory.Entities.Models.ArticuloTienda
        {
            ArticuloId = dto.ArticuloId,
            TiendaId = dto.TiendaId,
            Stock = dto.Stock,
            FechaAlta = dto.FechaAlta ?? DateTime.UtcNow.Date
        };
        db.ArticuloTiendas.Add(entity);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(int articuloId, int tiendaId, ArticuloTiendaUpdateDto dto)
    {
        var entity = await db.ArticuloTiendas.FindAsync(articuloId, tiendaId)
            ?? throw new KeyNotFoundException("Relación Articulo-Tienda no encontrada.");

        entity.Stock = dto.Stock;
        if (dto.FechaAlta.HasValue) entity.FechaAlta = dto.FechaAlta.Value;
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int articuloId, int tiendaId)
    {
        var entity = await db.ArticuloTiendas.FindAsync(articuloId, tiendaId)
            ?? throw new KeyNotFoundException("Relación Articulo-Tienda no encontrada.");
        db.ArticuloTiendas.Remove(entity);
        await db.SaveChangesAsync();
    }
}
