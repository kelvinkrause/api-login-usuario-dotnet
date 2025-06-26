using LoginUsuario.Comunication.Requests;
using LoginUsuario.Comunication.Responses;
using LoginUsuario.Domain.Interfaces;
using LoginUsuario.Domain.Interfaces.Security;
using LoginUsuario.Exception;
using System.Runtime.CompilerServices;

namespace LoginUsuario.Application.UseCases.DoLogin
{
    public class DoLoginUsuarioUseCase
    {
        private readonly IUsuarioRepository _repository;
        private readonly ICryptographyService _algorithm;
        private readonly ITokenService _tokenService;
        public DoLoginUsuarioUseCase(
            IUsuarioRepository repository, 
            ICryptographyService algorithm,
            ITokenService tokenService)
        {
            _repository = repository;
            _algorithm = algorithm;
            _tokenService = tokenService;
        }
        public async Task<ResponseLoginUsuarioJson> Execute(RequestLoginUsuarioJson request)
        {
            var usuario = await _repository.GetByEmailAsync(request.Email);

            if (usuario is null || !_algorithm.Verify(request.Password, usuario))
                throw new InvalidLoginException();

            return new ResponseLoginUsuarioJson
            {
                Name = usuario.Name,
                Token = _tokenService.GenerateToken(usuario)
            };

        }
    }
}
