namespace Inventory.Entities.DTOs;

public record RegisterRequest(string Email, string Password, string Nombre, string Apellidos, string? Direccion);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string AccessToken, int ClienteId, string Email, string NombreCompleto);
