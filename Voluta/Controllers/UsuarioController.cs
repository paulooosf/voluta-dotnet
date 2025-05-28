using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Voluta.Services;
using Voluta.ViewModels;

namespace Voluta.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<PaginatedViewModel<UsuarioViewModel>>> GetUsuarios(
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            var result = await _usuarioService.GetUsuariosAsync(pagina, tamanhoPagina);
            return Ok(result);
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioViewModel>> GetUsuario(int id)
        {
            var usuario = await _usuarioService.GetUsuarioAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // POST: api/Usuario/SolicitarVoluntariado
        [HttpPost("SolicitarVoluntariado")]
        public async Task<ActionResult<SolicitacaoVoluntariadoViewModel>> SolicitarVoluntariado(
            [FromBody] NovaSolicitacaoVoluntariadoViewModel model,
            [FromQuery] int usuarioId)
        {
            try
            {
                var result = await _usuarioService.SolicitarVoluntariadoAsync(usuarioId, model);
                return CreatedAtAction(nameof(GetSolicitacao), new { id = result.Id }, result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Usuario/Solicitacao/5
        [HttpGet("Solicitacao/{id}")]
        public async Task<ActionResult<SolicitacaoVoluntariadoViewModel>> GetSolicitacao(int id)
        {
            var solicitacao = await _usuarioService.GetSolicitacaoAsync(id);
            if (solicitacao == null)
            {
                return NotFound();
            }

            return solicitacao;
        }
    }
} 