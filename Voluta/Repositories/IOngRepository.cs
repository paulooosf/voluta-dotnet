using System.Collections.Generic;
using System.Threading.Tasks;
using Voluta.Models;

namespace Voluta.Repositories
{
    public interface IOngRepository
    {
        Task<IEnumerable<Ong>> GetAllAsync(int skip, int take);
        Task<Ong> GetByIdAsync(int id);
        Task<Ong> GetByCnpjAsync(string cnpj);
        Task<Ong> GetByEmailAsync(string email);
        Task<int> GetTotalCountAsync();
        Task<Ong> AddAsync(Ong ong);
        Task UpdateAsync(Ong ong);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 