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

        // GET: api/Solicitacao
        [HttpGet]
        public async Task<ActionResult<PaginatedViewModel<SolicitacaoVoluntariadoViewModel>>> GetSolicitacoes(
            [FromQuery] StatusSolicitacao? status = null,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            var result = await _solicitacaoService.GetSolicitacoesAsync(status, pagina, tamanhoPagina);
            return Ok(result);
        }

        // GET: api/Solicitacao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SolicitacaoVoluntariadoViewModel>> GetSolicitacao(int id)
        {
            var solicitacao = await _solicitacaoService.GetSolicitacaoAsync(id);
            return Ok(solicitacao);
        }

        // GET: api/Solicitacao/Ong/5
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

        // GET: api/Solicitacao/Usuario/5
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

        // PUT: api/Solicitacao/5/Aprovar
        [HttpPut("{id}/Aprovar")]
        public async Task<IActionResult> AprovarSolicitacao(int id)
        {
            await _solicitacaoService.AprovarSolicitacaoAsync(id);
            return NoContent();
        }

        // PUT: api/Solicitacao/5/Rejeitar
        [HttpPut("{id}/Rejeitar")]
        public async Task<IActionResult> RejeitarSolicitacao(int id)
        {
            await _solicitacaoService.RejeitarSolicitacaoAsync(id);
            return NoContent();
        }

        // DELETE: api/Solicitacao/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitacao(int id)
        {
            await _solicitacaoService.DeleteSolicitacaoAsync(id);
            return NoContent();
        }
    }
} 