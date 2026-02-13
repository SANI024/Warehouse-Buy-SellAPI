using Warehouse_Buy_Sell.DTO;

namespace Warehouse_Buy_Sell.Interfaces
{
    public interface IPurchaseService
    {
        Task<ApiResponce<List<PurchaseResponceDto>>> GetAllAsync();
        Task<ApiResponce<PurchaseResponceDto>> GetByIdAsync(int id);
        Task<ApiResponce<PurchaseResponceDto>> CreateAsync(PurchaseResponceDto dto);
        Task<ApiResponce<PurchaseResponceDto>> UpdateAsync(int id, PurchaseResponceDto dto);
        Task<ApiResponce<bool>> DeleteAsync(int id);
    }
}
