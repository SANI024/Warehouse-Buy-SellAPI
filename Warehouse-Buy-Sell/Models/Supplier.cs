using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Warehouse_Buy_Sell.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(225)]
        public string SupplierName { get; set; }
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(50)]
        public string PersonalNumber { get; set; }

        public ICollection<Purchase>purchase  { get; set; }
    }
}
