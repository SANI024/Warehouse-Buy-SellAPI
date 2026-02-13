using System.ComponentModel.DataAnnotations;

namespace Warehouse_Buy_Sell.DTO
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(100, ErrorMessage = "Username cannot exceed 100 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Password must contain uppercase, lowercase and a number")]
        public string Password { get; set; }
    }
}
