using LoginUsuario.Application.UseCases.DoLogin;
using LoginUsuario.Application.UseCases.Register;
using LoginUsuario.Comunication.Requests;
using LoginUsuario.Comunication.Responses;
using LoginUsuario.Exception;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoginUsuario.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly RegisterUsuarioUseCase _registerUsuarioUseCase;
        private readonly DoLoginUsuarioUseCase _doLoginUsuarioUseCase;
        public UsuarioController(RegisterUsuarioUseCase registerUsuarioUseCase,
                                 DoLoginUsuarioUseCase doLoginUsuarioUseCase)
        {
            _registerUsuarioUseCase = registerUsuarioUseCase;
            _doLoginUsuarioUseCase = doLoginUsuarioUseCase;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseLoginUsuarioJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] RequestLoginUsuarioJson request)
        {
            try
            {
                var response = await _doLoginUsuarioUseCase.Execute(request);
                return Ok(response);
            }
            catch (LoginUsuarioException login)
            {
                return Unauthorized(new ResponseErrorMessageJson
                {
                    Errors = login.GetErrorMessage()
                });
            }

        }
        [HttpPost("register")]
        [ProducesResponseType(typeof(ResponseRegisteredUseCase), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorMessageJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RequestRegisterUsuarioJson request)
        {
            try
            {
                var response = await _registerUsuarioUseCase.Execute(request);
                return Created(string.Empty, response);

            }
            catch (LoginUsuarioException register)
            {
                return BadRequest(new ResponseErrorMessageJson
                {
                    Errors = register.GetErrorMessage()
                });
            }
        }
    }
}
