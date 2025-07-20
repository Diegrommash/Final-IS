using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public static class SesionJugador
    {
        private static Jugador? _jugadorActual;

        public static Jugador? JugadorActual => _jugadorActual;

        public static bool EstaLogueado => _jugadorActual != null;

        public static void IniciarSesion(Jugador jugador)
        {
            _jugadorActual = jugador;
        }

        public static void CerrarSesion()
        {
            _jugadorActual = null;
        }
    }
}
