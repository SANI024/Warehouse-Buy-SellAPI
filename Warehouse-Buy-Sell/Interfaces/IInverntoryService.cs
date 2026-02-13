using Warehouse_Buy_Sell.DTO;

namespace Warehouse_Buy_Sell.Interfaces
{
    public interface IInverntoryService
    {
        Task<ApiResponce<List<InventoryResponceDto>>> GetAllAsync();
        Task<ApiResponce<List<InventoryResponceDto>>> GetByWarehouseAsync(int warehouseId);
        Task<ApiResponce<InventoryResponceDto>> GetByWarehouseAndProductAsync(int warehouseId, int productId);
    }
}
