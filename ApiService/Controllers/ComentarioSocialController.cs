using AnalisisDeOpiniones.Data.Entidades.DataBase;
using AnalisisDeOpiniones.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComentarioSocialController : ControllerBase
    {
        private readonly IOpinionesDbRepository _repositorio;
        private readonly ILogger<ComentarioSocialController> _logger;

        public ComentarioSocialController(IOpinionesDbRepository repositorio, ILogger<ComentarioSocialController> logger)
        {
            _repositorio = repositorio;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comentario_social>>> Get(CancellationToken ct)
        {
            try
            {
                var comentarios = await _repositorio.ObtenerComentariosSocialesAsync(ct);
                return Ok(comentarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo los comentarios sociales");
                return StatusCode(500, "Error al obtener los comentarios.");
            }
        }
    }
}