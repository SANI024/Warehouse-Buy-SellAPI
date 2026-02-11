using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_Buy_Sell.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(225)]
        public string ProductName { get; set; }
        [Required]
        [Column(TypeName="decimal(10,2)")]
        public decimal PurchasePrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal SalePrice { get; set; }

        public ICollection<PurchaseItems> purchaseItems { get; set; }
        public ICollection<SaleItem> saleItems { get; set; }
        public ICollection<InternalMovementItem> internalMovementItems { get; set; }
        public ICollection<Inventory> inventories { get; set; }
    }
}
