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
    public class InternalMovementController : ControllerBase
    {
        private readonly IInternalMovementService _movementService;

        public InternalMovementController(IInternalMovementService movementService)
        {
            _movementService = movementService;
        }

        [HttpGet("GetAllInternalMovements")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _movementService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("GetInternalMovementBy{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponce<object>.Fail("Invalid ID"));

            var result = await _movementService.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("CreateInternalMovement")]
        public async Task<IActionResult> Create([FromBody] InternalMovementCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _movementService.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        [HttpDelete("DeleteInternalMovementBy{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponce<object>.Fail("Invalid ID"));

            var result = await _movementService.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


    }
}
