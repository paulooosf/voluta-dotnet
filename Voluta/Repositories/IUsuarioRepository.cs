using System.Collections.Generic;
using System.Threading.Tasks;
using Voluta.Models;

namespace Voluta.Repositories
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync(int skip, int take);
        Task<Usuario> GetByIdAsync(int id);
        Task<int> GetTotalCountAsync();
        Task<Usuario> AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Usuario>> GetDisponiveisByAreasAsync(IEnumerable<AreaAtuacao> areas, int skip, int take);
        Task<int> GetDisponiveisByAreasCountAsync(IEnumerable<AreaAtuacao> areas);
    }
} 