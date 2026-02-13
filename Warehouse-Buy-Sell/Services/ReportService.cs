using Microsoft.EntityFrameworkCore;
using Warehouse_Buy_Sell.Data;
using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Interfaces;

namespace Warehouse_Buy_Sell.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponce<ProfitReportDto>> GetProfitReportAsync(DateTime? from, DateTime? to)
        {
            var salesQuery = _context.SaleItems
               .Include(si => si.sale)
               .Include(si => si.product)
               .AsQueryable();

            var purchasesQuery = _context.PurchaseItems
                .Include(pi => pi.purchase)
                .Include(pi => pi.product)
                .AsQueryable();

            if (from.HasValue)
            {
                salesQuery = salesQuery.Where(si => si.sale.SaleDate >= from.Value);
                purchasesQuery = purchasesQuery.Where(pi => pi.purchase.PurchaseDate >= from.Value);
            }

            if (to.HasValue)
            {
                salesQuery = salesQuery.Where(si => si.sale.SaleDate <= to.Value);
                purchasesQuery = purchasesQuery.Where(pi => pi.purchase.PurchaseDate <= to.Value);
            }

            var saleItems = await salesQuery.ToListAsync();
            var purchaseItems = await purchasesQuery.ToListAsync();

            var productIds = saleItems.Select(s => s.ProductId)
                .Union(purchaseItems.Select(p => p.ProductId))
                .Distinct();

            var productProfits = productIds.Select(productId =>
            {
                var revenue = saleItems
                    .Where(s => s.ProductId == productId)
                    .Sum(s => s.Subtotal);

                var cost = purchaseItems
                    .Where(p => p.ProductId == productId)
                    .Sum(p => p.Subtotal);

                var productName = saleItems.FirstOrDefault(s => s.ProductId == productId)?.product?.ProductName
                    ?? purchaseItems.FirstOrDefault(p => p.ProductId == productId)?.product?.ProductName;

                return new ProductProfitDto
                {
                    ProductId = productId,
                    ProductName = productName,
                    TotalRevenue = revenue,
                    TotalCost = cost,
                    Profit = revenue - cost
                };
            }).ToList();

            var report = new ProfitReportDto
            {
                TotalRevenue = productProfits.Sum(p => p.TotalRevenue),
                TotalCost = productProfits.Sum(p => p.TotalCost),
                TotalProfit = productProfits.Sum(p => p.Profit),
                ProductProfits = productProfits
            };
            return ApiResponce<ProfitReportDto>.Ok(report);
        }

        public async Task<ApiResponce<List<StockReportDto>>> GetStockReportAsync(int? warehouseId)
        {
            var query = _context.Inventories
                 .Include(i => i.warehouse)
                 .Include(i => i.product)
                 .AsQueryable();

            if (warehouseId.HasValue)
                query = query.Where(i => i.WarehouseId == warehouseId.Value);

            var inventory = await query.ToListAsync();

            var report = inventory
                .GroupBy(i => new { i.WarehouseId, i.warehouse.WarehouseName })
                .Select(g => new StockReportDto
                {
                    WarehouseId = g.Key.WarehouseId,
                    WarehouseName = g.Key.WarehouseName,
                    Items = g.Select(i => new StockItemDto
                    {
                        ProductId = i.ProductId,
                        ProductName = i.product.ProductName,
                        Quantity = i.Quantity
                    }).ToList()
                }).ToList();

            return ApiResponce<List<StockReportDto>>.Ok(report);
        }
    }
}
