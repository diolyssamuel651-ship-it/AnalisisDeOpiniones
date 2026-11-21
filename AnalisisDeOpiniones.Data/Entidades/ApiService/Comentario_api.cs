using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisDeOpiniones.Data.Entidades.ApiService
{
    public class Comentario_api
    {
        public int ID_COMENTARIO { get; set; }
        public int? ID_CLIENTE { get; set; }
        public int? ID_PRODUCTO { get; set; }
        public int? ID_FUENTE { get; set; }
        public string? RED_SOCIAL { get; set; }
        public DateOnly FECHA { get; set; }
        public string? COMENTARIO { get; set; }
    }
}
