using System.Collections.Generic;
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
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<Ong> GetByIdAsync(int id)
        {
            return await _context.Ongs.FindAsync(id);
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
            _context.Entry(ong).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ong = await _context.Ongs.FindAsync(id);
            if (ong != null)
            {
                _context.Ongs.Remove(ong);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Ongs.AnyAsync(o => o.Id == id);
        }
    }
} 