using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Interfaces;

namespace Warehouse_Buy_Sell.Services
{
    public class ReportService : IReportService
    {
        public Task<ApiResponce<ProfitReportDto>> GetProfitReportAsync(DateTime? from, DateTime? to)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponce<List<StockReportDto>>> GetStockReportAsync(int? warehouseId)
        {
            throw new NotImplementedException();
        }
    }
}
