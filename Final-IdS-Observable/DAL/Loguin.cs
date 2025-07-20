using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Loguin
    {
        public Jugador? Jugador { get; set; }
        public string Mensaje { get; set; }

        public Loguin(Jugador? jugador, string mensaje)
        {
            Jugador = jugador;
            Mensaje = mensaje;
        }

        public Loguin()
        {
            
        }
    }
}
