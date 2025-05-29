using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voluta.Models;
using Voluta.Repositories;
using Voluta.ViewModels;
using Voluta.Exceptions;

namespace Voluta.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IOngRepository _ongRepository;
        private readonly ISolicitacaoVoluntariadoRepository _solicitacaoRepository;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IOngRepository ongRepository,
            ISolicitacaoVoluntariadoRepository solicitacaoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _ongRepository = ongRepository;
            _solicitacaoRepository = solicitacaoRepository;
        }

        public async Task<PaginatedViewModel<UsuarioViewModel>> GetUsuariosAsync(int pagina, int tamanhoPagina)
        {
            var skip = (pagina - 1) * tamanhoPagina;
            var usuarios = await _usuarioRepository.GetAllAsync(skip, tamanhoPagina);
            var total = await _usuarioRepository.GetTotalCountAsync();
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

        public async Task<UsuarioViewModel> GetUsuarioAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                throw new ErroNaoEncontrado($"Usuário com ID {id} não foi encontrado");

            return UsuarioViewModel.FromUsuario(usuario);
        }

        public async Task<SolicitacaoVoluntariadoViewModel> SolicitarVoluntariadoAsync(
            int usuarioId, 
            NovaSolicitacaoVoluntariadoViewModel model)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
                throw new ErroNaoEncontrado($"Usuário com ID {usuarioId} não foi encontrado");

            if (!usuario.Disponivel)
                throw new ErroNegocio("Usuário não está disponível para voluntariado");

            var ong = await _ongRepository.GetByIdAsync(model.OngId);
            if (ong == null)
                throw new ErroNaoEncontrado($"ONG com ID {model.OngId} não foi encontrada");

            var solicitacaoExistente = await _solicitacaoRepository.ExistePendente(usuarioId, model.OngId);
            if (solicitacaoExistente)
                throw new ErroNegocio("Já existe uma solicitação pendente para esta ONG");

            var solicitacao = new SolicitacaoVoluntariado
            {
                UsuarioId = usuarioId,
                OngId = model.OngId,
                Mensagem = model.Mensagem,
                DataSolicitacao = DateTime.Now,
                Status = StatusSolicitacao.Pendente
            };

            solicitacao = await _solicitacaoRepository.AddAsync(solicitacao);
            return SolicitacaoVoluntariadoViewModel.FromSolicitacao(solicitacao);
        }

        public async Task<SolicitacaoVoluntariadoViewModel> GetSolicitacaoAsync(int id)
        {
            var solicitacao = await _solicitacaoRepository.GetByIdAsync(id);
            if (solicitacao == null)
                throw new ErroNaoEncontrado($"Solicitação com ID {id} não foi encontrada");

            return SolicitacaoVoluntariadoViewModel.FromSolicitacao(solicitacao);
        }
    }
} 