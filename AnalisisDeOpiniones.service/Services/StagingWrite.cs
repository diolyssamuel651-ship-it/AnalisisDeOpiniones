using System.Text.Encodings.Web;
using System.Text.Json;
using AnalisisDeOpiniones.Data.Configuracion;
using AnalisisDeOpiniones.service.Interfaces;
using Microsoft.Extensions.Options;

namespace AnalisisDeOpiniones.service.Services
{
    public class StagingWriter : IStagingWriter
    {
        private static readonly JsonSerializerOptions _json = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private readonly string _carpeta;
        private readonly ILogger<StagingWriter> _logger;

        public StagingWriter(IOptions<EtlOptions> options, ILogger<StagingWriter> logger)
        {
            _carpeta = Path.Combine(AppContext.BaseDirectory, options.Value.Staging.Carpeta);
            _logger = logger;
        }

        public async Task<string> GuardarAsync<T>(string nombre, IEnumerable<T> datos, CancellationToken ct)
        {
            Directory.CreateDirectory(_carpeta);
            var ruta = Path.Combine(_carpeta, $"{nombre}_{DateTime.Now:yyyyMMdd_HHmmss}.json");

            await using var archivo = File.Create(ruta);
            await JsonSerializer.SerializeAsync(archivo, datos, _json, ct);

            _logger.LogInformation("Staging guardado: {Ruta}", ruta);
            return ruta;
        }
    }
}