using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voluta.Services;
using Voluta.ViewModels;

namespace Voluta.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: api/Usuario
        [HttpGet]
        [AllowAnonymous]
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
            return Ok(usuario);
        }

        // POST: api/Usuario
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UsuarioViewModel>> CreateUsuario([FromBody] NovoUsuarioViewModel model)
        {
            var usuario = await _usuarioService.CreateUsuarioAsync(model);
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] AtualizarUsuarioViewModel model)
        {
            await _usuarioService.UpdateUsuarioAsync(id, model);
            return NoContent();
        }

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            await _usuarioService.DeleteUsuarioAsync(id);
            return NoContent();
        }

        // POST: api/Usuario/SolicitarVoluntariado
        [HttpPost("SolicitarVoluntariado")]
        public async Task<ActionResult<SolicitacaoVoluntariadoViewModel>> SolicitarVoluntariado(
            [FromBody] NovaSolicitacaoVoluntariadoViewModel model,
            [FromQuery] int usuarioId)
        {
            var result = await _usuarioService.SolicitarVoluntariadoAsync(usuarioId, model);
            return CreatedAtAction(nameof(GetSolicitacao), new { id = result.Id }, result);
        }

        // GET: api/Usuario/Solicitacao/5
        [HttpGet("Solicitacao/{id}")]
        public async Task<ActionResult<SolicitacaoVoluntariadoViewModel>> GetSolicitacao(int id)
        {
            var solicitacao = await _usuarioService.GetSolicitacaoAsync(id);
            return Ok(solicitacao);
        }
    }
} 