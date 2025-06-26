using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginUsuario.Comunication.Responses
{
    public class ResponseSucessAutentication
    {
        public string Mensagem => "Você esta autenticando.";
        public string Id { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
