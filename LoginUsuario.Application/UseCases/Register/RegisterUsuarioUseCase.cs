using FluentValidation;
using LoginUsuario.Comunication.Requests;
using LoginUsuario.Comunication.Responses;
using LoginUsuario.Domain.Entities;
using LoginUsuario.Domain.Interfaces;
using LoginUsuario.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginUsuario.Application.UseCases.Register
{
    public class RegisterUsuarioUseCase
    {
        private readonly IUsuarioRepository _repository;
        private readonly IValidator<RequestRegisterUsuarioJson> _registerValidator;
        public RegisterUsuarioUseCase(IUsuarioRepository repository, IValidator<RequestRegisterUsuarioJson> registerValidator)
        {
            _repository = repository;
            _registerValidator = registerValidator;
        }
        public async Task<ResponseRegisteredUseCase> Execute(RequestRegisterUsuarioJson request)
        {
            var resultados = _registerValidator.Validate(request);

            var usuarioExiste = await _repository.GetByEmailAsync(request.Email);

            if (usuarioExiste is not null)
                resultados.Errors
                    .Add(new FluentValidation.Results.ValidationFailure("Email", "Email já registrado na plataforma."));

            if (resultados.IsValid == false)
            {
                var erroMensagens = resultados.Errors.Select(erro => erro.ErrorMessage).ToList();
                throw new ErroNaValidacaoException(erroMensagens);
            }

            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                CreateAt = DateTime.UtcNow
            };

            await _repository.CreateAsync(usuario);

            return new ResponseRegisteredUseCase
            {
                Nome = usuario.Name,
                Token = "Aqui algum dia terá um token de acesso."
            };

        }
    }
}
