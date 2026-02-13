using System.ComponentModel.DataAnnotations;

namespace Warehouse_Buy_Sell.DTO
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Product name is required")]
        [MaxLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Purchase price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Purchase price must be greater than 0")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "Sale price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Sale price must be greater than 0")]
        public decimal SalePrice { get; set; }
    }
}
