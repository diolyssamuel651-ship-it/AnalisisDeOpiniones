using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisDeOpiniones.Data.Entidades.DataBase
{
    public class Encuesta
    {
        public int ID_ENCUESTA { get; set; }
        public int? ID_CLIENTE { get; set; }
        public int? ID_PRODUCTO { get; set; }
        public int? ID_FUENTE { get; set; }
        public DateOnly FECHA { get; set; }
        public string? COMENTARIO { get; set; } = string.Empty;
        public string? CLASIFICACION { get; set; }
        public int? PUNTAJE { get; set; }
        public string? CANAL { get; set; }
    }
}
