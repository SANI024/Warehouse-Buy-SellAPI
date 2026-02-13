using Microsoft.EntityFrameworkCore;
using Warehouse_Buy_Sell.Data;
using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Interfaces;
using Warehouse_Buy_Sell.Models;

namespace Warehouse_Buy_Sell.Services
{
    public class InventoryService : IInverntoryService
    {
        private AppDbContext _context;
        public InventoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponce<List<InventoryResponceDto>>> GetAllAsync()
        {
            var inventory = await _context.Inventories
              .Include(i => i.warehouse)
              .Include(i => i.product)
              .Select(i => MapToResponse(i))
              .ToListAsync();

            return ApiResponce<List<InventoryResponceDto>>.Ok(inventory);
        }

        public async Task<ApiResponce<InventoryResponceDto>> GetByWarehouseAndProductAsync(int warehouseId, int productId)
        {
            var inventory = await _context.Inventories
                .Include(i => i.warehouse)
                .Include(i => i.product)
                .FirstOrDefaultAsync(i => i.WarehouseId == warehouseId && i.ProductId == productId);

            if (inventory == null)
                return ApiResponce<InventoryResponceDto>.Fail("No stock found for this product in this warehouse");

            return ApiResponce<InventoryResponceDto>.Ok(MapToResponse(inventory));
        }

        public async Task<ApiResponce<List<InventoryResponceDto>>> GetByWarehouseAsync(int warehouseId)
        {
            var warehouse = await _context.Warehouses.FindAsync(warehouseId);
            if (warehouse == null)
                return ApiResponce<List<InventoryResponceDto>>.Fail("Warehouse not found");

            var inventory = await _context.Inventories
                .Include(i => i.warehouse)
                .Include(i => i.product)
                .Where(i => i.WarehouseId == warehouseId)
                .Select(i => MapToResponse(i))
                .ToListAsync();

            return ApiResponce<List<InventoryResponceDto>>.Ok(inventory);
        }
        private static InventoryResponceDto MapToResponse(Inventory i) => new InventoryResponceDto
        {
            Id = i.Id,
            WarehouseId = i.WarehouseId,
            WarehouseName = i.warehouse?.WarehouseName,
            ProductId = i.ProductId,
            ProductName = i.product?.ProductName,
            Quantity = i.Quantity,
        };
    }
}
