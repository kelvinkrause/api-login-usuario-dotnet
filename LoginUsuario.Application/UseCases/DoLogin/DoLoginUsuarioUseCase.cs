using LoginUsuario.Comunication.Requests;
using LoginUsuario.Comunication.Responses;
using LoginUsuario.Domain.Interfaces;
using LoginUsuario.Exception;

namespace LoginUsuario.Application.UseCases.DoLogin
{
    public class DoLoginUsuarioUseCase
    {
        public readonly IUsuarioRepository _repository;
        public DoLoginUsuarioUseCase(IUsuarioRepository repository)
        {
            _repository = repository;
        }
        public async Task<ResponseLoginUsuarioJson> Execute(RequestLoginUsuarioJson request)
        {
            var usuario = await _repository.GetByEmailAsync(request.Email);

            if (usuario is null || !usuario.Password.Equals(request.Password))
                throw new InvalidLoginException();

            return new ResponseLoginUsuarioJson
            {
                Name = usuario.Name,
                Token = "Aqui algum dia terá um token de acesso."
            };

        }
    }
}
