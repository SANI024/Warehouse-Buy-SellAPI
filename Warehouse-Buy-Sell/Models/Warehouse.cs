using System.ComponentModel.DataAnnotations;

namespace Warehouse_Buy_Sell.Models
{
    public class Warehouse
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(225)]
        public string WarehouseName {  get; set; }
        public  string Address { get; set; }

        public ICollection<Purchase> purchases { get; set; }
        public ICollection<Sale> sales { get; set; }
        public ICollection<InternalMovement> MovementsFrom { get; set; }
        public ICollection<InternalMovement> MovementsTo { get; set; }
        public ICollection<Inventory> Inventories { get; set; }
    }
}
