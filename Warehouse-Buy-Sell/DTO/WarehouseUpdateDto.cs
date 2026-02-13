using System.ComponentModel.DataAnnotations;

namespace Warehouse_Buy_Sell.DTO
{
    public class WarehouseUpdateDto
    {
        [Required(ErrorMessage = "Warehouse name is required")]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }
    }
}
