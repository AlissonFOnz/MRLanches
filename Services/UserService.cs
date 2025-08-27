using MRLanches.Users.API.Interfaces;
using MRLanches.Users.API.Models;

namespace MRLanches.Users.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateUserAsync(CreateUserRequest request)
        {
            // Verificar se o usuário já existe
            if (await _userRepository.ExistsAsync(request.Username))
            {
                throw new InvalidOperationException("Usuário já existe");
            }

            // Criar hash da senha
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                UserType = UserType.User, // Usuários criados são sempre do tipo User
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            return await _userRepository.CreateAsync(user);
        }

        public async Task<User> UpdateUserAsync(int id, UpdateUserRequest request)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                throw new InvalidOperationException("Usuário não encontrado");
            }

            // Atualizar username se fornecido
            if (!string.IsNullOrEmpty(request.Username))
            {
                // Verificar se o novo username já existe (exceto para o usuário atual)
                if (request.Username != existingUser.Username && 
                    await _userRepository.ExistsAsync(request.Username))
                {
                    throw new InvalidOperationException("Username já existe");
                }
                existingUser.Username = request.Username;
            }

            // Atualizar senha se fornecida
            if (!string.IsNullOrEmpty(request.Password))
            {
                existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            }

            // Atualizar status se fornecido
            if (request.IsActive.HasValue)
            {
                existingUser.IsActive = request.IsActive.Value;
            }

            existingUser.UpdatedAt = DateTime.UtcNow;

            return await _userRepository.UpdateAsync(existingUser);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _userRepository.ExistsAsync(username);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }
    }
}
