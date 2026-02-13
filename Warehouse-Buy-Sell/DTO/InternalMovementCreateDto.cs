using System.ComponentModel.DataAnnotations;

namespace Warehouse_Buy_Sell.DTO
{
    public class InternalMovementCreateDto
    {
        [Required(ErrorMessage = "Movement date is required")]
        public DateTime MovementDate { get; set; }

        [Required(ErrorMessage = "Source warehouse ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid warehouse ID")]
        public int FromWarehouseId { get; set; }

        [Required(ErrorMessage = "Destination warehouse ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid warehouse ID")]
        public int ToWarehouseId { get; set; }

        [Required(ErrorMessage = "At least one item is required")]
        [MinLength(1, ErrorMessage = "At least one item is required")]
        public List<InternalMovementItemCreateDto> Items { get; set; }
    }
}
