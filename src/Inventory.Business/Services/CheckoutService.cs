// Inventory.Business/Services/CheckoutService.cs
using Inventory.Data;
using Inventory.Entities.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Business.Services;

public class CheckoutService : ICheckoutService
{
    private readonly AppDbContext _db;

    public CheckoutService(AppDbContext db) => _db = db;

    public async Task<CheckoutResultDto> ConfirmAsync(int clienteId, CancellationToken ct = default)
    {
        Console.WriteLine($"Starting checkout for clienteId {clienteId}");
        var lines = await _db.ClienteArticulos
            .AsNoTracking()
            .Where(x => x.ClienteId == clienteId)
            .GroupBy(x => x.ArticuloId)
            .Select(g => new { ArticuloId = g.Key, Qty = g.Count() })
            .ToListAsync(ct);

        if (lines.Count == 0)
            return new CheckoutResultDto(true); 

        var articuloIds = lines.Select(l => l.ArticuloId).ToArray();
        var articulos = await _db.Articulos
            .Where(a => articuloIds.Contains(a.Id))
            .ToListAsync(ct);

        var errors = new List<CheckoutErrorItem>();
        foreach (var l in lines)
        {
            var a = articulos.First(x => x.Id == l.ArticuloId);
            if (a.Stock < l.Qty)
            {
                errors.Add(new CheckoutErrorItem(a.Id, a.Codigo, a.Descripcion, l.Qty, a.Stock));
            }
        }
        if (errors.Count > 0)
            return new CheckoutResultDto(false, errors);

        await using var tx = await _db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, ct);
        try
        {
            var lockedArts = await _db.Articulos
                .Where(a => articuloIds.Contains(a.Id))
                .ToListAsync(ct);

            foreach (var l in lines)
            {
                var a = lockedArts.First(x => x.Id == l.ArticuloId);
                if (a.Stock < l.Qty)
                {
                    errors.Add(new CheckoutErrorItem(a.Id, a.Codigo, a.Descripcion, l.Qty, a.Stock));
                }
                else
                {
                    a.Stock -= l.Qty;
                }
            }

            if (errors.Count > 0)
            {
                await tx.RollbackAsync(ct);
                return new CheckoutResultDto(false, errors);
            }

            var cartLines = await _db.ClienteArticulos
                .Where(x => x.ClienteId == clienteId)
                .ToListAsync(ct);

            _db.ClienteArticulos.RemoveRange(cartLines);

            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            return new CheckoutResultDto(true);
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }
    }
}
