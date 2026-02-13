using System.ComponentModel.DataAnnotations;

namespace Warehouse_Buy_Sell.DTO
{
    public class PurchaseCreateDto
    {
        [Required(ErrorMessage = "Purchase date is required")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Warehouse ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid warehouse ID")]
        public int WarehouseId { get; set; }

        [Required(ErrorMessage = "Supplier ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid supplier ID")]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "At least one item is required")]
        [MinLength(1, ErrorMessage = "At least one item is required")]
        public List<PurchaseItemCreateDto> Items { get; set; }
    }
}
