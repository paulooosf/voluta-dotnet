using System.Threading.Tasks;
using Voluta.Models;
using Voluta.ViewModels;

namespace Voluta.Services
{
    public interface ISolicitacaoService
    {
        Task<PaginatedViewModel<SolicitacaoVoluntariadoViewModel>> GetSolicitacoesOngAsync(
            int ongId, 
            StatusSolicitacao? status, 
            int pagina, 
            int tamanhoPagina);
            
        Task AprovarSolicitacaoAsync(int id);
        Task RejeitarSolicitacaoAsync(int id);
    }
} 