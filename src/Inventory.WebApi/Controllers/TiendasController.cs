using Inventory.Business.Services;
using Inventory.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TiendasController(ITiendaService svc) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TiendaListItemDto>), 200)]
    public async Task<IActionResult> GetAll()
        => Ok(await svc.GetAllAsync());

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TiendaDetailDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        var dto = await svc.GetByIdAsync(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] TiendaCreateDto dto)
    {
        var id = await svc.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] TiendaUpdateDto dto)
    {
        await svc.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        await svc.DeleteAsync(id);
        return NoContent();
    }
}
