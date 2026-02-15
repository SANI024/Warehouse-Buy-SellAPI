using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Interfaces;
using Warehouse_Buy_Sell.Services;

namespace Warehouse_Buy_Sell.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet("GetAllSales")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _saleService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("GetSaleBy{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponce<object>.Fail("Invalid ID"));

            var result = await _saleService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("CreateSale")]
        public async Task<IActionResult> Create([FromBody] SaleCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _saleService.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }
       

        [HttpDelete("DeleteSaleBy{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponce<object>.Fail("Invalid ID"));

            var result = await _saleService.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
