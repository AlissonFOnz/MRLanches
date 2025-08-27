using Microsoft.AspNetCore.Mvc;
using MRLanches.Users.API.Interfaces;
using MRLanches.Users.API.Models;

namespace MRLanches.Users.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Endpoint para autenticação de usuários e sistemas
        /// </summary>
        /// <param name="request">Dados de autenticação (username e senha)</param>
        /// <returns>Token JWT em caso de sucesso</returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
        {
            try
            {
                var response = await _authService.AuthenticateAsync(request);
                
                if (response.Success)
                {
                    return Ok(response);
                }
                
                return Unauthorized(response);
            }
            catch (Exception)
            {
                return StatusCode(500, new AuthResponse
                {
                    Success = false,
                    Message = "Erro interno do servidor"
                });
            }
        }
    }
}
