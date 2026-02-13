using Microsoft.EntityFrameworkCore;
using Warehouse_Buy_Sell.Data;
using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Interfaces;
using Warehouse_Buy_Sell.Models;

namespace Warehouse_Buy_Sell.Services
{
    public class AuthService : IAuthService
    {
        private AppDbContext _context;
        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> LoginAsync(UserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                throw new Exception("username allready exsists");
            var user = new User
            {
                Username = dto.Username,
                PasswordHash=dto.Password,
                Role= dto.Role,
                Email= dto.Email,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;


        }
        public async Task<User> RegisterAsync(UserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null) throw new Exception("please enter username");
            if (user.PasswordHash != dto.Password) throw new Exception("password is incorrect");
            return user;
        }
    }
}
