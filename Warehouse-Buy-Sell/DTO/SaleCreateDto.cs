using System.ComponentModel.DataAnnotations;

namespace Warehouse_Buy_Sell.DTO
{
    public class SaleCreateDto
    {

        [Required(ErrorMessage = "Sale date is required")]
        public DateTime SaleDate { get; set; }

        [Required(ErrorMessage = "Warehouse ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid warehouse ID")]
        public int WarehouseId { get; set; }

        [Required(ErrorMessage = "Customer ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid customer ID")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "At least one item is required")]
        [MinLength(1, ErrorMessage = "At least one item is required")]
        public List<SaleItemCreateDto> Items { get; set; }
    }
}

