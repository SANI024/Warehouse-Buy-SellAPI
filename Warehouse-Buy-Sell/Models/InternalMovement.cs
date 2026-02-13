using System.ComponentModel.DataAnnotations;

namespace Warehouse_Buy_Sell.Models
{
    public class InternalMovement
    {
        public int Id { get; set; }
        public DateTime MovementDate { get; set; }

        [Required]
        public int FromWarehouseId { get; set; }
        public Warehouse fromWarehouse { get; set; }

        [Required]
        public int ToWarehouseId { get;set; }
        public Warehouse toWarehouse { get; set; }

        public ICollection<InternalMovementItem> internalMovementItems { get; set; }
    }
}
