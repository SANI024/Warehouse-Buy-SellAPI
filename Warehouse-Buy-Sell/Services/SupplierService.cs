using Microsoft.EntityFrameworkCore;
using Warehouse_Buy_Sell.Data;
using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Interfaces;
using Warehouse_Buy_Sell.Models;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Warehouse_Buy_Sell.Services
{
    public class SupplierService : ISupplierService
    {
        private AppDbContext _context;
        public SupplierService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponce<SupplierResponceDto>> CreateAsync(SupplierCreateDto dto)
        {
            var exsists = await _context.Suppliers
                .AnyAsync(s=> s.PhoneNumber==dto.PersonalNumber);

            if (exsists)
                return ApiResponce<SupplierResponceDto>.Fail("A supplier with this personal number already exists");

            var supplier = new Supplier
            {
                SupplierName = dto.Name,
                PersonalNumber = dto.PersonalNumber,
                PhoneNumber = dto.Phone
            };
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return ApiResponce<SupplierResponceDto>.Ok(MapToResponse(supplier), "Supplier created successfully");
        }

        public async Task<ApiResponce<bool>> DeleteAsync(int id)
        {

            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
                return ApiResponce<bool>.Fail($"Supplier with ID {id} not found");

            var hasPurchases = await _context.Purchases.AnyAsync(p => p.SupplierId == id);
            if (hasPurchases)
                return ApiResponce<bool>.Fail("Cannot delete supplier with existing purchases");

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return ApiResponce<bool>.Ok(true, "Supplier deleted successfully");
        }

        public async Task<ApiResponce<List<SupplierResponceDto>>> GetAllAsync()
        {
           var suppliers = await _context.Suppliers
                .Select(s=> new SupplierResponceDto 
                {
                    Id  = s.Id,
                    Name =s.SupplierName,
                    PersonalNumber = s.PersonalNumber,
                    Phone = s.PhoneNumber
                    
                }).ToListAsync();

            return ApiResponce<List<SupplierResponceDto>>.Ok(suppliers);
        }

        public async Task<ApiResponce<SupplierResponceDto>> GetByIdAsync(int id)
        {
           var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
                return ApiResponce<SupplierResponceDto>.Fail($"supplier with ID {id} not found");

            return ApiResponce<SupplierResponceDto>.Ok(MapToResponse(supplier));
        }

        public async Task<ApiResponce<SupplierResponceDto>> UpdateAsync(SupplierUpdateDto dto,int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if(supplier == null)
                return ApiResponce<SupplierResponceDto>.Fail($"Supplier with ID {id} not found");

            var exists = await _context.Suppliers
                .AnyAsync(s => s.PersonalNumber == dto.PersonalNumber && s.Id != id);
            if (exists)
                return ApiResponce<SupplierResponceDto>.Fail("Another supplier with this personal number already exists");

            supplier.SupplierName = dto.Name;
            supplier.PersonalNumber = dto.PersonalNumber;
            supplier.PhoneNumber = dto.Phone;

            await _context.SaveChangesAsync();
            return ApiResponce<SupplierResponceDto>.Ok(MapToResponse(supplier), "Supplier updated successfully");
        
        }

        private SupplierResponceDto MapToResponse(Supplier s) => new SupplierResponceDto
        {
            Id = s.Id,
            Name = s.SupplierName,
            PersonalNumber = s.PersonalNumber,
            Phone = s.PhoneNumber,
            
        };
    }
}
