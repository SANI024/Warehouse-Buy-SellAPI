using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Models;

namespace Warehouse_Buy_Sell.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(UserDto dto);
        Task<User> LoginAsync(UserDto dto);
    }
}
