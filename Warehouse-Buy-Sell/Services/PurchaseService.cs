using Microsoft.EntityFrameworkCore;
using Warehouse_Buy_Sell.Data;
using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Interfaces;
using Warehouse_Buy_Sell.Models;

namespace Warehouse_Buy_Sell.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly AppDbContext _context;

        public PurchaseService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponce<PurchaseResponceDto>> CreateAsync(PurchaseCreateDto dto)
        {
            var warehouse = await _context.Warehouses.FindAsync(dto.WarehouseId);
            if (warehouse == null)
                return ApiResponce<PurchaseResponceDto>.Fail("Warehouse not found");

            var supplier = await _context.Suppliers.FindAsync(dto.SupplierId);
            if (supplier == null)
                return ApiResponce<PurchaseResponceDto>.Fail("Supplier not found");

            var productIds = dto.Items.Select(i => i.ProductId).Distinct().ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            if (products.Count != productIds.Count)
                return ApiResponce<PurchaseResponceDto>.Fail("One or more products not found");

            if (dto.Items.GroupBy(i => i.ProductId).Any(g => g.Count() > 1))
                return ApiResponce<PurchaseResponceDto>.Fail("Duplicate products in purchase items");

            var purchase = new Purchase
            {
                PurchaseDate = dto.PurchaseDate,
                WarehouseId = dto.WarehouseId,
                SupplierId = dto.SupplierId,
                purchaseItems = dto.Items.Select(i => new PurchaseItems
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Quantity * i.UnitPrice
                }).ToList()
            };
            purchase.TotalAmount = purchase.purchaseItems.Sum(i => i.Subtotal);

            _context.Purchases.Add(purchase);


            //update inventory
            foreach (var item in purchase.purchaseItems)
            {
                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(inv =>
                        inv.WarehouseId == dto.WarehouseId &&
                        inv.ProductId == item.ProductId);

                if (inventory == null)
                {
                    _context.Inventories.Add(new Inventory
                    {
                        WarehouseId = dto.WarehouseId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }
                else
                {
                    inventory.Quantity += item.Quantity;
                }
            }
            await _context.SaveChangesAsync();

            //reload with navigation properties

            var created = await _context.Purchases
               .Include(p => p.warehouse)
               .Include(p => p.supplier)
               .Include(p => p.purchaseItems)
                   .ThenInclude(pi => pi.product)
               .FirstOrDefaultAsync(p => p.Id == purchase.Id);

            return ApiResponce<PurchaseResponceDto>.Ok(MapToResponse(created), "Purchase created successfully");
        }

        public  async Task<ApiResponce<bool>> DeleteAsync(int id)
        {
            var purchase = await _context.Purchases
                .Include(p => p.purchaseItems)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (purchase == null)
                return ApiResponce<bool>.Fail($"Purchase with ID {id} not found");

            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();
            return ApiResponce<bool>.Ok(true, "Purchase deleted successfully");
        }

        public async Task<ApiResponce<List<PurchaseResponceDto>>> GetAllAsync()
        {
           var purchases= await _context.Purchases
                .Include(p=>p.warehouse)
                .Include(p=>p.supplier)
                .Include(p=>p.purchaseItems)
                .ThenInclude(pI=>pI.product)
                .Select(p=> MapToResponse(p))
                .ToListAsync();

            return ApiResponce<List<PurchaseResponceDto>>.Ok(purchases);
        }

        public  async Task<ApiResponce<PurchaseResponceDto>> GetByIdAsync(int id)
        {
            var purchase = await _context.Purchases
                .Include(p => p.warehouse)
                .Include(p => p.supplier)
                .Include(p => p.purchaseItems)
                    .ThenInclude(pi => pi.product)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (purchase == null)
                return ApiResponce<PurchaseResponceDto>.Fail($"Purchase with ID {id} not found");

            return ApiResponce<PurchaseResponceDto>.Ok(MapToResponse(purchase));
        }

        private static PurchaseResponceDto MapToResponse(Purchase p) => new PurchaseResponceDto
        {
            Id = p.Id,
            PurchaseDate = p.PurchaseDate,
            WarehouseId = p.WarehouseId,
            WarehouseName = p.warehouse?.WarehouseName,
            SupplierId = p.SupplierId,
            SupplierName = p.supplier?.SupplierName,
            TotalAmount = p.TotalAmount ?? 0,
            Items = p.purchaseItems?.Select(i => new PurchaseItemResponceDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.product?.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Subtotal = i.Subtotal
            }).ToList()
        };
    }
}
