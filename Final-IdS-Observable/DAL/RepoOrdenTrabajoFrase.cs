using BE.Excepciones;
using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RepoOrdenTrabajoFrase
    {
        private readonly Acceso _acceso;

        public RepoOrdenTrabajoFrase()
        {
            _acceso = new Acceso();
        }

        public async Task<int> Agregar(OrdenTrabajoFrase otf)
        {
            try
            {
                var sql = "SP_INSERTAR_ORDEN_RELACION";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@OrdenId", otf.Orden.Id, DbType.Int32),
                    _acceso.CrearParametro("@TipoTrabajoId", otf.Trabajo.Id, DbType.Int32),
                    _acceso.CrearParametro("@Frase", otf.Frase, DbType.String)
                };

                var id = await _acceso.EscribirAsync(sql, parametros);
                return id;
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion(ex.Message, ex);
            }
        }

        public async Task<bool> Modificar(OrdenTrabajoFrase otf)
        {
            try
            {
                var sql = "SP_MODIFICAR_ORDEN_RELACION";
                var parametros = new List<IDbDataParameter>
                {
                     _acceso.CrearParametro("@OrdenId", otf.Orden.Id, DbType.Int32),
                    _acceso.CrearParametro("@TipoTrabajoId", otf.Trabajo.Id, DbType.Int32),
                    _acceso.CrearParametro("@Frase", otf.Frase, DbType.String)
                };

                var resultado = await _acceso.EscribirAsync(sql, parametros);
                return resultado > 0;
            }
            catch (SqlException ex)
            {
                throw new RepositorioExcepcion("La relacion no se pudo modificar", ex);
            }
        }

        public async Task<bool> Eliminar(OrdenTrabajoFrase otf)
        {
            try
            {
                var sql = "SP_ELIMINAR_ORDEN_RELACION";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@OrdenId", otf.Orden.Id, DbType.Int32),
                    _acceso.CrearParametro("@TipoTrabajoId", otf.Trabajo.Id, DbType.Int32),
                };

                var resultado = await _acceso.EscribirAsync(sql, parametros);
                return resultado > 0;
            }
            catch (SqlException ex)
            {
                throw new RepositorioExcepcion("La orden no se pudo eliminar", ex);
            }
        }

        public async Task<OrdenTrabajoFrase> Buscar(int ordenId)
        {
            try
            {
                var sql = "SP_BUSCAR_RELACIONES_POR_ORDEN";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@OrdenId", ordenId, DbType.Int64)
                };

                var tabla = await _acceso.LeerAsync(sql, parametros);
                if (tabla == null || tabla.Rows.Count == 0) return new OrdenTrabajoFrase();

                var fila = tabla.Rows[0];
                var orden = MapearOrdenTrabajoFrase(fila);
                return orden;
            }
            catch (SqlException ex)
            {
                throw new RepositorioExcepcion("Error al buscar la relacion", ex);
            }
        }

        public async Task<List<OrdenTrabajoFrase>> Listar()
        {
            try
            {
                var sql = "SP_BUSCAR_TODAS_ORDENES_RELACION";
                var tabla = await _acceso.LeerAsync(sql);
                if (tabla == null || tabla.Rows.Count == 0) return new List<OrdenTrabajoFrase>();

                var otfs = new List<OrdenTrabajoFrase>();
                foreach (DataRow fila in tabla.Rows)
                {
                    var orden = MapearOrdenTrabajoFrase(fila);
                    otfs.Add(orden);
                }

                return otfs;
            }
            catch (SqlException ex)
            {
                throw new RepositorioExcepcion("Error al buscar las relaciones", ex);
            }
        }

        private OrdenTrabajoFrase MapearOrdenTrabajoFrase(DataRow fila)
        {
            return new OrdenTrabajoFrase
            {
                Orden = new Orden
                {
                    Id = Convert.ToInt32(fila["OrdenId"]),
                    Declaracion = fila["Orden"].ToString() ?? string.Empty
                },
                Trabajo = new Item
                {
                    Id = Convert.ToInt32(fila["TipoTrabajoId"]),
                    Nombre = fila["TipoTrabajo"].ToString() ?? string.Empty
                },
                Frase = fila["Frase"].ToString() ?? string.Empty
            };
        }

        public async Task<bool> ExisteRelacion(int ordenId, int tipoTrabajoId)
        {
            var sql = "SP_EXISTE_ORDEN_RELACION";
            var parametros = new List<IDbDataParameter>
            {
                _acceso.CrearParametro("@OrdenId", ordenId, DbType.Int32),
                _acceso.CrearParametro("@TipoTrabajoId", tipoTrabajoId, DbType.Int32)
            };

            var tabla = await _acceso.LeerAsync(sql, parametros);
            return tabla != null && tabla.Rows.Count > 0;
        }


    }
}
