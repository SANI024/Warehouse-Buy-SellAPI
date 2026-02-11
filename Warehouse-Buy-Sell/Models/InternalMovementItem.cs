using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_Buy_Sell.Models
{
    public class InternalMovementItem
    {
        public int Id { get; set; }

        [Required]
        public int MovementId { get; set; }
        public InternalMovement internalMovement { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product product { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Quantity { get; set; }
    }
}
