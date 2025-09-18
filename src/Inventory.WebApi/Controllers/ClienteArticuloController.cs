using Inventory.Business.Services;
using Inventory.Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteArticuloController(IClienteArticuloService svc) : ControllerBase
{
    [HttpGet("by-cliente/{clienteId:int}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ClienteArticuloListItemDto>>> GetByCliente(int clienteId)
        => Ok(await svc.GetByClienteAsync(clienteId));

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Create([FromBody] ClienteArticuloCreateDto dto)
    {
        await svc.CreateAsync(dto);
        return Created();
    }

    [HttpDelete]
    [Authorize]
    public async Task<ActionResult> Delete([FromQuery] int clienteId, [FromQuery] int articuloId, [FromQuery] DateTime fecha)
    {
        await svc.DeleteAsync(clienteId, articuloId, fecha);
        return NoContent();
    }
}
