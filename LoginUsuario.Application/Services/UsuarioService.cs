using LoginUsuario.Application.DTOs;
using LoginUsuario.Application.Interfaces;
using LoginUsuario.Domain.Entities;
using LoginUsuario.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginUsuario.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }
        public async Task<LoginUsuarioResponse> LoginAsync(LoginUsuarioRequest request)
        {
            var usuario = await _repository.GetByEmailAsync(request.Email);

            if (usuario.Password != request.Password || usuario is null)
                throw new Exception("Usuário e/ou senha inválidos.");

            return new LoginUsuarioResponse
            {
                Name = usuario.Name,
                Token = "Aqui algum dia terá um token de acesso."
            };

        }

        public async Task RegistrarAsync(RegisterUsuarioRequest request)
        {
            var usuarioExist = await _repository.GetByEmailAsync(request.Email);

            if (usuarioExist != null)
                throw new Exception("Já existe um usuário cadastrado com este e-mail");

            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                CreateAt = DateTime.UtcNow               
            };

            await _repository.CreateAsync(usuario);

        }
    }
}
