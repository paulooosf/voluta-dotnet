using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voluta.Data;
using Voluta.Models;
using System.Text.Json;

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
                .OrderBy(u => u.Nome)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.SolicitacoesVoluntariado
                    .Where(s => s.Status == StatusSolicitacao.Pendente))
                .FirstOrDefaultAsync(u => u.Id == id);
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
            var entry = _context.Entry(usuario);
            if (entry.State == EntityState.Detached)
            {
                var existingUsuario = await _context.Usuarios.FindAsync(usuario.Id);
                if (existingUsuario != null)
                {
                    _context.Entry(existingUsuario).CurrentValues.SetValues(usuario);
                }
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Usuarios
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .AnyAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<Usuario>> GetDisponiveisByAreasAsync(IEnumerable<AreaAtuacao> areas, int skip, int take)
        {
            var query = _context.Usuarios
                .AsNoTracking()
                .Where(u => u.Disponivel);

            if (areas?.Any() == true)
            {
                var areasList = areas.Select(a => (int)a).ToList();
                query = query.Where(u => 
                    u.AreasInteresseJson != null &&
                    areasList.Any(area => u.AreasInteresseJson.Contains(area.ToString())));
            }

            return await query
                .OrderBy(u => u.Nome)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<int> GetDisponiveisByAreasCountAsync(IEnumerable<AreaAtuacao> areas)
        {
            var query = _context.Usuarios
                .AsNoTracking()
                .Where(u => u.Disponivel);

            if (areas?.Any() == true)
            {
                var areasList = areas.Select(a => (int)a).ToList();
                query = query.Where(u => 
                    u.AreasInteresseJson != null &&
                    areasList.Any(area => u.AreasInteresseJson.Contains(area.ToString())));
            }

            return await query.CountAsync();
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<Usuario>> GetVoluntariosByOngAsync(int ongId)
        {
            return await _context.Ongs
                .Include(o => o.Voluntarios)
                .Where(o => o.Id == ongId)
                .SelectMany(o => o.Voluntarios)
                .ToListAsync();
        }
    }
} 