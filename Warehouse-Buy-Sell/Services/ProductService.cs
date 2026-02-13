using Microsoft.EntityFrameworkCore;
using Warehouse_Buy_Sell.Data;
using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Interfaces;
using Warehouse_Buy_Sell.Models;

namespace Warehouse_Buy_Sell.Services
{
    public class ProductService : IProductService
    {
        private AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponce<ProductResponceDto>> CreateAsync(ProductCreateDto dto)
        {
            if (dto.SalePrice < dto.PurchasePrice)
                return ApiResponce<ProductResponceDto>.Fail("Sale price should not be less than purchase price");

            var product = new Product
            {
                ProductName = dto.Name,
                PurchasePrice = dto.PurchasePrice,
                SalePrice = dto.SalePrice
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return ApiResponce<ProductResponceDto>.Ok(MapToResponse(product), "Product created successfully");

        }
        public async Task<ApiResponce<bool>> DeleteAsync(int id)
        {
            var product= await _context.Products.FindAsync(id);
            if (product == null)
                return ApiResponce<bool>.Fail($"Product with ID {id} not found");

            var isUsed = await _context.PurchaseItems.AnyAsync(pi => pi.ProductId == id) ||
                         await _context.SaleItems.AnyAsync(si => si.ProductId == id);
            if(isUsed)
                return ApiResponce<bool>.Fail("Cannot delete product that has been used in purchases or sales");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return ApiResponce<bool>.Ok(true, "Product deleted successfully");
        }

        public async Task<ApiResponce<List<ProductResponceDto>>> GetAllAsync()
        {

            var products = await _context.Products
                .Select(p => new ProductResponceDto
                {
                    Id = p.Id,
                    Name = p.ProductName,
                    PurchasePrice = p.PurchasePrice,
                    SalePrice = p.SalePrice,
             
                }).ToListAsync();

            return ApiResponce<List<ProductResponceDto>>.Ok(products); ;
        }

        public async Task<ApiResponce<ProductResponceDto>> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return ApiResponce<ProductResponceDto>.Fail($"Product with ID {id} not found");

            return ApiResponce<ProductResponceDto>.Ok(MapToResponse(product));
        }

        public async Task<ApiResponce<ProductResponceDto>> UpdateAsync(ProductUpdateDto dto, int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return ApiResponce<ProductResponceDto>.Fail($"Product with ID {id} not found");

            if (dto.SalePrice < dto.PurchasePrice)
                return ApiResponce<ProductResponceDto>.Fail("Sale price should not be less than purchase price");

            product.ProductName = dto.Name;
            product.PurchasePrice = dto.PurchasePrice;
            product.SalePrice = dto.SalePrice;

            await _context.SaveChangesAsync();
            return ApiResponce<ProductResponceDto>.Ok(MapToResponse(product), "Product updated successfully");

        }
        private ProductResponceDto MapToResponse(Product p) => new ProductResponceDto
        {
            Id = p.Id,
            Name = p.ProductName,
            PurchasePrice = p.PurchasePrice,
            SalePrice = p.SalePrice
        };
    }
}
