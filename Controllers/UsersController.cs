using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRLanches.Users.API.Interfaces;
using MRLanches.Users.API.Models;
using System.Security.Claims;

namespace MRLanches.Users.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Endpoint para cadastrar novo usuário (apenas ADM pode criar usuários)
        /// </summary>
        /// <param name="request">Dados do usuário a ser criado</param>
        /// <returns>Usuário criado</returns>
        [HttpPost]
        [Authorize(Roles = "ADM")]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                var user = await _userService.CreateUserAsync(request);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Endpoint para alterar usuário existente (apenas ADM pode alterar qualquer usuário)
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <param name="request">Dados para atualização</param>
        /// <returns>Usuário atualizado</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "ADM")]
        public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(id, request);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Endpoint para deletar usuário (apenas ADM pode deletar usuários)
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <returns>Confirmação da exclusão</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADM")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (result)
                {
                    return Ok(new { message = "Usuário deletado com sucesso" });
                }
                return NotFound(new { message = "Usuário não encontrado" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Endpoint para buscar usuário por ID (ADM pode buscar qualquer usuário, MR pode buscar usuários comuns)
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <returns>Usuário encontrado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                // ADM pode buscar qualquer usuário
                if (currentUserRole == "ADM")
                {
                    var user = await _userService.GetUserByIdAsync(id);
                    if (user == null)
                        return NotFound(new { message = "Usuário não encontrado" });
                    
                    return Ok(user);
                }

                // MR pode buscar usuários comuns
                if (currentUserRole == "MR")
                {
                    var user = await _userService.GetUserByIdAsync(id);
                    if (user == null || user.UserType != UserType.User)
                        return NotFound(new { message = "Usuário não encontrado" });
                    
                    return Ok(user);
                }

                // Usuários comuns só podem buscar a si mesmos
                if (currentUserId == id)
                {
                    var user = await _userService.GetUserByIdAsync(id);
                    if (user == null)
                        return NotFound(new { message = "Usuário não encontrado" });
                    
                    return Ok(user);
                }

                return Forbid();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Endpoint para listar todos os usuários (apenas ADM pode listar todos)
        /// </summary>
        /// <returns>Lista de usuários</returns>
        [HttpGet]
        [Authorize(Roles = "ADM")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }
    }
}
