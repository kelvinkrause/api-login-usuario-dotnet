using LoginUsuario.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginUsuario.Domain.Interfaces.Security
{
    public interface ITokenService
    {
        string GenerateToken(Usuario usuario); 
    }
}
