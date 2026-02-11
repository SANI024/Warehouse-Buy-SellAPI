using System.ComponentModel.DataAnnotations;

namespace Warehouse_Buy_Sell.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [MaxLength(50)]
        public string Role { get; set; } = "admin";
    }
}
