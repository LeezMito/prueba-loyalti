using Inventory.Data;
using Inventory.Entities.DTOs;
using Inventory.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Business.Services;

public class ArticuloService(AppDbContext db) : IArticuloService
{
    public async Task<IEnumerable<ArticuloListItemDto>> GetAllAsync()
    {
        return await db.Articulos.AsNoTracking()
            .OrderBy(x => x.Codigo)
            .Select(x => new ArticuloListItemDto(
                x.Id,
                x.Codigo,
                x.Descripcion,
                x.Precio,
                x.ImagenUrl,
                x.Stock, 
                x.ArticuloTiendas
                    .GroupBy(at => new { at.TiendaId, at.Tienda.Sucursal, at.Tienda.Direccion })
                    .Select(g => new TiendaDto(
                        g.Key.TiendaId,
                        g.Key.Sucursal,
                        g.Key.Direccion ?? string.Empty
                    ))
                    .ToList()
            ))
            .ToListAsync();
    }

    public async Task<ArticuloDetailDto?> GetByIdAsync(int id)
    {
        return await db.Articulos.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new ArticuloDetailDto(
                x.Id, x.Codigo,
                x.Descripcion,
                x.Precio,
                x.ImagenUrl,
                x.Stock
            ))
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateAsync(ArticuloCreateDto dto)
    {
        var entity = new Articulo
        {
            Codigo = dto.Codigo,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio,
            ImagenUrl = dto.ImagenUrl,
            Stock = dto.Stock
        };
        db.Articulos.Add(entity);
        await db.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(int id, ArticuloUpdateDto dto)
    {
        var entity = await db.Articulos.FindAsync(id) ?? throw new KeyNotFoundException();
        entity.Codigo = dto.Codigo;
        entity.Descripcion = dto.Descripcion;
        entity.Precio = dto.Precio;
        entity.ImagenUrl = dto.ImagenUrl;
        entity.Stock = entity.Stock;
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await db.Articulos.FindAsync(id) ?? throw new KeyNotFoundException();
        db.Articulos.Remove(entity);
        await db.SaveChangesAsync();
    }
}
