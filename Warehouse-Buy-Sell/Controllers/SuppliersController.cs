using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Interfaces;

namespace Warehouse_Buy_Sell.Controllers
{
  
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _supplierService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("GetBy{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponce<object>.Fail("Invalid ID"));

            var result = await _supplierService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("whoami")]
        public IActionResult WhoAmI()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new { userId, role });
        }
        [HttpPost("CreateSupplier")]
        public async Task<IActionResult> Create([FromBody] SupplierCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _supplierService.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        [HttpPut("UpdateSupplierBy{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SupplierUpdateDto dto)
        {
            if (id <= 0)
                return BadRequest(ApiResponce<object>.Fail("Invalid ID"));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _supplierService.UpdateAsync(dto,id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("DeleteSupplierBy{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponce<object>.Fail("Invalid ID"));

            var result = await _supplierService.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}

