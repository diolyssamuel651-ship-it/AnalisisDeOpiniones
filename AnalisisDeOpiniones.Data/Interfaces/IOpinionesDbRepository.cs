using AnalisisDeOpiniones.Data.Entidades.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisDeOpiniones.Data.Interfaces
{
    public interface IOpinionesDbRepository
    {
        Task<List<Resena_web>> ObtenerResenasAsync(CancellationToken ct);
        Task<List<Encuesta>> ObtenerEncuestasAsync(CancellationToken ct);
        Task<List<Comentario_social>> ObtenerComentariosSocialesAsync(CancellationToken ct);
    }
}
