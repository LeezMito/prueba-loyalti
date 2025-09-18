using Inventory.Business.Services;
using Inventory.Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticuloTiendaController(IArticuloTiendaService svc) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ArticuloTiendaItemDto>>> Get([FromQuery] int? tiendaId, [FromQuery] int? articuloId)
        => Ok(await svc.GetAsync(tiendaId, articuloId));

    [HttpGet("{articuloId:int}/{tiendaId:int}")]
    [Authorize]
    public async Task<ActionResult<ArticuloTiendaItemDto>> GetOne(int articuloId, int tiendaId)
        => (await svc.GetOneAsync(articuloId, tiendaId)) is { } dto ? Ok(dto) : NotFound();

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Create([FromBody] ArticuloTiendaCreateDto dto)
    {
        await svc.CreateAsync(dto);
        return CreatedAtAction(nameof(GetOne), new { articuloId = dto.ArticuloId, tiendaId = dto.TiendaId }, null);
    }

    [HttpPut("{articuloId:int}/{tiendaId:int}")]
    [Authorize]
    public async Task<ActionResult> Update(int articuloId, int tiendaId, [FromBody] ArticuloTiendaUpdateDto dto)
    {
        await svc.UpdateAsync(articuloId, tiendaId, dto);
        return NoContent();
    }

    [HttpDelete("{articuloId:int}/{tiendaId:int}")]
    [Authorize]
    public async Task<ActionResult> Delete(int articuloId, int tiendaId)
    {
        await svc.DeleteAsync(articuloId, tiendaId);
        return NoContent();
    }
}
