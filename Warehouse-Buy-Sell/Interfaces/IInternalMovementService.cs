using Warehouse_Buy_Sell.DTO;

namespace Warehouse_Buy_Sell.Interfaces
{
    public interface IInternalMovementService
    {
        Task<ApiResponce<List<InternalMovementResponceDto>>> GetAllAsync();
        Task<ApiResponce<InternalMovementResponceDto>> GetByIdAsync(int id);
        Task<ApiResponce<InternalMovementResponceDto>> CreateAsync(InternalMovementCreateDto dto);
        Task<ApiResponce<bool>> DeleteAsync(int id);
    }
}
