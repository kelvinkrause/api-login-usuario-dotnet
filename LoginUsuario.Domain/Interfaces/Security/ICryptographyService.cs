using LoginUsuario.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginUsuario.Domain.Interfaces.Security
{
    public interface ICryptographyService
    {
        public string HashPassword(string password);
        public bool Verify(string password, Usuario usuario);
    }
}
