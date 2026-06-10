using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zenith_v1.Models
{
    internal class Producto
    {
            public int id { get; set; }
            public string nombre { get; set; }
            public string descripcion { get; set; }
            public int precio { get; set; }
            public int stock { get; set; }
            public string marca { get; set; }
            public string imagen { get; set; }
            public bool destacado { get; set; }
            public bool oferta { get; set; }
            public bool super_oferta { get; set; }
    }
}
