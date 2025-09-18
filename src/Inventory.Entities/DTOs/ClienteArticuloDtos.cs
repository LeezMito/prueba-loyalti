namespace Inventory.Entities.DTOs;

public record ClienteArticuloCreateDto(int ClienteId, int ArticuloId, DateTime? Fecha = null);

public record ClienteArticuloListItemDto(
    int ClienteId,
    string ClienteNombreCompleto,
    int ArticuloId,
    string ArticuloCodigo,
    string ArticuloDescripcion,
    DateTime Fecha
);
