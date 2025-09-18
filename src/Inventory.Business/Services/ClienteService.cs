using Inventory.Data;
using Inventory.Entities.DTOs;
using Inventory.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Business.Services;

public class ClienteService(AppDbContext db) : IClienteService
{
    public async Task<IEnumerable<ClienteListItemDto>> GetAllAsync()
    {
        return await db.Clientes.AsNoTracking()
            .OrderBy(x => x.Apellidos).ThenBy(x => x.Nombre)
            .Select(x => new ClienteListItemDto(x.Id, x.Nombre, x.Apellidos, x.Direccion))
            .ToListAsync();
    }

    public async Task<ClienteDetailDto?> GetByIdAsync(int id)
    {
        return await db.Clientes.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new ClienteDetailDto(x.Id, x.Nombre, x.Apellidos, x.Direccion))
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateAsync(ClienteCreateDto dto)
    {
        var entity = new Cliente { Nombre = dto.Nombre, Apellidos = dto.Apellidos, Direccion = dto.Direccion };
        db.Clientes.Add(entity);
        await db.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(int id, ClienteUpdateDto dto)
    {
        var entity = await db.Clientes.FindAsync(id) ?? throw new KeyNotFoundException();
        entity.Nombre = dto.Nombre;
        entity.Apellidos = dto.Apellidos;
        entity.Direccion = dto.Direccion;
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await db.Clientes.FindAsync(id) ?? throw new KeyNotFoundException();
        db.Clientes.Remove(entity);
        await db.SaveChangesAsync();
    }
}
