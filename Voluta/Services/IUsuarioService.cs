using System.Threading.Tasks;
using Voluta.ViewModels;

namespace Voluta.Services
{
    public interface IUsuarioService
    {
        Task<PaginatedViewModel<UsuarioViewModel>> GetUsuariosAsync(int pagina, int tamanhoPagina);
        Task<UsuarioViewModel> GetUsuarioAsync(int id);
        Task<UsuarioViewModel> CreateUsuarioAsync(NovoUsuarioViewModel model);
        Task<UsuarioViewModel> UpdateUsuarioAsync(int id, AtualizarUsuarioViewModel model);
        Task DeleteUsuarioAsync(int id);
        Task<SolicitacaoVoluntariadoViewModel> SolicitarVoluntariadoAsync(int usuarioId, NovaSolicitacaoVoluntariadoViewModel model);
        Task<SolicitacaoVoluntariadoViewModel> GetSolicitacaoAsync(int id);
    }
} 