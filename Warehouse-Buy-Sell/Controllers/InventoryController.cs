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
    public class InventoryController : ControllerBase
    {
        private readonly IInverntoryService _inventoryService;

        public InventoryController(IInverntoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _inventoryService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("warehouse/{warehouseId}")]
        public async Task<IActionResult> GetByWarehouse(int warehouseId)
        {
            if (warehouseId <= 0)
                return BadRequest(ApiResponce<object>.Fail("Invalid warehouse ID"));

            var result = await _inventoryService.GetByWarehouseAsync(warehouseId);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("warehouse/{warehouseId}/product/{productId}")]
        public async Task<IActionResult> GetByWarehouseAndProduct(int warehouseId, int productId)
        {
            if (warehouseId <= 0 || productId <= 0)
                return BadRequest(ApiResponce<object>.Fail("Invalid ID"));

            var result = await _inventoryService.GetByWarehouseAndProductAsync(warehouseId, productId);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}
