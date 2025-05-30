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
    public class OngController : ControllerBase
    {
        private readonly IOngService _ongService;

        public OngController(IOngService ongService)
        {
            _ongService = ongService;
        }

        // GET: api/Ong
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedViewModel<OngViewModel>>> GetOngs(
            [FromQuery] int pagina = 1, 
            [FromQuery] int tamanhoPagina = 10)
        {
            var result = await _ongService.GetOngsAsync(pagina, tamanhoPagina);
            return Ok(result);
        }

        // GET: api/Ong/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<OngViewModel>> GetOng(int id)
        {
            var ong = await _ongService.GetOngAsync(id);
            return Ok(ong);
        }

        // GET: api/Ong/5/VoluntariosDisponiveis
        [HttpGet("{id}/VoluntariosDisponiveis")]
        public async Task<ActionResult<PaginatedViewModel<UsuarioViewModel>>> GetVoluntariosDisponiveis(
            int id,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            var result = await _ongService.GetVoluntariosDisponiveisAsync(id, pagina, tamanhoPagina);
            return Ok(result);
        }
    }
} 