using Warehouse_Buy_Sell.DTO;
using Warehouse_Buy_Sell.Models;

namespace Warehouse_Buy_Sell.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponce<AuthResponceDto>> LoginAsync(LoginDto dto);
        Task<ApiResponce<AuthResponceDto>> RegisterAsync(RegisterDto dto);
    }
}
