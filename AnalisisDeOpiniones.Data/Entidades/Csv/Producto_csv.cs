
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using CsvHelper.Configuration.Attributes;


namespace AnalisisDeOpiniones.Data.Entidades.Csv
{
    public class Producto_csv
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoría { get; set; } = string.Empty;
    }
}