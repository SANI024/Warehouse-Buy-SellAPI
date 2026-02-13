using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Warehouse_Buy_Sell.Data;
using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Interfaces;
using Warehouse_Buy_Sell.Models;
using Microsoft.AspNetCore.Authorization;

namespace Warehouse_Buy_Sell.Services
{
    public class AuthService : IAuthService
    {
        private AppDbContext _context;
        private IConfiguration _config;
        public AuthService(AppDbContext context, IConfiguration config  )
        {
            _context = context;
            _config = config;
        }

        public async Task<ApiResponce<AuthResponceDto>> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return ApiResponce<AuthResponceDto>.Fail("Invalid username or password");

            var token = GenerateJwtToken(user);

            return ApiResponce<AuthResponceDto>.Ok(new AuthResponceDto
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            });
        }

        public async Task<ApiResponce<AuthResponceDto>> RegisterAsync(RegisterDto dto)
        {
            var usernameExists = await _context.Users.AnyAsync(u => u.Username == dto.Username);
            if (usernameExists)
                return ApiResponce<AuthResponceDto>.Fail("Username already taken");

            var emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (emailExists)
                return ApiResponce<AuthResponceDto>.Fail("Email already in use");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "admin"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);

            return ApiResponce<AuthResponceDto>.Ok(new AuthResponceDto
            {
                Token = null,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            }, "Registration successful");
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12), 
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
