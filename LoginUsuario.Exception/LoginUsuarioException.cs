using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LoginUsuario.Exception
{
    public abstract class LoginUsuarioException : System.Exception
    {
        public abstract List<string> GetErrorMessage();
        public abstract HttpStatusCode GetStatusCode();
    }
}
