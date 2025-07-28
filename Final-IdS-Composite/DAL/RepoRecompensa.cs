using BE;
using BE.Excepciones;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RepoRecompensa
    {
        private readonly Acceso _acceso;
        private readonly RepoItem _repoItem;
        public RepoRecompensa()
        {
            _acceso = new Acceso();
            _repoItem = new RepoItem(_acceso);
        }



        public async Task<int> Agregar(IMision mision, Item recompensa)
        {
            try
            {
                var sql = "SP_AGREGAR_RECOMPENSA";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@MisionId", mision.Id, DbType.Int32),
                    _acceso.CrearParametro("@ItemId", recompensa.Id, DbType.Int32)
                };

                var resultado = await _acceso.EscribirAsync(sql, parametros, CommandType.StoredProcedure);
                return resultado;
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion("Error al agregar recompensa", ex);
            } 
        }

        public async Task<bool> Eliminar(IMision mision, Item recompensa)
        {
            try
            {
                var sql = "SP_ELIMINAR_RECOMPENSA";
                var parametros = new List<IDbDataParameter>
                {
                   _acceso.CrearParametro("@MisionId", mision.Id, DbType.Int32),
                   _acceso.CrearParametro("@ItemId", recompensa.Id, DbType.Int32)
                };

                var resultado = await _acceso.EscribirAsync(sql, parametros, CommandType.StoredProcedure);
                return resultado > 0;
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion("Error al quitar recompensa", ex);
            }
        }

        public async Task<List<Item>> BuscarRecompensasMision(IMision mision)
        {
            try
            {
                var sql = "SP_BUSCAR_RECOMPENSAS";
                var parametros = new List<IDbDataParameter>
                {
                     _acceso.CrearParametro("@MisionId", mision.Id, DbType.Int32)
                };

                var resultado = await _acceso.LeerAsync(sql, parametros, CommandType.StoredProcedure);
                if (resultado == null || resultado.Rows.Count == 0)
                    return new List<Item>();

                return resultado.AsEnumerable()
                                .Select(row => _repoItem.MapearItem(row))
                                .ToList();
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion("Error al buscar recompensas de una mision", ex);
            }
        }

        public async Task<IList<Item>> BuscarItems(IMision mision)
        {
            try
            {
                return await _repoItem.BuscarTodos();
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion("Error al agregar recompensa", ex);
            }
        }

    }
}
