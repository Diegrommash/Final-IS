using BE;
using DAL;
using Servicios;

namespace BLL
{
    public class ServicioLogin
    {
        private readonly RepoUsuario _repoUsuario;
        public ServicioLogin()
        {
            _repoUsuario = new RepoUsuario();
        }

        public async Task<Loguin> Loguin(Jugador jugador)
        {

            if (string.IsNullOrEmpty(jugador.Nombre) || string.IsNullOrEmpty(jugador.Contraseña))
            {
                return new Loguin
                {
                    Jugador = null,
                    Mensaje = "Nombre y contraseña son obligatorios."
                };
            }
            var resultado = await _repoUsuario.Loguin(jugador);
            if (resultado.Jugador != null)
            {
                SesionJugador.IniciarSesion(resultado.Jugador);
                return resultado;
            }
            else
            {
                return new Loguin
                {
                    Jugador = null,
                    Mensaje = "Usuario o contraseña incorrectos."
                };
            } 
        }
    }
}
