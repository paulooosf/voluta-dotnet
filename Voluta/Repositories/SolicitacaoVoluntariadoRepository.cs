using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voluta.Data;
using Voluta.Models;

namespace Voluta.Repositories
{
    public class SolicitacaoVoluntariadoRepository : ISolicitacaoVoluntariadoRepository
    {
        private readonly VolutaDbContext _context;

        public SolicitacaoVoluntariadoRepository(VolutaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SolicitacaoVoluntariado>> GetByOngAsync(int ongId, StatusSolicitacao? status, int skip, int take)
        {
            var query = _context.SolicitacoesVoluntariado
                .Include(s => s.Usuario)
                .Include(s => s.Ong)
                .Where(s => s.OngId == ongId);

            if (status.HasValue)
            {
                query = query.Where(s => s.Status == status.Value);
            }

            return await query
                .OrderByDescending(s => s.DataSolicitacao)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<int> GetTotalByOngAsync(int ongId, StatusSolicitacao? status)
        {
            var query = _context.SolicitacoesVoluntariado
                .Where(s => s.OngId == ongId);

            if (status.HasValue)
            {
                query = query.Where(s => s.Status == status.Value);
            }

            return await query.CountAsync();
        }

        public async Task<SolicitacaoVoluntariado> GetByIdAsync(int id)
        {
            return await _context.SolicitacoesVoluntariado
                .Include(s => s.Usuario)
                .Include(s => s.Ong)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<SolicitacaoVoluntariado> AddAsync(SolicitacaoVoluntariado solicitacao)
        {
            _context.SolicitacoesVoluntariado.Add(solicitacao);
            await _context.SaveChangesAsync();
            return solicitacao;
        }

        public async Task UpdateAsync(SolicitacaoVoluntariado solicitacao)
        {
            _context.Entry(solicitacao).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.SolicitacoesVoluntariado.AnyAsync(s => s.Id == id);
        }

        public async Task<bool> ExistePendente(int usuarioId, int ongId)
        {
            return await _context.SolicitacoesVoluntariado
                .AnyAsync(s => s.UsuarioId == usuarioId && 
                              s.OngId == ongId && 
                              s.Status == StatusSolicitacao.Pendente);
        }
    }
} 