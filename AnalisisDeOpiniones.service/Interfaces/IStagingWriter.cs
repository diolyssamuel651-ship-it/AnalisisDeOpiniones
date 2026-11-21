using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisDeOpiniones.service.Interfaces
{
    public interface IStagingWriter
    {
        Task<string> GuardarAsync<T>(string nombre, IEnumerable<T> datos, CancellationToken ct);
    }
}
