using Microsoft.EntityFrameworkCore;
using Warehouse_Buy_Sell.Data;
using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Interfaces;
using Warehouse_Buy_Sell.Models;

namespace Warehouse_Buy_Sell.Services
{
    public class InternalMovementService : IInternalMovementService
    {
        private AppDbContext _context;
        public InternalMovementService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponce<InternalMovementResponceDto>> CreateAsync(InternalMovementCreateDto dto)
        {
            if (dto.FromWarehouseId == dto.ToWarehouseId)
                return ApiResponce<InternalMovementResponceDto>.Fail("Source and destination warehouses must be different");

            var fromWarehouse = await _context.Warehouses.FindAsync(dto.FromWarehouseId);
            if (fromWarehouse == null)
                return ApiResponce<InternalMovementResponceDto>.Fail("Source warehouse not found");

            var toWarehouse = await _context.Warehouses.FindAsync(dto.ToWarehouseId);
            if (toWarehouse == null)
                return ApiResponce<InternalMovementResponceDto>.Fail("Destination warehouse not found");

            var productIds = dto.Items.Select(i => i.ProductId).Distinct().ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            if (products.Count != productIds.Count)
                return ApiResponce<InternalMovementResponceDto>.Fail("One or more products not found");

            if (dto.Items.GroupBy(i => i.ProductId).Any(g => g.Count() > 1))
                return ApiResponce<InternalMovementResponceDto>.Fail("Duplicate products in movement items");

            foreach (var item in dto.Items)
            {
                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(inv =>
                        inv.WarehouseId == dto.FromWarehouseId &&
                        inv.ProductId == item.ProductId);

                if (inventory == null || inventory.Quantity < item.Quantity)
                {
                    var product = products.First(p => p.Id == item.ProductId);
                    return ApiResponce<InternalMovementResponceDto>.Fail(
                        $"Insufficient stock for '{product.ProductName}' in source warehouse. " +
                        $"Available: {inventory?.Quantity ?? 0}, Requested: {item.Quantity}");
                }
            }

            var movement = new InternalMovement
            {
                MovementDate = dto.MovementDate,
                FromWarehouseId = dto.FromWarehouseId,
                ToWarehouseId = dto.ToWarehouseId,
                internalMovementItems = dto.Items.Select(i => new InternalMovementItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };

            _context.InternalMovements.Add(movement);

            foreach (var item in movement.internalMovementItems)
            {
                var fromInventory = await _context.Inventories
                    .FirstOrDefaultAsync(inv =>
                        inv.WarehouseId == dto.FromWarehouseId &&
                        inv.ProductId == item.ProductId);

                fromInventory.Quantity -= item.Quantity;

                var toInventory = await _context.Inventories
                    .FirstOrDefaultAsync(inv =>
                        inv.WarehouseId == dto.ToWarehouseId &&
                        inv.ProductId == item.ProductId);

                if (toInventory == null)
                {
                    _context.Inventories.Add(new Inventory
                    {
                        WarehouseId = dto.ToWarehouseId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }
                else
                {
                    toInventory.Quantity += item.Quantity;
                }
            }

            await _context.SaveChangesAsync();

            var created = await _context.InternalMovements
                .Include(m => m.fromWarehouse)
                .Include(m => m.toWarehouse)
                .Include(m => m.internalMovementItems)
                    .ThenInclude(i => i.product)        
                .FirstOrDefaultAsync(m => m.Id == movement.Id);

            return ApiResponce<InternalMovementResponceDto>.Ok(MapToResponse(created), "Movement created successfully");
        }

        public async Task<ApiResponce<bool>> DeleteAsync(int id)
        {
            var movement = await _context.InternalMovements
               .Include(m => m.internalMovementItems)
               .FirstOrDefaultAsync(m => m.Id == id);

            if (movement == null)
                return ApiResponce<bool>.Fail($"Movement with ID {id} not found");

            _context.InternalMovements.Remove(movement);
            await _context.SaveChangesAsync();
            return ApiResponce<bool>.Ok(true, "Movement deleted successfully");
        }

        public async Task<ApiResponce<List<InternalMovementResponceDto>>> GetAllAsync()
        {
            var movements = await _context.InternalMovements
               .Include(m => m.fromWarehouse)
               .Include(m => m.toWarehouse)
               .Include(m => m.internalMovementItems)
                   .ThenInclude(i => i.product)
               .Select(m => MapToResponse(m))
               .ToListAsync();

            return ApiResponce<List<InternalMovementResponceDto>>.Ok(movements);
        }

        public  async Task<ApiResponce<InternalMovementResponceDto>> GetByIdAsync(int id)
        {
            var movement = await _context.InternalMovements
                .Include(m => m.fromWarehouse)
                .Include(m => m.toWarehouse)
                .Include(m => m.internalMovementItems)
                    .ThenInclude(i => i.product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movement == null)
                return ApiResponce<InternalMovementResponceDto>.Fail($"Movement with ID {id} not found");

            return ApiResponce<InternalMovementResponceDto>.Ok(MapToResponse(movement));
        }

        private InternalMovementResponceDto MapToResponse(InternalMovement m) => new InternalMovementResponceDto
        {
            Id = m.Id,
            MovementDate = m.MovementDate,
            FromWarehouseId = m.FromWarehouseId,
            FromWarehouseName = m.fromWarehouse?.WarehouseName,
            ToWarehouseId = m.ToWarehouseId,
            ToWarehouseName = m.toWarehouse?.WarehouseName,
            Items = m.internalMovementItems?.Select(i => new InternalMovementItemResponceDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.product?.ProductName,
                Quantity = i.Quantity
            }).ToList()
        };
    }
}
