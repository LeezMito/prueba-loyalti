using Inventory.Data;
using Inventory.Entities.DTOs;
using Inventory.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Business.Services;

public class TiendaService(AppDbContext db) : ITiendaService
{
    public async Task<IEnumerable<TiendaListItemDto>> GetAllAsync()
    {
        return await db.Tiendas.AsNoTracking()
            .OrderBy(x => x.Sucursal)
            .Select(x => new TiendaListItemDto(x.Id, x.Sucursal, x.Direccion))
            .ToListAsync();
    }

    public async Task<TiendaDetailDto?> GetByIdAsync(int id)
    {
        return await db.Tiendas.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new TiendaDetailDto(x.Id, x.Sucursal, x.Direccion))
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateAsync(TiendaCreateDto dto)
    {
        var entity = new Tienda { Sucursal = dto.Sucursal, Direccion = dto.Direccion };
        db.Tiendas.Add(entity);
        await db.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(int id, TiendaUpdateDto dto)
    {
        var entity = await db.Tiendas.FindAsync(id) ?? throw new KeyNotFoundException();
        entity.Sucursal = dto.Sucursal;
        entity.Direccion = dto.Direccion;
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await db.Tiendas.FindAsync(id) ?? throw new KeyNotFoundException();
        db.Tiendas.Remove(entity);
        await db.SaveChangesAsync();
    }
}
