using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalisisDeOpiniones.Data.Interfaces;
using AnalisisDeOpiniones.service.Interfaces;

namespace AnalisisDeOpiniones.service.Extractores
{
    public class DatabaseExtractor : IExtractor
    {
        private readonly IOpinionesDbRepository _repositorio;
        private readonly IStagingWriter _staging;
        private readonly ILogger<DatabaseExtractor> _logger;

        public string Fuente => "Base de datos (Oracle)";

        public DatabaseExtractor(IOpinionesDbRepository repositorio, IStagingWriter staging,
                                 ILogger<DatabaseExtractor> logger)
        {
            _repositorio = repositorio;
            _staging = staging;
            _logger = logger;
        }

        public async Task<int> ExtraerAsync(CancellationToken ct)
        {
            var tResenas = _repositorio.ObtenerResenasAsync(ct);
            var tEncuestas = _repositorio.ObtenerEncuestasAsync(ct);

            await Task.WhenAll(tResenas, tEncuestas);

            var resenas = await tResenas;
            var encuestas = await tEncuestas;

            _logger.LogInformation("BD: {Resenas} reseñas web y {Encuestas} encuestas extraídas",
                resenas.Count, encuestas.Count);

            await Task.WhenAll(
                _staging.GuardarAsync("db_resenas_web", resenas, ct),
                _staging.GuardarAsync("db_encuestas", encuestas, ct));

            return resenas.Count + encuestas.Count;
        }
    }
}