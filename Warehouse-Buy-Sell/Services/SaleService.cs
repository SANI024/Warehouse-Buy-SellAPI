using Microsoft.EntityFrameworkCore;
using Warehouse_Buy_Sell.Data;
using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Interfaces;
using Warehouse_Buy_Sell.Models;

namespace Warehouse_Buy_Sell.Services
{
    public class SaleService : ISaleService
    {
        private AppDbContext _context;
        public SaleService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponce<SaleResponceDto>> CreateAsync(SaleCreateDto dto)
        {
            var warehouse = await _context.Warehouses.FindAsync(dto.WarehouseId);
            if (warehouse == null)
                return ApiResponce<SaleResponceDto>.Fail("Warehouse not found");

            var customer = await _context.Customers.FindAsync(dto.CustomerId);
            if (customer == null)
                return ApiResponce<SaleResponceDto>.Fail("Customer not found");

            var productIds = dto.Items.Select(i => i.ProductId).Distinct().ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            if (products.Count != productIds.Count)
                return ApiResponce<SaleResponceDto>.Fail("One or more products not found");

            if (dto.Items.GroupBy(i => i.ProductId).Any(g => g.Count() > 1))
                return ApiResponce<SaleResponceDto>.Fail("Duplicate products in sale items");

            foreach (var item in dto.Items)
            {
                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(inv =>
                        inv.WarehouseId == dto.WarehouseId &&
                        inv.ProductId == item.ProductId);

                if (inventory == null || inventory.Quantity < item.Quantity)
                {
                    var product = products.First(p => p.Id == item.ProductId);
                    return ApiResponce<SaleResponceDto>.Fail(
                        $"Insufficient stock for product '{product.ProductName}'. " +
                        $"Available: {inventory?.Quantity ?? 0}, Requested: {item.Quantity}");
                }
            }
            var sale = new Sale
            {
                 SaleDate=dto.SaleDate,
                WarehouseId = dto.WarehouseId,
                CustomerId = dto.CustomerId,
                saleItems = dto.Items.Select(i => new SaleItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Quantity * i.UnitPrice
                }).ToList()
            };

            sale.TotalAmount = sale.saleItems.Sum(i => i.Subtotal);

            _context.Sales.Add(sale);

            foreach (var item in sale.saleItems)
            {
                var inventory = await _context.Inventories
                    .FirstOrDefaultAsync(inv =>
                        inv.WarehouseId == dto.WarehouseId &&
                        inv.ProductId == item.ProductId);

                inventory.Quantity -= item.Quantity;
            }

            await _context.SaveChangesAsync();

            var created = await _context.Sales
                .Include(s => s.warehouse)
                .Include(s => s.customer)
                .Include(s => s.saleItems)
                    .ThenInclude(si => si.product)
                .FirstOrDefaultAsync(s => s.Id == sale.Id);

            return ApiResponce<SaleResponceDto>.Ok(MapToResponse(created), "Sale created successfully");
        

 }

        public  async Task<ApiResponce<bool>> DeleteAsync(int id)
        {
            var sale = await _context.Sales
                 .Include(s => s.saleItems)
                 .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null)
                return ApiResponce<bool>.Fail($"Sale with ID {id} not found");

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
            return ApiResponce<bool>.Ok(true, "Sale deleted successfully");
        }

        public async Task<ApiResponce<List<SaleResponceDto>>> GetAllAsync()
        {
            var sales = await _context.Sales
                .Include(s=>s.warehouse)
                .Include(s => s.customer)
                .Include(s => s.saleItems)
                .ThenInclude(si=>si.product)
                .Select(s=> MapToResponse(s))
                .ToListAsync();
            return ApiResponce<List<SaleResponceDto>>.Ok(sales);
        }

        public async Task<ApiResponce<SaleResponceDto>> GetByIdAsync(int id)
        {
            var sale = await _context.Sales
               .Include(s => s.warehouse)
               .Include(s => s.customer)
               .Include(s => s.saleItems)
                   .ThenInclude(si => si.product)
               .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null)
                return ApiResponce<SaleResponceDto>.Fail($"Sale with ID {id} not found");

            return ApiResponce<SaleResponceDto>.Ok(MapToResponse(sale));
        }
        private static SaleResponceDto MapToResponse(Sale s) => new SaleResponceDto
        {
            Id = s.Id,
            SaleDate = s.SaleDate,
            WarehouseId = s.WarehouseId,
            WarehouseName = s.warehouse?.WarehouseName,
            CustomerId = s.CustomerId,
            CustomerName = s.customer?.CustomerName,
            TotalAmount = s.TotalAmount ?? 0,
            Items = s.saleItems?.Select(i => new SaleItemResponceDto
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
