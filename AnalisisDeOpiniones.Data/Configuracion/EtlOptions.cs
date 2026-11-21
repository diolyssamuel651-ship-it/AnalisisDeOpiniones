using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisDeOpiniones.Data.Configuracion
{
    public class EtlOptions
    {
        public int IntervaloMinutos { get; set; } = 0;
        public CsvOptions Csv { get; set; } = new();
        public ApiOptions Api { get; set; } = new();
        public StagingOptions Staging { get; set; } = new();
    }
    public class CsvOptions
    {
        public string Carpeta { get; set; } = "Archivos";
        public string Clientes { get; set; } = "clients.csv";
        public string Productos { get; set; } = "products.csv";
        public string Fuentes { get; set; } = "fuente_datos.csv";
    }
    public class ApiOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ComentariosEndpoint { get; set; } = "api/ComentarioSocial";
        public int TimeoutSegundos { get; set; } = 30;
    }
    public class StagingOptions
    {
        public string Carpeta { get; set; } = "Staging";
    }
}
