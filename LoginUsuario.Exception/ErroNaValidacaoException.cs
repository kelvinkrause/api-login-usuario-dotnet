using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LoginUsuario.Exception
{
    public class ErroNaValidacaoException : LoginUsuarioException
    {
        private readonly List<string> _erros;
        public ErroNaValidacaoException(List<string> erros) 
        {
            _erros = erros;
        }

        public override List<string> GetErrorMessage() => _erros;

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
    }
}
