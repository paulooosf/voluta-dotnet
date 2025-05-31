using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voluta.Models;
using Voluta.Services;
using Voluta.ViewModels;
using Voluta.Models.Auth;

namespace Voluta.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = nameof(Roles.Representante))]
    public class SolicitacaoController : ControllerBase
    {
        private readonly ISolicitacaoService _solicitacaoService;

        public SolicitacaoController(ISolicitacaoService solicitacaoService)
        {
            _solicitacaoService = solicitacaoService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedViewModel<SolicitacaoVoluntariadoViewModel>>> GetSolicitacoes(
            [FromQuery] StatusSolicitacao? status = null,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            var result = await _solicitacaoService.GetSolicitacoesAsync(status, pagina, tamanhoPagina);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SolicitacaoVoluntariadoViewModel>> GetSolicitacao(int id)
        {
            var solicitacao = await _solicitacaoService.GetSolicitacaoAsync(id);
            return Ok(solicitacao);
        }

        [HttpGet("Ong/{ongId}")]
        public async Task<ActionResult<PaginatedViewModel<SolicitacaoVoluntariadoViewModel>>> GetSolicitacoesOng(
            int ongId,
            [FromQuery] StatusSolicitacao? status = null,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            var result = await _solicitacaoService.GetSolicitacoesOngAsync(ongId, status, pagina, tamanhoPagina);
            return Ok(result);
        }

        [HttpGet("Usuario/{usuarioId}")]
        public async Task<ActionResult<PaginatedViewModel<SolicitacaoVoluntariadoViewModel>>> GetSolicitacoesUsuario(
            int usuarioId,
            [FromQuery] StatusSolicitacao? status = null,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            var result = await _solicitacaoService.GetSolicitacoesUsuarioAsync(usuarioId, status, pagina, tamanhoPagina);
            return Ok(result);
        }

        [HttpPut("{id}/Aprovar")]
        public async Task<IActionResult> AprovarSolicitacao(int id)
        {
            await _solicitacaoService.AprovarSolicitacaoAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/Rejeitar")]
        public async Task<IActionResult> RejeitarSolicitacao(int id)
        {
            await _solicitacaoService.RejeitarSolicitacaoAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitacao(int id)
        {
            await _solicitacaoService.DeleteSolicitacaoAsync(id);
            return NoContent();
        }
    }
} 