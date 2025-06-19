using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LoginUsuario.Exception
{
    public class InvalidLoginException : LoginUsuarioException
    {
        public override List<string> GetErrorMessage() => ["Usuário e/ou senha inválidos."];
        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    }
}
