using System;
using System.Linq;
using System.Threading.Tasks;
using Voluta.Models;
using Voluta.Repositories;
using Voluta.ViewModels;
using Voluta.Exceptions;
using BC = BCrypt.Net.BCrypt;

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

        public async Task<OngViewModel> CreateOngAsync(NovaOngViewModel model)
        {
            var ongExistenteCnpj = await _ongRepository.GetByCnpjAsync(model.Cnpj);
            if (ongExistenteCnpj != null)
                throw new ErroNegocio("Já existe uma ONG cadastrada com este CNPJ");

            var ongExistenteEmail = await _ongRepository.GetByEmailAsync(model.Email);
            if (ongExistenteEmail != null)
                throw new ErroNegocio("Já existe uma ONG cadastrada com este e-mail");

            var ong = new Ong
            {
                Nome = model.Nome,
                Cnpj = model.Cnpj,
                Email = model.Email,
                Telefone = model.Telefone,
                Descricao = model.Descricao,
                AreasAtuacao = model.AreasAtuacao.Select(a => Enum.Parse<AreaAtuacao>(a)).ToArray(),
                Endereco = model.Endereco,
                DataCadastro = DateTime.Now
            };

            ong = await _ongRepository.AddAsync(ong);
            return OngViewModel.FromOng(ong);
        }

        public async Task<OngViewModel> UpdateOngAsync(int id, AtualizarOngViewModel model)
        {
            var ong = await _ongRepository.GetByIdAsync(id);
            if (ong == null)
                throw new ErroNaoEncontrado($"ONG com ID {id} não foi encontrada");

            ong.Nome = model.Nome;
            ong.Telefone = model.Telefone;
            ong.Descricao = model.Descricao;
            ong.AreasAtuacao = model.AreasAtuacao.Select(a => Enum.Parse<AreaAtuacao>(a)).ToArray();
            ong.Endereco = model.Endereco;

            await _ongRepository.UpdateAsync(ong);
            return OngViewModel.FromOng(ong);
        }

        public async Task DeleteOngAsync(int id)
        {
            var ong = await _ongRepository.GetByIdAsync(id);
            if (ong == null)
                throw new ErroNaoEncontrado($"ONG com ID {id} não foi encontrada");

            if (ong.SolicitacoesVoluntariado.Any(s => s.Status == StatusSolicitacao.Pendente))
                throw new ErroNegocio("Não é possível excluir uma ONG com solicitações pendentes");

            await _ongRepository.DeleteAsync(id);
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

        public async Task<IEnumerable<UsuarioViewModel>> GetVoluntariosAsync(int id)
        {
            var ong = await _ongRepository.GetByIdAsync(id);
            if (ong == null)
                throw new ErroNaoEncontrado($"ONG com ID {id} não foi encontrada");

            var voluntarios = await _usuarioRepository.GetVoluntariosByOngAsync(id);
            return voluntarios.Select(u => UsuarioViewModel.FromUsuario(u));
        }
    }
} 