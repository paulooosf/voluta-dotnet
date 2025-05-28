using System;
using System.Linq;
using System.Threading.Tasks;
using Voluta.Models;
using Voluta.Repositories;
using Voluta.ViewModels;

namespace Voluta.Services
{
    public class SolicitacaoService : ISolicitacaoService
    {
        private readonly ISolicitacaoVoluntariadoRepository _solicitacaoRepository;
        private readonly IOngRepository _ongRepository;

        public SolicitacaoService(
            ISolicitacaoVoluntariadoRepository solicitacaoRepository,
            IOngRepository ongRepository)
        {
            _solicitacaoRepository = solicitacaoRepository;
            _ongRepository = ongRepository;
        }

        public async Task<PaginatedViewModel<SolicitacaoVoluntariadoViewModel>> GetSolicitacoesOngAsync(
            int ongId,
            StatusSolicitacao? status,
            int pagina,
            int tamanhoPagina)
        {
            var ong = await _ongRepository.GetByIdAsync(ongId);
            if (ong == null)
                throw new Exception("ONG não encontrada");

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
                throw new Exception("Solicitação não encontrada");

            if (solicitacao.Status != StatusSolicitacao.Pendente)
                throw new Exception("Apenas solicitações pendentes podem ser aprovadas");

            solicitacao.Status = StatusSolicitacao.Aprovada;
            solicitacao.DataAprovacao = DateTime.Now;

            await _solicitacaoRepository.UpdateAsync(solicitacao);
        }

        public async Task RejeitarSolicitacaoAsync(int id)
        {
            var solicitacao = await _solicitacaoRepository.GetByIdAsync(id);
            if (solicitacao == null)
                throw new Exception("Solicitação não encontrada");

            if (solicitacao.Status != StatusSolicitacao.Pendente)
                throw new Exception("Apenas solicitações pendentes podem ser rejeitadas");

            solicitacao.Status = StatusSolicitacao.Rejeitada;

            await _solicitacaoRepository.UpdateAsync(solicitacao);
        }
    }
} 