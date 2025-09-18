using System.Security.Claims;

namespace Inventory.WebApi.Extensions;

public static class HttpContextExtensions
{
    public static int? GetClienteId(this ClaimsPrincipal user)
        => int.TryParse(user.FindFirstValue("sub") ?? user.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;
}
