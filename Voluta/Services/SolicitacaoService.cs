using System;
using System.Linq;
using System.Threading.Tasks;
using Voluta.Models;
using Voluta.Repositories;
using Voluta.ViewModels;
using Voluta.Exceptions;
using System.Collections.Generic;

namespace Voluta.Services
{
    public class SolicitacaoService : ISolicitacaoService
    {
        private readonly ISolicitacaoVoluntariadoRepository _solicitacaoRepository;
        private readonly IOngRepository _ongRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public SolicitacaoService(
            ISolicitacaoVoluntariadoRepository solicitacaoRepository,
            IOngRepository ongRepository,
            IUsuarioRepository usuarioRepository)
        {
            _solicitacaoRepository = solicitacaoRepository;
            _ongRepository = ongRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<PaginatedViewModel<SolicitacaoVoluntariadoViewModel>> GetSolicitacoesAsync(
            StatusSolicitacao? status,
            int pagina,
            int tamanhoPagina)
        {
            var skip = (pagina - 1) * tamanhoPagina;
            var solicitacoes = await _solicitacaoRepository.GetAllAsync(status, skip, tamanhoPagina);
            var total = await _solicitacaoRepository.GetTotalCountAsync(status);
            var totalPaginas = (int)Math.Ceiling(total / (double)tamanhoPagina);

            return new PaginatedViewModel<SolicitacaoVoluntariadoViewModel>
            {
                Items = solicitacoes.Select(s => SolicitacaoVoluntariadoViewModel.FromSolicitacao(s)),
                PageNumber = pagina,
                PageSize = tamanhoPagina,
                TotalPages = totalPaginas,
                TotalItems = total
            };
        }

        public async Task<SolicitacaoVoluntariadoViewModel> GetSolicitacaoAsync(int id)
        {
            var solicitacao = await _solicitacaoRepository.GetByIdAsync(id);
            if (solicitacao == null)
                throw new ErroNaoEncontrado($"Solicitação com ID {id} não foi encontrada");

            return SolicitacaoVoluntariadoViewModel.FromSolicitacao(solicitacao);
        }

        public async Task<PaginatedViewModel<SolicitacaoVoluntariadoViewModel>> GetSolicitacoesOngAsync(
            int ongId,
            StatusSolicitacao? status,
            int pagina,
            int tamanhoPagina)
        {
            var ong = await _ongRepository.GetByIdAsync(ongId);
            if (ong == null)
                throw new ErroNaoEncontrado($"ONG com ID {ongId} não foi encontrada");

            var skip = (pagina - 1) * tamanhoPagina;
            var solicitacoes = await _solicitacaoRepository.GetByOngAsync(ongId, status, skip, tamanhoPagina);
            var total = await _solicitacaoRepository.GetTotalByOngAsync(ongId, status);
            var totalPaginas = (int)Math.Ceiling(total / (double)tamanhoPagina);

            return new PaginatedViewModel<SolicitacaoVoluntariadoViewModel>
            {
                Items = solicitacoes.Select(s => SolicitacaoVoluntariadoViewModel.FromSolicitacao(s)),
                PageNumber = pagina,
                PageSize = tamanhoPagina,
                TotalPages = totalPaginas,
                TotalItems = total
            };
        }

        public async Task AprovarSolicitacaoAsync(int id)
        {
            var solicitacao = await _solicitacaoRepository.GetByIdAsync(id);
            if (solicitacao == null)
                throw new ErroNaoEncontrado($"Solicitação com ID {id} não foi encontrada");

            if (solicitacao.Status != StatusSolicitacao.Pendente)
                throw new ErroNegocio("Apenas solicitações pendentes podem ser aprovadas");

            var usuario = await _usuarioRepository.GetByIdAsync(solicitacao.UsuarioId);
            if (usuario == null)
                throw new ErroNaoEncontrado($"Usuário com ID {solicitacao.UsuarioId} não foi encontrado");

            var ong = await _ongRepository.GetByIdAsync(solicitacao.OngId);
            if (ong == null)
                throw new ErroNaoEncontrado($"ONG com ID {solicitacao.OngId} não foi encontrada");

            if (usuario.OngsVoluntario == null)
                usuario.OngsVoluntario = new List<Ong>();
            if (ong.Voluntarios == null)
                ong.Voluntarios = new List<Usuario>();

            usuario.OngsVoluntario.Add(ong);
            ong.Voluntarios.Add(usuario);

            solicitacao.Status = StatusSolicitacao.Aprovada;
            solicitacao.DataAprovacao = DateTime.Now;

            await _usuarioRepository.UpdateAsync(usuario);
            await _ongRepository.UpdateAsync(ong);
            await _solicitacaoRepository.UpdateAsync(solicitacao);
        }

        public async Task RejeitarSolicitacaoAsync(int id)
        {
            var solicitacao = await _solicitacaoRepository.GetByIdAsync(id);
            if (solicitacao == null)
                throw new ErroNaoEncontrado($"Solicitação com ID {id} não foi encontrada");

            if (solicitacao.Status != StatusSolicitacao.Pendente)
                throw new ErroNegocio("Apenas solicitações pendentes podem ser rejeitadas");

            solicitacao.Status = StatusSolicitacao.Rejeitada;
            await _solicitacaoRepository.UpdateAsync(solicitacao);
        }

        public async Task DeleteSolicitacaoAsync(int id)
        {
            var solicitacao = await _solicitacaoRepository.GetByIdAsync(id);
            if (solicitacao == null)
                throw new ErroNaoEncontrado($"Solicitação com ID {id} não foi encontrada");

            if (solicitacao.Status == StatusSolicitacao.Aprovada)
                throw new ErroNegocio("Não é possível excluir uma solicitação aprovada");

            await _solicitacaoRepository.DeleteAsync(id);
        }

        public async Task<PaginatedViewModel<SolicitacaoVoluntariadoViewModel>> GetSolicitacoesUsuarioAsync(
            int usuarioId,
            StatusSolicitacao? status,
            int pagina,
            int tamanhoPagina)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
                throw new ErroNaoEncontrado($"Usuário com ID {usuarioId} não foi encontrado");

            var skip = (pagina - 1) * tamanhoPagina;
            var solicitacoes = await _solicitacaoRepository.GetByUsuarioAsync(usuarioId, status, skip, tamanhoPagina);
            var total = await _solicitacaoRepository.GetTotalByUsuarioAsync(usuarioId, status);
            var totalPaginas = (int)Math.Ceiling(total / (double)tamanhoPagina);

            return new PaginatedViewModel<SolicitacaoVoluntariadoViewModel>
            {
                Items = solicitacoes.Select(s => SolicitacaoVoluntariadoViewModel.FromSolicitacao(s)),
                PageNumber = pagina,
                PageSize = tamanhoPagina,
                TotalPages = totalPaginas,
                TotalItems = total
            };
        }
    }
} 