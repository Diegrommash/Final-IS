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
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@Nombre", personaje.Nombre, DbType.String),
                };

                var paramId = new SqlParameter("@NuevoId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                parametros.Add(paramId);

                await _acceso.EscribirAsync(sql, parametros);
                int idGenerado = (int)paramId.Value;
                return idGenerado;
            }
            catch (SqlException ex)
            {
                throw new AccesoADatosExcepcion("Error al guarda el personaje", ex);
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
