using LoginUsuario.Domain.Entities;
using LoginUsuario.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginUsuario.Infrastructure.Security.Cryptography
{
    public class CryptographyService : ICryptographyService
    {
        public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
        public bool Verify(string password, Usuario usuario) => BCrypt.Net.BCrypt.Verify(password, usuario.Password);
    }
}
