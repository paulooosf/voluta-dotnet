using System.Collections.Generic;
using System.Threading.Tasks;
using Voluta.ViewModels;

namespace Voluta.Services
{
    public interface IOngService
    {
        Task<PaginatedViewModel<OngViewModel>> GetOngsAsync(int pagina, int tamanhoPagina);
        Task<OngViewModel> GetOngAsync(int id);
        Task<OngViewModel> CreateOngAsync(NovaOngViewModel model);
        Task<OngViewModel> UpdateOngAsync(int id, AtualizarOngViewModel model);
        Task DeleteOngAsync(int id);
        Task<PaginatedViewModel<UsuarioViewModel>> GetVoluntariosDisponiveisAsync(int ongId, int pagina, int tamanhoPagina);
        Task<IEnumerable<UsuarioViewModel>> GetVoluntariosAsync(int id);
    }
} 