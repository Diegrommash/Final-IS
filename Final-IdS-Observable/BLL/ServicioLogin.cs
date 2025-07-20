using BE;
using DAL;

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
            return await _repoUsuario.Loguin(jugador);
        }
    }
}
