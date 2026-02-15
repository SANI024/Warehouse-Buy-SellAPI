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
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("profit")]
        public async Task<IActionResult> GetProfitReport(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            if (from.HasValue && to.HasValue && from > to)
                return BadRequest(ApiResponce<object>.Fail("'From' date cannot be after 'To' date"));

            var result = await _reportService.GetProfitReportAsync(from, to);
            return Ok(result);
        }

        [HttpGet("stock")]
        public async Task<IActionResult> GetStockReport([FromQuery] int? warehouseId)
        {
            if (warehouseId.HasValue && warehouseId <= 0)
                return BadRequest(ApiResponce<object>.Fail("Invalid warehouse ID"));

            var result = await _reportService.GetStockReportAsync(warehouseId);
            return Ok(result);
        }
    }
}
