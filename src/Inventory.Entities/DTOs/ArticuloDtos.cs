namespace Inventory.Entities.DTOs;

public record ArticuloCreateDto(string Codigo, string Descripcion, decimal Precio, string? ImagenUrl);
public record ArticuloUpdateDto(string Codigo, string Descripcion, decimal Precio, string? ImagenUrl);
public record ArticuloListItemDto(int Id, string Codigo, string Descripcion, decimal Precio, string? ImagenUrl);
public record ArticuloDetailDto(int Id, string Codigo, string Descripcion, decimal Precio, string? ImagenUrl);
