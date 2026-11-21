using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisDeOpiniones.service.Interfaces
{
   public interface IExtractor
    {
        string Fuente { get; }
        Task<int> ExtraerAsync(CancellationToken ct); // devuelve cantidad de registros extraídos
    }
}
