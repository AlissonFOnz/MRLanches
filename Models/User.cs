using System.ComponentModel.DataAnnotations;

namespace MRLanches.Users.API.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        public UserType UserType { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public bool IsActive { get; set; } = true;
    }

    public enum UserType
    {
        User,       // Usuário comum
        MR,         // Sistema MR
        ADM         // Sistema Administrador
    }
}
