using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voluta.Data;
using Voluta.Models;

namespace Voluta.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly VolutaDbContext _context;

        public UsuarioRepository(VolutaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync(int skip, int take)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Usuarios.CountAsync();
        }

        public async Task<Usuario> AddAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Usuarios.AnyAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<Usuario>> GetDisponiveisByAreasAsync(IEnumerable<AreaAtuacao> areas, int skip, int take)
        {
            var query = _context.Usuarios
                .AsNoTracking()
                .Where(u => u.Disponivel);

            if (areas?.Any() == true)
            {
                query = query.Where(u => 
                    u.AreasInteresseJson != null &&
                    u.AreasInteresse.Any(ua => areas.Contains(ua)));
            }

            return await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<int> GetDisponiveisByAreasCountAsync(IEnumerable<AreaAtuacao> areas)
        {
            var query = _context.Usuarios
                .Where(u => u.Disponivel);

            if (areas?.Any() == true)
            {
                query = query.Where(u => 
                    u.AreasInteresseJson != null &&
                    u.AreasInteresse.Any(ua => areas.Contains(ua)));
            }

            return await query.CountAsync();
        }
    }
} 