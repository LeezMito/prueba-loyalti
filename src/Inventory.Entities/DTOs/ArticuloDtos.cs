namespace Inventory.Entities.DTOs;

public record ArticuloCreateDto(
    string Codigo,
    string Descripcion,
    decimal Precio,
    string? ImagenUrl,
    int Stock
);
public record ArticuloUpdateDto(
    string Codigo,
    string Descripcion,
    decimal Precio,
    string? ImagenUrl
);
public record ArticuloListItemDto(
    int Id,
    string Codigo,
    string Descripcion,
    decimal Precio,
    string? ImagenUrl,
    int Stock,
    List<TiendaDto> Tiendas
);
public record ArticuloDetailDto(
    int Id, 
    string Codigo, 
    string Descripcion, 
    decimal Precio, 
    string? ImagenUrl,
    int Stock
);
