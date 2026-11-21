using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalisisDeOpiniones.Data.Entidades.ApiService;

namespace AnalisisDeOpiniones.Data.Interfaces
{
    public interface IComentariosApiClient
    {
        Task<List<Comentario_api>> ObtenerComentariosAsync(CancellationToken ct);
    }
}
