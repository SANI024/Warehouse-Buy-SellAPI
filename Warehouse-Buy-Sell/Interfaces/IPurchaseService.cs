using Warehouse_Buy_Sell.DTO;

namespace Warehouse_Buy_Sell.Interfaces
{
    public interface IPurchaseService
    {
        Task<ApiResponce<List<ProductResponceDto>>> GetAllAsync();
        Task<ApiResponce<ProductResponceDto>> GetByIdAsync(int id);
        Task<ApiResponce<ProductResponceDto>> CreateAsync(ProductCreateDto dto);
        Task<ApiResponce<ProductResponceDto>> UpdateAsync(int id, ProductUpdateDto dto);
        Task<ApiResponce<bool>> DeleteAsync(int id);
    }
}
