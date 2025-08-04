using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class OrdenTrabajoFrase
    {
        public Orden Orden { get; set; }
        public Item Trabajo { get; set; }
        public string Frase { get; set; }
    }
}
