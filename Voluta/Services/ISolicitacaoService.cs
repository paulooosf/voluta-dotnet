using System.Threading.Tasks;
using Voluta.Models;
using Voluta.ViewModels;

namespace Voluta.Services
{
    public interface ISolicitacaoService
    {
        Task<PaginatedViewModel<SolicitacaoVoluntariadoViewModel>> GetSolicitacoesAsync(
            StatusSolicitacao? status,
            int pagina,
            int tamanhoPagina);
            
        Task<SolicitacaoVoluntariadoViewModel> GetSolicitacaoAsync(int id);
        
        Task<PaginatedViewModel<SolicitacaoVoluntariadoViewModel>> GetSolicitacoesOngAsync(
            int ongId, 
            StatusSolicitacao? status, 
            int pagina, 
            int tamanhoPagina);

        Task<PaginatedViewModel<SolicitacaoVoluntariadoViewModel>> GetSolicitacoesUsuarioAsync(
            int usuarioId,
            StatusSolicitacao? status,
            int pagina,
            int tamanhoPagina);
            
        Task AprovarSolicitacaoAsync(int id);
        Task RejeitarSolicitacaoAsync(int id);
        Task DeleteSolicitacaoAsync(int id);
    }
} 