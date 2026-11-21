using AnalisisDeOpiniones.Data.Configuracion;
using AnalisisDeOpiniones.Data.Entidades.Csv;
using AnalisisDeOpiniones.Data.Interfaces;
using AnalisisDeOpiniones.service.Interfaces;
using Microsoft.Extensions.Options;

namespace AnalisisDeOpiniones.service.Extractores
{
    public class CsvExtractor : IExtractor
    {
        private readonly IFileReaderRepository<Cliente_csv> _clientesReader;
        private readonly IFileReaderRepository<Producto_csv> _productosReader;
        private readonly IFileReaderRepository<Fuente_datos_csv> _fuentesReader;
        private readonly IStagingWriter _staging;
        private readonly CsvOptions _csv;
        private readonly ILogger<CsvExtractor> _logger;

        public string Fuente => "CSV";

        public CsvExtractor(IFileReaderRepository<Cliente_csv> clientesReader,
                            IFileReaderRepository<Producto_csv> productosReader,
                            IFileReaderRepository<Fuente_datos_csv> fuentesReader,
                            IStagingWriter staging,
                            IOptions<EtlOptions> options,
                            ILogger<CsvExtractor> logger)
        {
            _clientesReader = clientesReader;
            _productosReader = productosReader;
            _fuentesReader = fuentesReader;
            _staging = staging;
            _csv = options.Value.Csv;
            _logger = logger;
        }

        public async Task<int> ExtraerAsync(CancellationToken ct)
        {
            // Rutas completas de los archivos (salen del appsettings.json)
            var rutaClientes = ObtenerRuta(_csv.Clientes);
            var rutaProductos = ObtenerRuta(_csv.Productos);
            var rutaFuentes = ObtenerRuta(_csv.Fuentes);

            // Lectura en paralelo
            var tClientes = _clientesReader.ReadFileAsync(rutaClientes);
            var tProductos = _productosReader.ReadFileAsync(rutaProductos);
            var tFuentes = _fuentesReader.ReadFileAsync(rutaFuentes);

            await Task.WhenAll(tClientes, tProductos, tFuentes);

            var clientes = Validar(await tClientes, c => c.IdCliente > 0 && c.Email.Contains('@'), "clientes");
            var productos = Validar(await tProductos, p => p.IdProducto > 0 && !string.IsNullOrWhiteSpace(p.Nombre), "productos");
            var fuentes = Validar(await tFuentes, f => !string.IsNullOrWhiteSpace(f.IdFuente), "fuentes");

            await Task.WhenAll(
                _staging.GuardarAsync("csv_clientes", clientes, ct),
                _staging.GuardarAsync("csv_productos", productos, ct),
                _staging.GuardarAsync("csv_fuentes", fuentes, ct));

            return clientes.Count + productos.Count + fuentes.Count;
        }


        private string ObtenerRuta(string archivo) =>
            Path.Combine(AppContext.BaseDirectory, _csv.Carpeta, archivo);

        private List<T> Validar<T>(IEnumerable<T> datos, Func<T, bool> regla, string nombre)
        {
            var lista = datos.ToList();
            var validos = lista.Where(regla).ToList();
            var descartados = lista.Count - validos.Count;

            if (descartados > 0)
                _logger.LogWarning("CSV {Nombre}: {Descartados} registros descartados por validación", nombre, descartados);

            _logger.LogInformation("CSV {Nombre}: {Leidos} leídos, {Validos} válidos", nombre, lista.Count, validos.Count);
            return validos;
        }
    }
}