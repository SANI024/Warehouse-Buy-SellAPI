using Warehouse_Buy_Sell.DTO;

namespace Warehouse_Buy_Sell.Interfaces
{
    public interface IWarehouseService
    {
        Task<ApiResponce<List<WarehouseResponceDto>>> GetAllAsync();
        Task<ApiResponce<WarehouseResponceDto>>GetByIdAsync(int id);
        Task<ApiResponce<WarehouseResponceDto>> CreateAsync(WarehouseCreateDto dto);
        Task<ApiResponce<WarehouseResponceDto>>UpdateAsync(WarehouseUpdateDto dto);
        Task<ApiResponce<bool>> DeleteAsync(int id);
    }
}
