using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Interfaces;

namespace Warehouse_Buy_Sell.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

      [HttpGet("GetAllWarehouses")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _warehouseService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("GetWarehouseBy{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponce<object>.Fail("Invalid ID"));

            var result = await _warehouseService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpPost("CreateWarehouse")]
        public async Task<IActionResult> Create([FromBody] WarehouseCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _warehouseService.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }
        [HttpPut("UpdateWarehouseBy{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] WarehouseUpdateDto dto)
        {
            if (id <= 0)
                return BadRequest(ApiResponce<object>.Fail("Invalid ID"));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _warehouseService.UpdateAsync(dto,id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpDelete("DeleteWarehouseBy{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponce<object>.Fail("Invalid ID"));

            var result = await _warehouseService.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
