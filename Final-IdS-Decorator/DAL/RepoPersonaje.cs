using BE;
using BE.Excepciones;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RepoPersonaje
    {
        private readonly Acceso _acceso;

        public RepoPersonaje(Acceso acceso)
        {
            _acceso = acceso ?? throw new ArgumentNullException(nameof(acceso), "El acceso a datos no puede ser nulo.");
        }

        public async Task<int> GuardarPersonaje(Personaje personaje)
        {
            try
            {
                if (personaje == null) throw new ArgumentNullException(nameof(personaje));

                var sql = "SP_GUARDAR_PERSONAJE";

                var paramNombre = _acceso.CrearParametro("@Nombre", personaje.Nombre, DbType.String);
                var paramId = _acceso.CrearParametro("@NuevoId", 0, DbType.Int32, ParameterDirection.Output);

                var parametros = new List<IDbDataParameter> { paramNombre, paramId };

                await _acceso.EscribirAsync(sql, parametros);

                return Convert.ToInt32(paramId.Value);
            }
            catch (SqlException ex)
            {
                throw new AccesoADatosExcepcion("Error al guardar el personaje", ex);
            }
        }

        public async Task<int> GuardarPersonajeDeJugador(Personaje personaje, Jugador jugador)
        {
            try
            {
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@JugadorId", jugador.Id, DbType.Int32),
                    _acceso.CrearParametro("@PersonajeId", personaje.Id, DbType.Int32),
                 };

                return await _acceso.EscribirAsync("SP_GUARDAR_PERSONAJE_DE_JUGADOR", parametros);
            }
            catch (SqlException ex)
            {
                throw new AccesoADatosExcepcion("Error al guardar el personaje del jugador", ex);
            }
        }

        public async Task<Personaje> BuscarPersonaje(int personajeId)
        {
            try
            {
                var sql = "SP_BUSCAR_PERSONAJE";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@Id", personajeId, DbType.String),
                };

                var tabla = await _acceso.LeerAsync(sql, parametros, CommandType.StoredProcedure);
                if (tabla == null || tabla.Rows.Count == 0)
                {
                    return new Personaje();
                }

                var fila = tabla.Rows[0];
                var personaje = MapearPersonaje(fila);
                return personaje;
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion("Error al buscar personaje", ex);
            }
        }

        public async Task<bool> EliminarPersonaje(int personajeId)
        {
            try
            {
                var sql = "SP_ELIMINAR_PERSONAJE";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@Id", personajeId, DbType.Int32),
                };
                var filasAfectadas = await _acceso.EscribirAsync(sql, parametros, CommandType.StoredProcedure);
                return filasAfectadas > 0;
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion("Error al eliminar personaje", ex);
            }
        }

        public async Task<bool> EliminarItemsPersonaje(int personajeId)
        {
            try
            {
                var sql = "SP_ELIMINAR_RELACIONES_PERSONAJE_ITEM";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@Id", personajeId, DbType.Int32),
                };
                var filasAfectadas = await _acceso.EscribirAsync(sql, parametros, CommandType.StoredProcedure);
                return filasAfectadas > 0;
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion("Error al eliminar los items del personaje", ex);
            }
        }

        public async Task<List<Personaje>> BuscarPersonajesDeJugador(int personajeId)
        {
            try
            {
                var sql = "SP_BUSCAR_PERSONAJES_DE_JUGADOR";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@JugadorId", personajeId, DbType.Int64),
                };

                var tabla = await _acceso.LeerAsync(sql, parametros, CommandType.StoredProcedure);
                if (tabla == null || tabla.Rows.Count == 0)
                {
                    return new List<Personaje>();
                }

                var personajes = new List<Personaje>();
                foreach (DataRow fila in tabla.Rows)
                {
                    var idPersonaje = Convert.ToInt32(fila["PersonajeId"]);
                    var p = await BuscarPersonaje(idPersonaje);
                    personajes.Add(p);
                }


                return personajes;
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion("Error al buscar personaje", ex);
            }
        }

        private Personaje MapearPersonaje(DataRow fila)
        {
            if (fila == null) throw new ArgumentNullException(nameof(fila));
            return new Personaje
            {
                Id = Convert.ToInt32(fila["Id"]),
                Nombre = fila["Nombre"].ToString()
            };

        }
    }
}
