using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_Buy_Sell.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        [Required]
        public DateTime PurchaseDate {  get; set; }
        [Required]
        public int WarehouseId {  get; set; }
        public Warehouse warehouse { get; set; }
        [Required]
        public int SupplierId { get; set; }
        public Supplier supplier { get; set; }
        [Column(TypeName = "decimal(12,2)")]
        public decimal? TotalAmount { get; set; }

        public ICollection<PurchaseItems> purchaseItems { get; set; }
    }
}
