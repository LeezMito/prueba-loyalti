namespace Inventory.Entities.DTOs;

public record ArticuloTiendaCreateDto(int ArticuloId, int TiendaId, int Stock, DateTime? FechaAlta = null);
public record ArticuloTiendaUpdateDto(int Stock, DateTime? FechaAlta = null);

public record ArticuloTiendaItemDto(
    int ArticuloId,
    string ArticuloCodigo,
    string ArticuloDescripcion,
    int TiendaId,
    string TiendaSucursal,
    decimal Precio,
    DateTime FechaAlta
);
