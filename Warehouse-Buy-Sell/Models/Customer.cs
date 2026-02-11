using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Warehouse_Buy_Sell.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(225)]
        public string CustomerName { get; set; }
        [Required]
        [MaxLength(50)]
        public string PersonalNumber { get; set; }
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public ICollection<Sale> sale { get; set; }

    }
}
