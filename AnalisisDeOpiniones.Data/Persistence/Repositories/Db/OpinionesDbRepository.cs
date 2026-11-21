using System.Data.Common;
using AnalisisDeOpiniones.Data.Entidades.DataBase;
using AnalisisDeOpiniones.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace AnalisisDeOpiniones.Data.Persistence.Repositories.Db
{
    public class OpinionesDbRepository : IOpinionesDbRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<OpinionesDbRepository> _logger;

        public OpinionesDbRepository(IConfiguration configuration, ILogger<OpinionesDbRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("AnalisisVentas")
                ?? throw new InvalidOperationException("Falta la cadena de conexión 'AnalisisVentas'.");
            _logger = logger;
        }

        public Task<List<Resena_web>> ObtenerResenasAsync(CancellationToken ct) =>
            ConsultarAsync(
                "SELECT ID_RESENA, ID_CLIENTE, ID_PRODUCTO, ID_FUENTE, FECHA, COMENTARIO, RATING FROM RESENA_WEB",
                r => new Resena_web
                {
                    ID_RESENA = Entero(r, "ID_RESENA"),
                    ID_CLIENTE = EnteroNulo(r, "ID_CLIENTE"),
                    ID_PRODUCTO = EnteroNulo(r, "ID_PRODUCTO"),
                    ID_FUENTE = EnteroNulo(r, "ID_FUENTE"),
                    FECHA = Fecha(r, "FECHA"),
                    COMENTARIO = Texto(r, "COMENTARIO"),
                    RATING = EnteroNulo(r, "RATING")
                }, ct);

        public Task<List<Encuesta>> ObtenerEncuestasAsync(CancellationToken ct) =>
            ConsultarAsync(
                "SELECT ID_ENCUESTA, ID_CLIENTE, ID_PRODUCTO, ID_FUENTE, FECHA, COMENTARIO, CLASIFICACION, PUNTAJE, CANAL FROM ENCUESTA",
                r => new Encuesta
                {
                    ID_ENCUESTA = Entero(r, "ID_ENCUESTA"),
                    ID_CLIENTE = EnteroNulo(r, "ID_CLIENTE"),
                    ID_PRODUCTO = EnteroNulo(r, "ID_PRODUCTO"),
                    ID_FUENTE = EnteroNulo(r, "ID_FUENTE"),
                    FECHA = Fecha(r, "FECHA"),
                    COMENTARIO = Texto(r, "COMENTARIO"),
                    CLASIFICACION = Texto(r, "CLASIFICACION"),
                    PUNTAJE = EnteroNulo(r, "PUNTAJE"),
                    CANAL = Texto(r, "CANAL")
                }, ct);

        public Task<List<Comentario_social>> ObtenerComentariosSocialesAsync(CancellationToken ct) =>
            ConsultarAsync(
                "SELECT ID_COMENTARIO, ID_CLIENTE, ID_PRODUCTO, ID_FUENTE, RED_SOCIAL, FECHA, COMENTARIO FROM COMENTARIO_SOCIAL",
                r => new Comentario_social
                {
                    ID_COMENTARIO = Entero(r, "ID_COMENTARIO"),
                    ID_CLIENTE = EnteroNulo(r, "ID_CLIENTE"),
                    ID_PRODUCTO = EnteroNulo(r, "ID_PRODUCTO"),
                    ID_FUENTE = EnteroNulo(r, "ID_FUENTE"),
                    RED_SOCIAL = Texto(r, "RED_SOCIAL"),
                    FECHA = Fecha(r, "FECHA"),
                    COMENTARIO = Texto(r, "COMENTARIO")
                }, ct);

        private async Task<List<T>> ConsultarAsync<T>(string sql, Func<DbDataReader, T> mapear, CancellationToken ct)
        {
            var lista = new List<T>();

            await using var conexion = new OracleConnection(_connectionString);
            await conexion.OpenAsync(ct);

            await using var comando = new OracleCommand(sql, conexion);
            await using var reader = await comando.ExecuteReaderAsync(ct);

            while (await reader.ReadAsync(ct))
                lista.Add(mapear(reader));

            _logger.LogDebug("Consulta ejecutada: {Sql} -> {Filas} filas", sql, lista.Count);
            return lista;
        }

        private static int Entero(DbDataReader r, string col) => Convert.ToInt32(r[col]);

        private static int? EnteroNulo(DbDataReader r, string col) =>
            r[col] == DBNull.Value ? null : Convert.ToInt32(r[col]);

        private static string? Texto(DbDataReader r, string col) =>
            r[col] == DBNull.Value ? null : r[col].ToString();

        private static DateOnly Fecha(DbDataReader r, string col) =>
            r[col] is DateTime d ? DateOnly.FromDateTime(d) : default;
    }
}