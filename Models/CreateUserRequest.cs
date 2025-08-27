using System.ComponentModel.DataAnnotations;

namespace MRLanches.Users.API.Models
{
    public class CreateUserRequest
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [RegularExpression(@"^\d{7}$", ErrorMessage = "A senha deve conter exatamente 7 números")]
        public string Password { get; set; } = string.Empty;
    }
}
