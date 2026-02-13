using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Interfaces;

namespace Warehouse_Buy_Sell.Services
{
    public class InventoryService : IInverntoryService
    {
        public Task<ApiResponce<List<InventoryResponceDto>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponce<InventoryResponceDto>> GetByWarehouseAndProductAsync(int warehouseId, int productId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponce<List<InventoryResponceDto>>> GetByWarehouseAsync(int warehouseId)
        {
            throw new NotImplementedException();
        }
    }
}
