namespace Inventory.Entities.DTOs;

public record CheckoutRequestDto(int ClienteId);
public record CheckoutErrorItem(int ArticuloId, string Codigo, string Descripcion, int Requested, int Available);
public record CheckoutResultDto(bool Success, List<CheckoutErrorItem>? Errors = null);
