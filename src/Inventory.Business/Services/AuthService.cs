using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Inventory.Data;
using Inventory.Entities.DTOs;
using Inventory.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Inventory.Business.Services;

public class AuthService(AppDbContext db, IConfiguration config) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest req)
    {
        var email = req.Email.Trim().ToLowerInvariant();
        if (await db.Clientes.AnyAsync(c => c.Email == email))
            throw new InvalidOperationException("El email ya está registrado.");

        CreatePasswordHash(req.Password, out var hash, out var salt);

        var cliente = new Cliente
        {
            Email = email,
            PasswordHash = hash,
            PasswordSalt = salt,
            Nombre = req.Nombre,
            Apellidos = req.Apellidos,
            Direccion = req.Direccion
        };

        db.Clientes.Add(cliente);
        await db.SaveChangesAsync();

        var token = CreateJwt(cliente);
        return new AuthResponse(token, cliente.Id, cliente.Email!, $"{cliente.Nombre} {cliente.Apellidos}");
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest req)
    {
        var email = req.Email.Trim().ToLowerInvariant();
        var cliente = await db.Clientes.FirstOrDefaultAsync(c => c.Email == email)
            ?? throw new InvalidOperationException("Credenciales inválidas.");

        if (cliente.PasswordHash == null || cliente.PasswordSalt == null)
            throw new InvalidOperationException("Usuario sin contraseña establecida.");

        if (!VerifyPasswordHash(req.Password, cliente.PasswordHash, cliente.PasswordSalt))
            throw new InvalidOperationException("Credenciales inválidas.");

        var token = CreateJwt(cliente);
        return new AuthResponse(token, cliente.Id, cliente.Email!, $"{cliente.Nombre} {cliente.Apellidos}");
    }

    private string CreateJwt(Cliente c)
    {
        var jwtSection = config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, c.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, c.Email ?? ""),
            new("name", $"{c.Nombre} {c.Apellidos}")
        };

        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
        var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computed.SequenceEqual(storedHash);
    }
}
