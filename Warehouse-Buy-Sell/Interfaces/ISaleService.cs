using Warehouse_Buy_Sell.DTO;

namespace Warehouse_Buy_Sell.Interfaces
{
    public interface ISaleService
    {
        Task<ApiResponce<List<SaleResponceDto>>> GetAllAsync();
        Task<ApiResponce<SaleResponceDto>> GetByIdAsync(int id);
        Task<ApiResponce<SaleResponceDto>> CreateAsync(SaleCreateDto dto);
        Task<ApiResponce<bool>> DeleteAsync(int id);
    }
}
