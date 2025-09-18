// Inventory.WebApi/Controllers/CheckoutController.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Inventory.Business.Services;
using Inventory.Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckoutController(ICheckoutService svc) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CheckoutResultDto), 200)]
    [Authorize] 
    public async Task<IActionResult> Confirm(CancellationToken ct)
    {
        var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(sub))
            return Unauthorized();

        var clienteId = int.Parse(sub);

        var result = await svc.ConfirmAsync(clienteId, ct);
        if (!result.Success)
            return Conflict(result);

        return Ok(result);
    }
}
