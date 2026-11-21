using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisDeOpiniones.Data.Entidades.DataBase
{
    public class Comentario_social
    {
        public int ID_COMENTARIO { get; set; }
        public int? ID_CLIENTE { get; set; }
        public int? ID_PRODUCTO { get; set; }
        public int? ID_FUENTE { get; set; }
        public string? RED_SOCIAL { get; set; } =string.Empty;
        public DateOnly FECHA { get; set; }
        public string? COMENTARIO { get; set; } = string.Empty;

    }
}
