using Warehouse_Buy_Sell.DTO;

namespace Warehouse_Buy_Sell.Interfaces
{
    public interface ICustomerService
    {
        Task<ApiResponce<List<CustomerResponceDto>>> GetAllAsync();
        Task<ApiResponce<CustomerResponceDto>>GetByIdAsync(int id);
        Task<ApiResponce<CustomerResponceDto>> CreateAsync(CustomerCreateDto dto);
        Task<ApiResponce<CustomerResponceDto>> UpdateAsync(CustomerUpdateDto dto,int id);
        Task<ApiResponce<bool>>DeleteAsync (int id);
    }
}
