using CsvHelper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalisisDeOpiniones.Data.Interfaces;
using AnalisisDeOpiniones.Data.Entidades.Csv;
using CsvHelper.Configuration;


namespace AnalisisDeOpiniones.Data.Persistence.Repositories.Csv
{
    public class CsvFileReaderRepository<TClass> : IFileReaderRepository<TClass> where TClass : class
    {
        private readonly ILogger<CsvFileReaderRepository<TClass>> _logger;

        public CsvFileReaderRepository(ILogger<CsvFileReaderRepository<TClass>> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<TClass>> ReadFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"No se encontró el archivo CSV: {filePath}");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                TrimOptions = TrimOptions.Trim,
                BadDataFound = args =>
                    _logger.LogWarning("Dato mal formado en {Archivo}: {Registro}", filePath, args.RawRecord)
            };

            var registros = new List<TClass>();

            using var reader = new StreamReader(filePath, Encoding.UTF8);
            using var csv = new CsvReader(reader, config);

            await foreach (var registro in csv.GetRecordsAsync<TClass>())
                registros.Add(registro);

            _logger.LogInformation("Archivo leído: {Archivo} ({Cantidad} registros)", filePath, registros.Count);
            return registros;
        }
    }
}
