using LoginUsuario.Domain.Entities;
using LoginUsuario.Domain.Interfaces;
using LoginUsuario.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginUsuario.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UsuarioDbContext _context;
        public UsuarioRepository(UsuarioDbContext context) 
        {
            _context = context;
        }
        public async Task CreateAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
             
            var contato = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
            if (contato == null)
                return null;
            return contato;
        }

        public async Task<Usuario> GetByIdAsync(Guid id)
        {
            var contato = await _context.Usuarios.FindAsync(id);
            if (contato == null)
                return null;
            return contato;
        }
    }
}
