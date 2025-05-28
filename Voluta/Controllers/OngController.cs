using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Voluta.Services;
using Voluta.ViewModels;

namespace Voluta.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OngController : ControllerBase
    {
        private readonly IOngService _ongService;

        public OngController(IOngService ongService)
        {
            _ongService = ongService;
        }

        // GET: api/Ong
        [HttpGet]
        public async Task<ActionResult<PaginatedViewModel<OngViewModel>>> GetOngs(
            [FromQuery] int pagina = 1, 
            [FromQuery] int tamanhoPagina = 10)
        {
            var result = await _ongService.GetOngsAsync(pagina, tamanhoPagina);
            return Ok(result);
        }

        // GET: api/Ong/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OngViewModel>> GetOng(int id)
        {
            var ong = await _ongService.GetOngAsync(id);
            if (ong == null)
            {
                return NotFound();
            }

            return ong;
        }

        // GET: api/Ong/VoluntariosDisponiveis/5
        [HttpGet("VoluntariosDisponiveis/{ongId}")]
        public async Task<ActionResult<PaginatedViewModel<UsuarioViewModel>>> GetVoluntariosDisponiveis(
            int ongId,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            try
            {
                var result = await _ongService.GetVoluntariosDisponiveisAsync(ongId, pagina, tamanhoPagina);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
} 