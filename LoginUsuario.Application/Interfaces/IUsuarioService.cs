using LoginUsuario.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginUsuario.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task RegistrarAsync(RegisterUsuarioRequest request);
        Task<LoginUsuarioResponse> LoginAsync(LoginUsuarioRequest request);
    }
}
