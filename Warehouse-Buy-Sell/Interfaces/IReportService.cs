using Warehouse_Buy_Sell.DTO;

namespace Warehouse_Buy_Sell.Interfaces
{
    public interface IReportService
    {
        Task<ApiResponce<ProfitReportDto>> GetProfitReportAsync(DateTime? from, DateTime? to);
        Task<ApiResponce<List<StockReportDto>>> GetStockReportAsync(int? warehouseId);
    }
}
