using System.ComponentModel.DataAnnotations;

namespace MRLanches.Users.API.Models
{
    public class AuthRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
