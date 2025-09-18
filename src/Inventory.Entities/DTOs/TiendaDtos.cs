namespace Inventory.Entities.DTOs;

public record TiendaCreateDto(string Sucursal, string? Direccion);
public record TiendaUpdateDto(string Sucursal, string? Direccion);
public record TiendaListItemDto(int Id, string Sucursal, string? Direccion);
public record TiendaDetailDto(int Id, string Sucursal, string? Direccion);

public record TiendaDto(
    int TiendaId,
    string Sucursal,
    string Direccion
);