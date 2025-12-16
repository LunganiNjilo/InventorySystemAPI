using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace InventorySystemAPI.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController : ControllerBase
  {
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
      _service = service;
    }

    // GET: api/Products?pageSize=10&pageNumber=1&filterOn=ProductName&filterQuery=Product&sortBy=ProductName&isDescending=true
    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int pageSize = 1000,
        [FromQuery] int pageNumber = 1,
        [FromQuery] string? filterOn = null,
        [FromQuery] string? filterQuery = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool isDescending = false)
    {
      try
      {
        var paged = await _service.SearchAsync(filterOn, filterQuery, sortBy, isDescending, pageNumber, pageSize);

        if (paged.Result == null || paged.Result.Count == 0)
          return NotFound(new { success = false, message = "No data found." });

        return Ok(new
        {
          success = true,
          result = paged.Result,
          totalRecordCount = paged.TotalRecordCount,
          totalPages = paged.TotalPages,
          pageNumberMessage = paged.PageNumberMessage,
          isPrevious = paged.IsPrevious,
          isNext = paged.IsNext
        });
      }
      catch (ArgumentException ex)
      {
        return BadRequest(new { success = false, message = ex.Message });
      }
      catch (Exception ex)
      {
        // log exception
        return StatusCode(500, new { success = false, message = ex.Message });
      }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
      try
      {
        var dto = await _service.GetByIdAsync(id);
        return Ok(new { success = true, result = dto });
      }
      catch (Exception ex)
      {
        return NotFound(new { success = false, message = ex.Message });
      }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
      // validation should happen via FluentValidation middleware
      var created = await _service.CreateAsync(request);
      return CreatedAtAction(nameof(GetProduct), new { id = created.Id }, new { success = true, result = created });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request)
    {
      await _service.UpdateAsync(id, request);
      return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
      try
      {
        await _service.DeleteAsync(id);
        return NoContent();
      }
      catch (DbUpdateException dbEx)
      {
        if (dbEx.InnerException is SqlException sqlEx && sqlEx.Number == 547)
        {
          // 547 = FK constraint violation in SQL Server
          return BadRequest(new
          {
            error = "This product cannot be deleted because it is referenced by inventory or other data."
          });
        }

        return StatusCode(500, new { error = "A database error occurred." });
      }
      catch (Exception)
      {
        return StatusCode(500, new { error = "An unexpected error occurred." });
      }
    }

  }
}
