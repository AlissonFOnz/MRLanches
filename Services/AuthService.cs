using Microsoft.IdentityModel.Tokens;
using MRLanches.Users.API.Interfaces;
using MRLanches.Users.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MRLanches.Users.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthService(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public async Task<AuthResponse> AuthenticateAsync(AuthRequest request)
        {
            try
            {
                // Buscar usuário pelo username
                var user = await _userService.GetUserByUsernameAsync(request.Username);
                if (user == null || !user.IsActive)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Usuário ou senha inválidos"
                    };
                }

                // Verificar senha
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Usuário ou senha inválidos"
                    };
                }

                // Gerar token JWT
                var token = GenerateJwtToken(user);

                return new AuthResponse
                {
                    Success = true,
                    Token = token,
                    UserType = user.UserType,
                    Message = "Autenticação realizada com sucesso"
                };
            }
            catch (Exception)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Erro interno durante a autenticação"
                };
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]!);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JwtSettings:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]!);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.UserType.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(
                    Convert.ToDouble(_configuration["JwtSettings:ExpirationHours"])),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
