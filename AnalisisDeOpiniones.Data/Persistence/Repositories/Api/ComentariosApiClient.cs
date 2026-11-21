using System.Net.Http.Json;
using AnalisisDeOpiniones.Data.Configuracion;
using AnalisisDeOpiniones.Data.Entidades.ApiService;
using AnalisisDeOpiniones.Data.Interfaces;
using Microsoft.Extensions.Options;

namespace AnalisisDeOpiniones.Data.Persistence.Repositories.Api
{
    public class ComentariosApiClient : IComentariosApiClient
    {
        private readonly HttpClient _http;
        private readonly string _endpoint;

        public ComentariosApiClient(HttpClient http, IOptions<EtlOptions> options)
        {
            _http = http;
            _endpoint = options.Value.Api.ComentariosEndpoint;
        }

        public async Task<List<Comentario_api>> ObtenerComentariosAsync(CancellationToken ct)
        {
            using var respuesta = await _http.GetAsync(_endpoint, ct);
            respuesta.EnsureSuccessStatusCode();

            return await respuesta.Content.ReadFromJsonAsync<List<Comentario_api>>(cancellationToken: ct)
                   ?? new List<Comentario_api>();
        }
    }
}