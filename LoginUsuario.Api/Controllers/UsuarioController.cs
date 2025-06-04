using LoginUsuario.Application.DTOs;
using LoginUsuario.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoginUsuario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;
        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUsuarioRequest request)
        {
            await _service.RegistrarAsync(request);
            return Ok(new { message = "Usuário cadastrado com sucesso." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUsuarioRequest request)
        {
            var response = await _service.LoginAsync(request);
            return Ok(response);
        }
    }
}
