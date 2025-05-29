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
                .AsNoTracking()
                .Where(s => s.OngId == ongId);

            if (status.HasValue)
            {
                query = query.Where(s => s.Status == status.Value);
            }

            return await query
                .OrderByDescending(s => s.DataSolicitacao)
                .Select(s => new SolicitacaoVoluntariado
                {
                    Id = s.Id,
                    UsuarioId = s.UsuarioId,
                    OngId = s.OngId,
                    DataSolicitacao = s.DataSolicitacao,
                    DataAprovacao = s.DataAprovacao,
                    Status = s.Status,
                    Mensagem = s.Mensagem,
                    Usuario = new Usuario 
                    { 
                        Id = s.Usuario.Id,
                        Nome = s.Usuario.Nome,
                        Email = s.Usuario.Email
                    },
                    Ong = new Ong 
                    { 
                        Id = s.Ong.Id,
                        Nome = s.Ong.Nome
                    }
                })
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<int> GetTotalByOngAsync(int ongId, StatusSolicitacao? status)
        {
            var query = _context.SolicitacoesVoluntariado
                .AsNoTracking()
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
            var entry = _context.Entry(solicitacao);
            if (entry.State == EntityState.Detached)
            {
                var existingSolicitacao = await _context.SolicitacoesVoluntariado.FindAsync(solicitacao.Id);
                if (existingSolicitacao != null)
                {
                    _context.Entry(existingSolicitacao).CurrentValues.SetValues(solicitacao);
                }
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.SolicitacoesVoluntariado
                .AsNoTracking()
                .AnyAsync(s => s.Id == id);
        }

        public async Task<bool> ExistePendente(int usuarioId, int ongId)
        {
            return await _context.SolicitacoesVoluntariado
                .AsNoTracking()
                .AnyAsync(s => s.UsuarioId == usuarioId && 
                              s.OngId == ongId && 
                              s.Status == StatusSolicitacao.Pendente);
        }
    }
} 