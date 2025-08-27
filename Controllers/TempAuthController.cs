using Microsoft.AspNetCore.Mvc;

namespace MRLanches.Users.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TempAuthController : ControllerBase
    {
        /// <summary>
        /// Endpoint temporário para teste do Swagger
        /// </summary>
        /// <returns>Mensagem de teste</returns>
        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return Ok("API funcionando! Swagger está ativo.");
        }

        /// <summary>
        /// Endpoint de teste com parâmetros
        /// </summary>
        /// <param name="name">Nome para teste</param>
        /// <returns>Mensagem personalizada</returns>
        [HttpGet("hello/{name}")]
        public ActionResult<string> Hello(string name)
        {
            return Ok($"Olá {name}! A API MRLanches Users está funcionando perfeitamente.");
        }

        /// <summary>
        /// Endpoint POST de teste
        /// </summary>
        /// <param name="request">Dados de teste</param>
        /// <returns>Resposta de teste</returns>
        [HttpPost("echo")]
        public ActionResult<object> Echo([FromBody] object request)
        {
            return Ok(new { 
                message = "Dados recebidos com sucesso!", 
                data = request,
                timestamp = DateTime.UtcNow
            });
        }
    }
}
