using BE;
using BE.Excepciones;
using System.Data;

namespace DAL
{
    public class RepoUsuario
    {
        private readonly Acceso _acceso;

        public RepoUsuario()
        {
            _acceso = new Acceso();
        }

        public async Task<Loguin> Loguin(Jugador jugador)
        {
            try
            {
                var sql = "SP_LOGIN_JUGADOR";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@Nombre", jugador.Nombre, DbType.String),
                    _acceso.CrearParametro("@Contraseña", jugador.Contraseña, DbType.String)
                };

                var tabla = await _acceso.LeerAsync(sql, parametros, CommandType.StoredProcedure);
                if (tabla == null || tabla.Rows.Count == 0)
                {
                    return new Loguin
                    {
                        Jugador = null,
                        Mensaje = "Usuario o contraseña incorrectos."
                    };
                }
                var fila = tabla.Rows[0];
                return new Loguin
                {
                    Jugador = (MapearJugador(fila)),
                    Mensaje = "Inicio de sesión exitoso."
                };
            }
            catch (AccesoADatosExcepcion ex)
            {

                throw new RepositorioExcepcion("Error al intentar loguear", ex);
            }
           
        }

        private Jugador MapearJugador(DataRow fila)
        {
            return new Jugador
            {
                Id = Convert.ToInt32(fila["Id"]),
                Nombre = fila["Nombre"].ToString()
            };
        }

    }
}
