using MRLanches.Users.API.Models;

namespace MRLanches.Users.API.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> ExistsAsync(string username);
        Task<IEnumerable<User>> GetAllAsync();
    }
}
