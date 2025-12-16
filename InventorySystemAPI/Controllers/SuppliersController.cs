using Application.DTOs;
using Application.Interfaces;
using InventorySystemAPI.CustomActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystemAPI.Controllers
{
    [Authorize]
    [AllowAnonymous] // temporary for tests
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        // GET: api/Suppliers
        [HttpGet]
        public async Task<IActionResult> GetSuppliers(
            [FromQuery] int pageSize = 1000,
            [FromQuery] int pageNumber = 1,
            [FromQuery] string? filterOn = null,
            [FromQuery] string? filterQuery = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isDescending = false)
        {
            try
            {
                var result = await _supplierService.SearchAsync(
                    filterOn,
                    filterQuery,
                    sortBy,
                    isDescending,
                    pageNumber,
                    pageSize);

                // ? FIX: Result, not Items
                if (!result.Result.Any())
                {
                    return NotFound("No data found.");
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Suppliers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplier(Guid id)
        {
            try
            {
                var supplier = await _supplierService.GetByIdAsync(id);

                if (supplier == null)
                {
                    return NotFound("Supplier not found.");
                }

                return Ok(supplier);
            }
            catch (Exception)
            {
                return NotFound("Supplier not found.");
            }
        }

        // POST: api/Suppliers
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierCreateDto supplierDto)
        {
            try
            {
                var created = await _supplierService.CreateAsync(supplierDto);
                return CreatedAtAction(nameof(GetSupplier), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Suppliers/{id}
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateSupplier(Guid id, [FromBody] SupplierUpdateDto supplierDto)
        {
            try
            {
                await _supplierService.UpdateAsync(id, supplierDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Supplier not found.");
            }
        }

        // DELETE: api/Suppliers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(Guid id)
        {
            try
            {
                await _supplierService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Supplier not found.");
            }
        }
    }
}
