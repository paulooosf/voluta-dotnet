using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voluta.Models;
using Voluta.Models.Auth;
using Voluta.Services;
using Voluta.ViewModels;

namespace Voluta.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OngController : ControllerBase
    {
        private readonly IOngService _ongService;

        public OngController(IOngService ongService)
        {
            _ongService = ongService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedViewModel<OngViewModel>>> GetOngs(
            [FromQuery] int pagina = 1, 
            [FromQuery] int tamanhoPagina = 10)
        {
            var result = await _ongService.GetOngsAsync(pagina, tamanhoPagina);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<OngViewModel>> GetOng(int id)
        {
            var ong = await _ongService.GetOngAsync(id);
            return Ok(ong);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<ActionResult<OngViewModel>> CreateOng([FromBody] NovaOngViewModel model)
        {
            var ong = await _ongService.CreateOngAsync(model);
            return CreatedAtAction(nameof(GetOng), new { id = ong.Id }, ong);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> UpdateOng(int id, [FromBody] AtualizarOngViewModel model)
        {
            await _ongService.UpdateOngAsync(id, model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> DeleteOng(int id)
        {
            await _ongService.DeleteOngAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/VoluntariosDisponiveis")]
        [Authorize(Roles = nameof(Roles.Representante))]
        public async Task<ActionResult<PaginatedViewModel<UsuarioViewModel>>> GetVoluntariosDisponiveis(
            int id,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            var result = await _ongService.GetVoluntariosDisponiveisAsync(id, pagina, tamanhoPagina);
            return Ok(result);
        }

        [HttpGet("{id}/voluntarios")]
        [Authorize(Roles = nameof(Roles.Representante))]
        public async Task<ActionResult<IEnumerable<UsuarioViewModel>>> GetVoluntarios(int id)
        {
            var voluntarios = await _ongService.GetVoluntariosAsync(id);
            return Ok(voluntarios);
        }
    }
} 