using LoginUsuario.Comunication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoginUsuario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConectadoController : ControllerBase
    {
        [HttpGet("autenticado")]
        [Authorize]
        [ProducesResponseType(typeof(ResponseSucessAutentication), StatusCodes.Status200OK)]
        public IActionResult ObterPerfil()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub)?.Value;

            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value
                ?? User.FindFirst(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Email)?.Value;

            var userName = User.FindFirst("name")?.Value;

            var autenticado = new ResponseSucessAutentication
            {
                Id = userId,
                Email = userEmail,
                Nome = userName
            };

            return Ok(autenticado);
        }
    }
}
