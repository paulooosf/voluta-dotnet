using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Voluta.Models;
using Voluta.Services;
using Voluta.ViewModels;

namespace Voluta.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitacaoController : ControllerBase
    {
        private readonly ISolicitacaoService _solicitacaoService;

        public SolicitacaoController(ISolicitacaoService solicitacaoService)
        {
            _solicitacaoService = solicitacaoService;
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
    }
} 