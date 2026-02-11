using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse_Buy_Sell.Models
{
    public class Sale
    {
        public int Id { get; set; }

        [Required]
        public int WarehouseId { get; set; }
        public Warehouse warehouse { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public Customer customer { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal? TotalAmount { get; set; }

        public ICollection<SaleItem> saleItems { get; set; }
    }
}
