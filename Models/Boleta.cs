using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zenith_v1.Models
{
    public class Boleta
    {
        public int Numero { get; set; }
        public string Cliente { get; set; }
        public string Rut { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }

        public List<BoletaDetalle> Detalles { get; set; }
    }
}
