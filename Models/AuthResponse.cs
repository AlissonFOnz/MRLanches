namespace MRLanches.Users.API.Models
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
        public UserType? UserType { get; set; }
    }
}
