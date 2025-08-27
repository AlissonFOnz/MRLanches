using MRLanches.Users.API.Models;

namespace MRLanches.Users.API.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(CreateUserRequest request);
        Task<User> UpdateUserAsync(int id, UpdateUserRequest request);
        Task<bool> DeleteUserAsync(int id);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> UserExistsAsync(string username);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
