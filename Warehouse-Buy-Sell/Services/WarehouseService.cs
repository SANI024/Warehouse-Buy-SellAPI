using Microsoft.EntityFrameworkCore;
using Warehouse_Buy_Sell.Data;
using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Interfaces;
using Warehouse_Buy_Sell.Models;

namespace Warehouse_Buy_Sell.Services
{
    public class WarehouseService : IWarehouseService
    {
        private AppDbContext _context; 
        public WarehouseService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponce<WarehouseResponceDto>> CreateAsync(WarehouseCreateDto dto)
        {
            var warehouse = new Warehouse 
            {
             WarehouseName=dto.Name,
             Address=dto.Address,
            };

            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();
            return ApiResponce<WarehouseResponceDto>.Ok(MapToResponse(warehouse), "Warehouse created successfully");
        }

        public async Task<ApiResponce<bool>> DeleteAsync(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null)
                return ApiResponce<bool>.Fail($"Warehouse with ID {id} not found");

            var isUsed = await _context.Purchases.AnyAsync(p => p.WarehouseId == id) ||
                         await _context.Sales.AnyAsync(s => s.WarehouseId == id) ||
                         await _context.InternalMovements.AnyAsync(im =>
                             im.FromWarehouseId == id || im.ToWarehouseId == id);
            if (isUsed)
                return ApiResponce<bool>.Fail("Cannot delete warehouse that has existing operations");

            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();
            return ApiResponce<bool>.Ok(true, "Warehouse deleted successfully");
        }

        public async Task<ApiResponce<List<WarehouseResponceDto>>> GetAllAsync()
        {
            var warehouse = await _context.Warehouses
                .Select(w => new WarehouseResponceDto 
                {
                 Id = w.Id,
                 Name = w.WarehouseName,
                 Address = w.Address,

                }).ToListAsync();
            return ApiResponce<List<WarehouseResponceDto>>.Ok(warehouse);
        }

        public async Task<ApiResponce<WarehouseResponceDto>> GetByIdAsync(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if(warehouse == null) 
                return ApiResponce<WarehouseResponceDto>.Fail($"Warehouse with ID {id} not found");

            return ApiResponce<WarehouseResponceDto>.Ok(MapToResponse(warehouse));

        }

        public async Task<ApiResponce<WarehouseResponceDto>> UpdateAsync(WarehouseUpdateDto dto, int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if(warehouse == null)
                return ApiResponce<WarehouseResponceDto>.Fail($"Warehouse with ID {id} not found");

            warehouse.WarehouseName = dto.Name;
            warehouse.Address = dto.Address;
            

            await _context.SaveChangesAsync();
            return ApiResponce<WarehouseResponceDto>.Ok(MapToResponse(warehouse), "Warehouse updated successfully");
        }
        private WarehouseResponceDto MapToResponse(Warehouse w) => new WarehouseResponceDto
        {
            Id = w.Id,
            Name = w.WarehouseName,
            Address = w.Address
        };
    }
}
