using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductCategoriesController : ControllerBase
{
  private readonly IProductCategoryService _service;

  public ProductCategoriesController(IProductCategoryService service)
  {
    _service = service;
  }

  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    var result = await _service.GetAllAsync();
    return Ok(new { result });
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> Get(Guid id)
  {
    var result = await _service.GetByIdAsync(id);
    return Ok(new { result });
  }

  [HttpPost]
  public async Task<IActionResult> Create(ProductCategoryCreateDto dto)
  {
    var created = await _service.CreateAsync(dto);
    return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Update(Guid id, ProductCategoryCreateDto dto)
  {
    var updated = await _service.UpdateAsync(id, dto);
    return Ok(new { result = updated });
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(Guid id)
  {
    await _service.DeleteAsync(id);
    return Ok(new { message = "Category deleted" });
  }
}
