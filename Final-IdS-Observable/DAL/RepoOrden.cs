using BE;
using BE.Excepciones;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RepoOrden
    {
        private readonly Acceso _acceso;

        public RepoOrden()
        {
            _acceso = new Acceso();
        }

        public async Task<int> Agregar(Orden orden)
        {
            try
            {
                var sql = "SP_AGREGAR_ORDEN";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@Declaracion", orden.Declaracion, DbType.String)
                };

                var id = await _acceso.EscribirAsync(sql, parametros);
                return id;
            }
            catch (SqlException ex)
            {
                throw new RepositorioExcepcion ("La orden no se pudo agregar", ex);
            }
        }

        public async Task<bool> Modificar(Orden orden)
        {
            try
            {
                var sql = "SP_MODIFICAR_ORDEN";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@Id", orden.Id, DbType.Int64),
                    _acceso.CrearParametro("@Declaracion", orden.Declaracion, DbType.String)
                };

                var resultado = await _acceso.EscribirAsync(sql, parametros);
                return resultado > 0;
            }
            catch (SqlException ex)
            {
                throw new RepositorioExcepcion("La orden no se pudo modificar", ex);
            }
        }

        public async Task<bool> Eliminar(Orden orden)
        {
            try
            {
                var sql = "SP_ELIMINAR_ORDEN";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@Id", orden.Id, DbType.Int64)
                };

                var resultado = await _acceso.EscribirAsync(sql, parametros);
                return resultado > 0;
            }
            catch (SqlException ex)
            {
                throw new RepositorioExcepcion("La orden no se pudo eliminar", ex);
            }
        }

        public async Task<Orden> Buscar(int ordenId)
        {
            try
            {
                var sql = "SP_BUSCAR_ORDEN";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@Id", ordenId, DbType.Int64)
                };

                var tabla = await _acceso.LeerAsync(sql, parametros);
                if (tabla == null || tabla.Rows.Count == 0) return new Orden();

                var fila = tabla.Rows[0];
                var orden = MapearOrden(fila);
                return orden;
            }
            catch (SqlException ex)
            {
                throw new RepositorioExcepcion("Error al buscar la orden", ex);
            }
        }

        public async Task<List<Orden>> Listar()
        {
            try
            {
                var sql = "SP_LISTAR_ORDENES";
                var tabla = await _acceso.LeerAsync(sql);
                if (tabla == null || tabla.Rows.Count == 0) return  new List<Orden>();

                var ordenes = new List<Orden>();
                foreach (DataRow fila in tabla.Rows)
                {
                    var orden = MapearOrden(fila);
                    ordenes.Add(orden);
                }

                return ordenes;
            }
            catch (SqlException ex)
            {
                throw new RepositorioExcepcion("Error al buscar las ordenes", ex);
            }
        }

        private Orden MapearOrden(DataRow fila)
        {
            return new Orden
            {
                Id = Convert.ToInt32(fila["Id"]),
                Declaracion = fila["Declaracion"].ToString() ?? string.Empty
            };
        }
    }
}
