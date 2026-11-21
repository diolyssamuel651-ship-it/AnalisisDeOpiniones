using AnalisisDeOpiniones.Data.Configuracion;
using AnalisisDeOpiniones.service.Extractores;
using AnalisisDeOpiniones.service.Interfaces;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace AnalisisDeOpiniones.service
{
    public class Worker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly EtlOptions _options;
        private readonly ILogger<Worker> _logger;

        public Worker(IServiceScopeFactory scopeFactory, IHostApplicationLifetime lifetime,
                      IOptions<EtlOptions> options, ILogger<Worker> logger)
        {
            _scopeFactory = scopeFactory;
            _lifetime = lifetime;
            _options = options.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await EjecutarExtraccionAsync(stoppingToken);

                if (_options.IntervaloMinutos <= 0)
                {
                    _logger.LogInformation("Ejecución única completada. Deteniendo el servicio.");
                    _lifetime.StopApplication();
                    break;
                }

                await Task.Delay(TimeSpan.FromMinutes(_options.IntervaloMinutos), stoppingToken);
            }
        }

        private async Task EjecutarExtraccionAsync(CancellationToken ct)
        {
            _logger.LogInformation("Inicio de la extracción: {Hora} 😣", DateTimeOffset.Now);
            var reloj = Stopwatch.StartNew();

            using var scope = _scopeFactory.CreateScope();
            var extractores = scope.ServiceProvider.GetServices<IExtractor>();

            // Todas las fuentes se extraen en paralelo
            var resultados = await Task.WhenAll(extractores.Select(e => EjecutarExtractorAsync(e, ct)));

            reloj.Stop();
            _logger.LogInformation("Extracción finalizada en {Ms} ms. Total de registros: {Total} Errr mejor",
                reloj.ElapsedMilliseconds, resultados.Sum());
        }

        private async Task<int> EjecutarExtractorAsync(IExtractor extractor, CancellationToken ct)
        {
            var reloj = Stopwatch.StartNew();
            try
            {
                var cantidad = await extractor.ExtraerAsync(ct);
                _logger.LogInformation("[{Fuente}] {Cantidad} registros en {Ms} ms",
                    extractor.Fuente, cantidad, reloj.ElapsedMilliseconds);
                return cantidad;
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Si una fuente falla, las demás continúan
                _logger.LogError(ex, "[{Fuente}] Error durante la extracción🥲 wao papa Dios", extractor.Fuente);
                return 0;
            }
        }
    }
}
