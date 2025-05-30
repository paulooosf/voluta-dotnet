using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voluta.Data;
using Voluta.Models;

namespace Voluta.Repositories
{
    public class OngRepository : IOngRepository
    {
        private readonly VolutaDbContext _context;

        public OngRepository(VolutaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ong>> GetAllAsync(int skip, int take)
        {
            return await _context.Ongs
                .AsNoTracking()
                .OrderBy(o => o.Nome)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<Ong> GetByIdAsync(int id)
        {
            return await _context.Ongs
                .Include(o => o.SolicitacoesVoluntariado
                    .Where(s => s.Status == StatusSolicitacao.Pendente))
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Ong> GetByCnpjAsync(string cnpj)
        {
            return await _context.Ongs
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Cnpj == cnpj);
        }

        public async Task<Ong> GetByEmailAsync(string email)
        {
            return await _context.Ongs
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Email == email);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Ongs.CountAsync();
        }

        public async Task<Ong> AddAsync(Ong ong)
        {
            _context.Ongs.Add(ong);
            await _context.SaveChangesAsync();
            return ong;
        }

        public async Task UpdateAsync(Ong ong)
        {
            var entry = _context.Entry(ong);
            if (entry.State == EntityState.Detached)
            {
                var existingOng = await _context.Ongs.FindAsync(ong.Id);
                if (existingOng != null)
                {
                    _context.Entry(existingOng).CurrentValues.SetValues(ong);
                }
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Ongs
                .Where(o => o.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Ongs
                .AsNoTracking()
                .AnyAsync(o => o.Id == id);
        }
    }
} 