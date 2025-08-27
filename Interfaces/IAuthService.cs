using MRLanches.Users.API.Models;

namespace MRLanches.Users.API.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(AuthRequest request);
        Task<bool> ValidateTokenAsync(string token);
        string GenerateJwtToken(User user);
    }
}
