using System.Collections.Generic;
using System.Threading.Tasks;
using Voluta.Models;

namespace Voluta.Repositories
{
    public interface ISolicitacaoVoluntariadoRepository
    {
        Task<IEnumerable<SolicitacaoVoluntariado>> GetAllAsync(StatusSolicitacao? status, int skip, int take);
        Task<int> GetTotalCountAsync(StatusSolicitacao? status);
        Task<IEnumerable<SolicitacaoVoluntariado>> GetByOngAsync(int ongId, StatusSolicitacao? status, int skip, int take);
        Task<int> GetTotalByOngAsync(int ongId, StatusSolicitacao? status);
        Task<IEnumerable<SolicitacaoVoluntariado>> GetByUsuarioAsync(int usuarioId, StatusSolicitacao? status, int skip, int take);
        Task<int> GetTotalByUsuarioAsync(int usuarioId, StatusSolicitacao? status);
        Task<SolicitacaoVoluntariado> GetByIdAsync(int id);
        Task<SolicitacaoVoluntariado> AddAsync(SolicitacaoVoluntariado solicitacao);
        Task UpdateAsync(SolicitacaoVoluntariado solicitacao);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistePendente(int usuarioId, int ongId);
    }
} 