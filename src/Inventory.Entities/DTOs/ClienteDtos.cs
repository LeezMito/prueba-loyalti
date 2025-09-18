namespace Inventory.Entities.DTOs;

public record ClienteCreateDto(string Nombre, string Apellidos, string? Direccion);
public record ClienteUpdateDto(string Nombre, string Apellidos, string? Direccion);
public record ClienteListItemDto(int Id, string Nombre, string Apellidos, string? Direccion);
public record ClienteDetailDto(int Id, string Nombre, string Apellidos, string? Direccion);
