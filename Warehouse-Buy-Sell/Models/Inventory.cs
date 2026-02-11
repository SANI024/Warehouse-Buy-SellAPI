using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_Buy_Sell.Models
{
    public class Inventory
    {
        public int Id { get; set; }

        [Required]
        public int WarehouseId { get; set; }
        public Warehouse warehouse { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product product { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Quantity { get; set; } = 0;
    }
}
