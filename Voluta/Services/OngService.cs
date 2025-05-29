using System;
using System.Linq;
using System.Threading.Tasks;
using Voluta.Models;
using Voluta.Repositories;
using Voluta.ViewModels;
using Voluta.Exceptions;

namespace Voluta.Services
{
    public class OngService : IOngService
    {
        private readonly IOngRepository _ongRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public OngService(
            IOngRepository ongRepository,
            IUsuarioRepository usuarioRepository)
        {
            _ongRepository = ongRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<PaginatedViewModel<OngViewModel>> GetOngsAsync(int pagina, int tamanhoPagina)
        {
            var skip = (pagina - 1) * tamanhoPagina;
            var ongs = await _ongRepository.GetAllAsync(skip, tamanhoPagina);
            var total = await _ongRepository.GetTotalCountAsync();
            var totalPaginas = (int)Math.Ceiling(total / (double)tamanhoPagina);

            return new PaginatedViewModel<OngViewModel>
            {
                Items = ongs.Select(o => OngViewModel.FromOng(o)),
                PageNumber = pagina,
                PageSize = tamanhoPagina,
                TotalPages = totalPaginas,
                TotalItems = total
            };
        }

        public async Task<OngViewModel> GetOngAsync(int id)
        {
            var ong = await _ongRepository.GetByIdAsync(id);
            if (ong == null)
                throw new ErroNaoEncontrado($"ONG com ID {id} não foi encontrada");

            return OngViewModel.FromOng(ong);
        }

        public async Task<PaginatedViewModel<UsuarioViewModel>> GetVoluntariosDisponiveisAsync(
            int ongId, 
            int pagina, 
            int tamanhoPagina)
        {
            var ong = await _ongRepository.GetByIdAsync(ongId);
            if (ong == null)
                throw new ErroNaoEncontrado($"ONG com ID {ongId} não foi encontrada");

            var skip = (pagina - 1) * tamanhoPagina;
            var usuarios = await _usuarioRepository.GetDisponiveisByAreasAsync(ong.AreasAtuacao, skip, tamanhoPagina);
            var total = await _usuarioRepository.GetDisponiveisByAreasCountAsync(ong.AreasAtuacao);
            var totalPaginas = (int)Math.Ceiling(total / (double)tamanhoPagina);

            return new PaginatedViewModel<UsuarioViewModel>
            {
                Items = usuarios.Select(u => UsuarioViewModel.FromUsuario(u)),
                PageNumber = pagina,
                PageSize = tamanhoPagina,
                TotalPages = totalPaginas,
                TotalItems = total
            };
        }
    }
} 