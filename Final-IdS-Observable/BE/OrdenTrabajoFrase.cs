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

        public OrdenTrabajoFrase(Orden orden, Item trabajo, string frase)
        {
            Orden = orden;
            Trabajo = trabajo;
            Frase = frase;
        }
        public OrdenTrabajoFrase()
        {
            Orden = new Orden();
            Trabajo = new Item();
        }

        public string ObtenerOrden => Orden?.Declaracion ?? string.Empty;
        public string ObtenerTrabajo => Trabajo?.Nombre ?? string.Empty;
    }
}
