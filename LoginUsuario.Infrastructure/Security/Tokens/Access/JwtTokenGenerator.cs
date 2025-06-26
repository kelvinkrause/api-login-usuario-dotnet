using LoginUsuario.Domain.Entities;
using LoginUsuario.Domain.Interfaces.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoginUsuario.Infrastructure.Security.Tokens.Access
{
    public class JwtTokenGenerator
    {
        public class JwtTokenService : ITokenService
        {
            private readonly IConfiguration _configuration;

            public JwtTokenService(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public string GenerateToken(Usuario usuario)
            {
                var claims = GetClaims(usuario);

                var credentials = new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(1),
                    SigningCredentials = credentials
                };

                var handler = new JwtSecurityTokenHandler();
                var token = handler.CreateToken(tokenDescriptor);

                return handler.WriteToken(token);
            }

            private IEnumerable<Claim> GetClaims(Usuario usuario)
            {
                return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("name", usuario.Name)
            };
            }

            private SymmetricSecurityKey GetSecurityKey()
            {
                var secret = _configuration["JwtSettings:SecretKey"];
                if (string.IsNullOrEmpty(secret))
                    throw new InvalidOperationException("Chave JWT não configurada.");

                return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            }
        }
    }
}
