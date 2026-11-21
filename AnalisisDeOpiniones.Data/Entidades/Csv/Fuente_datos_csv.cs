using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisDeOpiniones.Data.Entidades.Csv
{
    public class Fuente_datos_csv
    {
        public string IdFuente { get; set; } = string.Empty;
        public string TipoFuente { get; set; } = string.Empty; 
        public DateOnly FechaCarga { get; set; }
    }
}
