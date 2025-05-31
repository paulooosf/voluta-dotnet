using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voluta.Models.Auth;
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

        [HttpGet]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<ActionResult<PaginatedViewModel<UsuarioViewModel>>> GetUsuarios(
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            var result = await _usuarioService.GetUsuariosAsync(pagina, tamanhoPagina);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<ActionResult<UsuarioViewModel>> GetUsuario(int id)
        {
            var usuario = await _usuarioService.GetUsuarioAsync(id);
            if (usuario == null)
                return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UsuarioViewModel>> CreateUsuario([FromBody] NovoUsuarioViewModel model)
        {
            var usuario = await _usuarioService.CreateUsuarioAsync(model);
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Roles.Usuario))]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] AtualizarUsuarioViewModel model)
        {
            await _usuarioService.UpdateUsuarioAsync(id, model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Roles.Usuario))]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            await _usuarioService.DeleteUsuarioAsync(id);
            return NoContent();
        }

        [HttpPost("SolicitarVoluntariado")]
        [Authorize(Roles = nameof(Roles.Usuario))]
        public async Task<ActionResult<SolicitacaoVoluntariadoViewModel>> SolicitarVoluntariado(
            [FromBody] NovaSolicitacaoVoluntariadoViewModel model,
            [FromQuery] int usuarioId)
        {
            var result = await _usuarioService.SolicitarVoluntariadoAsync(usuarioId, model);
            return CreatedAtAction(nameof(GetSolicitacao), new { id = result.Id }, result);
        }

        [HttpGet("Solicitacao/{id}")]
        [Authorize(Roles = nameof(Roles.Usuario))]
        public async Task<ActionResult<SolicitacaoVoluntariadoViewModel>> GetSolicitacao(int id)
        {
            var solicitacao = await _usuarioService.GetSolicitacaoAsync(id);
            if (solicitacao == null)
                return NotFound();
            return Ok(solicitacao);
        }
    }
} 