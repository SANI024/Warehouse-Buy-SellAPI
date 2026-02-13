using Warehouse_Buy_Sell.DTO;

namespace Warehouse_Buy_Sell.Interfaces
{
    public interface ISupplierService
    {
        Task<ApiResponce<List<SupplierResponceDto>>> GetAllAsync();
        Task<ApiResponce<SupplierResponceDto>> GetByIdAsync(int id);
        Task<ApiResponce<SupplierResponceDto>> CreateAsync(SupplierCreateDto dto);
        Task<ApiResponce<SupplierResponceDto>> UpdateAsync(SupplierUpdateDto dto);
        Task<ApiResponce<bool>> DeleteAsync(int id);
    }
}
