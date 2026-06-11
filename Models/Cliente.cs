using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zenith_v1.Models
{
    public class Cliente
    {
        public string numero_documento { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string fecha_nacimiento { get; set; }
        public string rut { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }

        public string direccion { get; set; }
        public string numero { get; set; }
        public string comuna { get; set; }
        public string departamento { get; set; }
        public string informacion_adicional { get; set; }
    }
}
