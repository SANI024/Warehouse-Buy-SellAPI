using Microsoft.EntityFrameworkCore;
using Warehouse_Buy_Sell.Data;
using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Interfaces;
using Warehouse_Buy_Sell.Models;

namespace Warehouse_Buy_Sell.Services
{
    public class CustomerService : ICustomerService

    {
        private AppDbContext _context;
        public CustomerService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponce<CustomerResponceDto>> CreateAsync(CustomerCreateDto dto)
        {
            var exists = await _context.Customers
                .AnyAsync(c=> c.PersonalNumber==dto.PersonalNumber);
            if(exists)
                return ApiResponce<CustomerResponceDto>.Fail("A customer with this personal number already exists");

            var customer = new Customer
            {
                CustomerName = dto.Name,
                PersonalNumber = dto.PersonalNumber,
                PhoneNumber = dto.Phone,
                Address = dto.Address
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return ApiResponce<CustomerResponceDto>.Ok(MapToResponse(customer), "Customer created successfully");
        
        }

        public async Task<ApiResponce<bool>> DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return ApiResponce<bool>.Fail($"Customer with ID {id} not found");

            var hasSales = await _context.Sales.AnyAsync(s => s.CustomerId == id);
            if (hasSales)
                return ApiResponce<bool>.Fail("Cannot delete customer with existing sales");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return ApiResponce<bool>.Ok(true, "Customer deleted successfully");
        }

        public async Task<ApiResponce<List<CustomerResponceDto>>> GetAllAsync()
        {
            var customers = await _context.Customers
                .Select(c => new CustomerResponceDto
                {
                    Id= c.Id,
                    Name=c.CustomerName,
                    PersonalNumber=c.PersonalNumber,
                    Phone=c.PhoneNumber,
                    Address=c.Address,

                }).ToListAsync();
            return ApiResponce<List<CustomerResponceDto>>.Ok(customers);
        }

        public async Task<ApiResponce<CustomerResponceDto>> GetByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if(customer == null) 
                return ApiResponce<CustomerResponceDto>.Fail($"Customer with ID {id} not found");

            return ApiResponce<CustomerResponceDto>.Ok(MapToResponse(customer));
        }

        public async Task<ApiResponce<CustomerResponceDto>> UpdateAsync(CustomerUpdateDto dto, int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return ApiResponce<CustomerResponceDto>.Fail($"Customer with ID {id} not found");

            var exists = await _context.Customers
                .AnyAsync(c => c.PersonalNumber == dto.PersonalNumber && c.Id != id);
            if (exists)
                return ApiResponce<CustomerResponceDto>.Fail("Another customer with this personal number already exists");

            customer.CustomerName = dto.Name;
            customer.PersonalNumber = dto.PersonalNumber;
            customer.PhoneNumber = dto.Phone;
            customer.Address = dto.Address;

            await _context.SaveChangesAsync();
            return ApiResponce<CustomerResponceDto>.Ok(MapToResponse(customer), "Customer updated successfully");
        }
        

        private CustomerResponceDto MapToResponse(Customer c) => new CustomerResponceDto
        {
            Id = c.Id,
            Name = c.CustomerName,
            PersonalNumber = c.PersonalNumber,
            Phone = c.PhoneNumber,
            Address = c.Address,
        };
    }
}
