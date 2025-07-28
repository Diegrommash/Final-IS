using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BE;
using BE.Enums;
using BE.Excepciones;

namespace DAL
{
    public class RepoItem
    {
        private readonly Acceso _acceso;

        public RepoItem(Acceso acceso)
        {
            _acceso = acceso ?? throw new ArgumentNullException(nameof(acceso), "El acceso a datos no puede ser nulo.");
        }

        public async Task<IList<Item>> BuscarTodos()
        {
            try
            {
                var sql = "SP_BUSCAR_TODOS_ITEM";
                var tabla = await _acceso.LeerAsync(sql, null, CommandType.StoredProcedure);
                if (tabla == null || tabla.Rows.Count == 0)
                {
                    return new List<Item>();
                }

                var items = new List<Item>();
                foreach (DataRow fila in tabla.Rows)
                {
                    items.Add(MapearItem(fila));
                }
                return items;
            }
            catch (AccesoADatosExcepcion ex)
            {

                throw new RepositorioExcepcion("Error al buscar los items", ex);
            }
           
        }

        public async Task<int> Guardar(int personajeId, int itemId, int orden)
        {
            try
            {
                var sql = "SP_GUARDAR_PERSONAJE_ITEM";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@PersonajeId", personajeId, DbType.Int64),
                     _acceso.CrearParametro("@ItemId", itemId, DbType.Int64),
                      _acceso.CrearParametro("@Orden", orden, DbType.Int64)
                };
                var resultado = await _acceso.EscribirAsync(sql, parametros, CommandType.StoredProcedure);
                return resultado;
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion("Error al guardar un item", ex);
            }

        }

        public async Task<IList<Item>> BuscarItemsDePersonajeOrdenados(int personajeId)
        {
            try
            {
                var sql = "SP_BUSCAR_ITEMS_DE_PERSONAJE_ORDENADOS";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@PersonajeId", personajeId, DbType.Int64)
                };

                var tabla = await _acceso.LeerAsync(sql, parametros);
                if (tabla == null || tabla.Rows.Count == 0)
                {
                    return new List<Item>();
                }

                var items = new List<Item>();
                foreach (DataRow fila in tabla.Rows)
                {
                    items.Add(MapearItem(fila));
                }
                return items;
            }
            catch (AccesoADatosExcepcion ex)
            {

                throw new RepositorioExcepcion($"Error al buscar los items del producto con id: {personajeId}", ex);
            }         
        }


        public Item MapearItem(DataRow fila)
        {
            return new Item
            {
                Id = Convert.ToInt32(fila["Id"]),
                Nombre = fila["Nombre"].ToString() ?? string.Empty,
                Defensa = Convert.ToInt32(fila["Defensa"]),
                Poder = Convert.ToInt32(fila["Poder"]),
                AtributoPpl = (StatEnum)Convert.ToInt32(fila["EstadisticaId"]),
                Tipo = (TipoDecoradorEnum)Convert.ToInt32(fila["TipoItemId"])
            };
        }
    }
}
