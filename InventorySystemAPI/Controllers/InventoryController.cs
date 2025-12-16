using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystemAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class InventoryController : ControllerBase
  {
    private readonly IInventoryService _service;

    public InventoryController(IInventoryService service)
    {
      _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] string? search,
        [FromQuery] string? sortBy,
        [FromQuery] bool desc = false)
    {
      var result = await _service.SearchAsync(search, sortBy, desc);
      return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
      var item = await _service.GetByIdAsync(id);
      return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InventoryCreateDto dto)
    {
      var created = await _service.CreateAsync(dto);
      return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] InventoryUpdateDto dto)
    {
      await _service.UpdateAsync(id, dto);
      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
      await _service.DeleteAsync(id);
      return NoContent();
    }
  }
}
