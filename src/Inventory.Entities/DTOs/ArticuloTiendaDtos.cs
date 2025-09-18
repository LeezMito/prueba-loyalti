namespace Inventory.Entities.DTOs;

public record ArticuloTiendaCreateDto(int ArticuloId, int TiendaId, int Stock, DateTime? FechaAlta = null);
public record ArticuloTiendaUpdateDto(int Stock, DateTime? FechaAlta = null);

public record ArticuloTiendaListItemDto(
    int ArticuloId,
    string ArticuloCodigo,
    string ArticuloDescripcion,
    int TiendaId,
    string TiendaSucursal,
    int Stock,
    decimal Precio,
    DateTime FechaAlta
);
