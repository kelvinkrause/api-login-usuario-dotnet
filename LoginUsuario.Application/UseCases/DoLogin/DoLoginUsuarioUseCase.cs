using LoginUsuario.Comunication.Requests;
using LoginUsuario.Comunication.Responses;
using LoginUsuario.Domain.Interfaces;
using LoginUsuario.Domain.Interfaces.Security;
using LoginUsuario.Exception;

namespace LoginUsuario.Application.UseCases.DoLogin
{
    public class DoLoginUsuarioUseCase
    {
        private readonly IUsuarioRepository _repository;
        private readonly ICryptographyService _algorithm;
        public DoLoginUsuarioUseCase(IUsuarioRepository repository, ICryptographyService algorithm)
        {
            _repository = repository;
            _algorithm = algorithm;
        }
        public async Task<ResponseLoginUsuarioJson> Execute(RequestLoginUsuarioJson request)
        {
            var usuario = await _repository.GetByEmailAsync(request.Email);

            if (usuario is null || !_algorithm.Verify(request.Password, usuario))
                throw new InvalidLoginException();

            return new ResponseLoginUsuarioJson
            {
                Name = usuario.Name,
                Token = "Aqui algum dia terá um token de acesso."
            };

        }
    }
}
