using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalisisDeOpiniones.Data.Interfaces;
using AnalisisDeOpiniones.service.Interfaces;

namespace AnalisisDeOpiniones.service.Extractores
{
    public class ApiExtractor : IExtractor
    {
        private readonly IComentariosApiClient _api;
        private readonly IStagingWriter _staging;
        private readonly ILogger<ApiExtractor> _logger;

        public string Fuente => "API REST";

        public ApiExtractor(IComentariosApiClient api, IStagingWriter staging, ILogger<ApiExtractor> logger)
        {
            _api = api;
            _staging = staging;
            _logger = logger;
        }

        public async Task<int> ExtraerAsync(CancellationToken ct)
        {
            var comentarios = await _api.ObtenerComentariosAsync(ct);
            var validos = comentarios.Where(c => !string.IsNullOrWhiteSpace(c.COMENTARIO)).ToList();

            _logger.LogInformation("API: {Total} comentarios recibidos, {Validos} válidos",
                comentarios.Count, validos.Count);

            await _staging.GuardarAsync("api_comentarios_sociales", validos, ct);
            return validos.Count;
        }
    }
}
