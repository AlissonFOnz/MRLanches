using System.ComponentModel.DataAnnotations;

namespace MRLanches.Users.API.Models
{
    public class UpdateUserRequest
    {
        [StringLength(50)]
        public string? Username { get; set; }
        
        [RegularExpression(@"^\d{7}$", ErrorMessage = "A senha deve conter exatamente 7 números")]
        public string? Password { get; set; }
        
        public bool? IsActive { get; set; }
    }
}
