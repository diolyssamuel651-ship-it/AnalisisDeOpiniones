using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisDeOpiniones.Data.Entidades.DataBase
{
    public class Resena_web
    {
        public int? ID_RESENA { get; set; }
        public int? ID_CLIENTE { get; set; }
        public int? ID_PRODUCTO { get; set; }
        public int? ID_FUENTE { get; set; }
        public DateOnly FECHA { get; set; }
        public string? COMENTARIO { get; set; } = string.Empty;
        public int? RATING { get; set; }
    }
}
