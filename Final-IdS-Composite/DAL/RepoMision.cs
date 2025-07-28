using BE;
using BE.Excepciones;
using System.Data;

namespace DAL
{
    public class RepoMision
    {
        private readonly Acceso _acceso;

        public RepoMision()
        {
            _acceso = new Acceso();
        }

        //Agregar misión
        public async Task<int> Agregar(IMision mision)
        {
            try
            {
                var sql = "SP_AGREGAR_MISION";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@Nombre", mision.Nombre, DbType.String),
                    _acceso.CrearParametro("@Descripcion", mision.Descripcion, DbType.String),
                    _acceso.CrearParametro("@Dificultad", mision.Dificultad, DbType.Int32),
                    _acceso.CrearParametro("@EsCompuesta", mision.EsCompuesta, DbType.Boolean)
                };

                var tabla = await _acceso.LeerAsync(sql, parametros);
                return tabla.Rows.Count > 0 ? Convert.ToInt32(tabla.Rows[0]["NuevoId"]) : 0;
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion("Error al agregar misión", ex);
            }
        }

        //Modificar misión
        public async Task<bool> Modificar(IMision mision)
        {
            try
            {
                var sql = "SP_MODIFICAR_MISION";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@Id", mision.Id, DbType.Int32),
                    _acceso.CrearParametro("@Nombre", mision.Nombre, DbType.String),
                    _acceso.CrearParametro("@Descripcion", mision.Descripcion, DbType.String),
                    _acceso.CrearParametro("@Dificultad", mision.Dificultad, DbType.Int32),
                    _acceso.CrearParametro("@EsCompuesta", mision.EsCompuesta, DbType.Boolean)
                };
                var filas = await _acceso.EscribirAsync(sql, parametros);
                return filas > 0;
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion("Error al modificar misión", ex);
            }
        }

        //Eliminar misión
        public async Task<bool> Eliminar(IMision mision)
        {
            try
            {
                var sql = "SP_ELIMINAR_MISION";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@Id", mision.Id, DbType.Int32)
                };
                var filas = await _acceso.EscribirAsync(sql, parametros);
                return filas > 0;
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion("Error al eliminar misión", ex);
            }
        }

        //Asignar misión hija
        public async Task<bool> Asignar(IMision padre, IMision hija)
        {
            try
            {
                var sql = "SP_ASIGNAR_MISION";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@MisionPadreId", padre.Id, DbType.Int32),
                    _acceso.CrearParametro("@MisionHijaId", hija.Id, DbType.Int32)
                };
                var filas = await _acceso.EscribirAsync(sql, parametros);
                return filas > 0;
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion("Error al asignar misión", ex);
            }
        }

        //Quitar misión hija
        public async Task<bool> Quitar(IMision padre, IMision hija)
        {
            try
            {
                var sql = "SP_QUITAR_MISION"; // 🔹 corregido
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@MisionPadreId", padre.Id, DbType.Int32),
                    _acceso.CrearParametro("@MisionHijaId", hija.Id, DbType.Int32)
                };
                var filas = await _acceso.EscribirAsync(sql, parametros);
                return filas > 0;
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion("Error al quitar misión", ex);
            }
        }

        //Obtener TODAS las misiones como jerarquía
        public async Task<List<IMision>> ObtenerArbol()
        {
            try
            {
                var sql = "SP_OBTENER_ARBOL_MISIONES";
                var tabla = await _acceso.LeerAsync(sql, null);

                return ReconstruirArbol(tabla);
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion("Error al obtener el árbol de misiones", ex);
            }
        }

        //Obtener una misión por Id (subárbol)
        public async Task<IMision?> ObtenerPorId(int id)
        {
            try
            {
                var sql = "SP_OBTENER_SUBARBOL_MISION";
                var parametros = new List<IDbDataParameter>
                {
                    _acceso.CrearParametro("@Id", id, DbType.Int32)
                };

                var tabla = await _acceso.LeerAsync(sql, parametros);
                var lista = ReconstruirArbol(tabla);

                return lista.FirstOrDefault();
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion("Error al obtener misión por Id", ex);
            }
        }

        //reconstrucción del Composite desde DataTable
        //private List<IMision> ReconstruirArbol(DataTable tabla)
        //{
        //    if (tabla == null || tabla.Rows.Count == 0)
        //        return new List<IMision>();

        //    var misiones = tabla.AsEnumerable().Select(r => new
        //    {
        //        Id = r.Field<int>("Id"),
        //        Nombre = r.Field<string>("Nombre"),
        //        Descripcion = r.Field<string>("Descripcion"),
        //        Dificultad = r.Field<int>("Dificultad"),
        //        EstaCompleta = r.Field<bool>("EsCompleta"),
        //        EsCompuesta = r.Field<bool>("EsCompuesta"),
        //        PadreId = r.IsNull("PadreId") ? (int?)null : r.Field<int>("PadreId")
        //    }).ToList();

        //    //Crear instancias sin relaciones
        //    var dic = new Dictionary<int, IMision>();
        //    foreach (var m in misiones)
        //    {
        //        IMision obj = m.EsCompuesta
        //            ? new MisionCompuesta(m.Id, m.Nombre, m.Descripcion, m.EstaCompleta)
        //            : new MisionSimple(m.Id, m.Nombre, m.Descripcion, m.Dificultad, m.EstaCompleta);

        //        dic[m.Id] = obj;
        //    }

        //    //Reconstruir relaciones padre-hijo
        //    foreach (var m in misiones.Where(x => x.PadreId != null))
        //    {
        //        if (dic[m.PadreId.Value] is MisionCompuesta padre)
        //            padre.Hijas.Add(dic[m.Id]);
        //    }

        //    //Devolver solo raíces
        //    return misiones.Where(x => x.PadreId == null).Select(x => dic[x.Id]).ToList();
        //}

        
        private List<IMision> ReconstruirArbol(DataTable tabla)
        {
            if (tabla == null || tabla.Rows.Count == 0)
                return new List<IMision>();

            var datos = MapearDatos(tabla);
            var dic = CrearInstancias(datos);
            ReconstruirRelaciones(datos, dic);

            return ObtenerRaices(datos, dic);
        }

        private List<MisionDTO> MapearDatos(DataTable tabla)
        {
            return tabla.AsEnumerable().Select(r => new MisionDTO
            {
                Id = r.Field<int>("Id"),
                Nombre = r.Field<string>("Nombre"),
                Descripcion = r.Field<string>("Descripcion"),
                Dificultad = r.Field<int>("Dificultad"),
                EstaCompleta = r.Field<bool>("EsCompleta"),
                EsCompuesta = r.Field<bool>("EsCompuesta"),
                PadreId = r.IsNull("PadreId") ? (int?)null : r.Field<int>("PadreId")
            }).ToList();
        }

        // DTO para reconstrucción
        private class MisionDTO
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public int Dificultad { get; set; }
            public bool EstaCompleta { get; set; }
            public bool EsCompuesta { get; set; }
            public int? PadreId { get; set; }
        }

        private Dictionary<int, IMision> CrearInstancias(List<MisionDTO> datos)
        {
            var dic = new Dictionary<int, IMision>();

            foreach (var m in datos)
            {
                IMision obj = m.EsCompuesta
                    ? new MisionCompuesta(m.Id, m.Nombre, m.Descripcion, m.EstaCompleta)
                    : new MisionSimple(m.Id, m.Nombre, m.Descripcion, m.Dificultad, m.EstaCompleta);

                dic[m.Id] = obj;
            }

            return dic;
        }

        private void ReconstruirRelaciones(List<MisionDTO> datos, Dictionary<int, IMision> dic)
        {
            foreach (var m in datos.Where(x => x.PadreId != null))
            {
                if (dic[m.PadreId.Value] is MisionCompuesta padre)
                    padre.Hijas.Add(dic[m.Id]);
            }
        }

        private List<IMision> ObtenerRaices(List<MisionDTO> datos, Dictionary<int, IMision> dic)
        {
            return datos
                .Where(x => x.PadreId == null)
                .Select(x => dic[x.Id])
                .ToList();
        }


        public async Task<bool> MarcarComoCompleta(int id)
        {
            try
            {
                var sql = "SP_COMPLETAR_MISION";
                var parametros = new List<IDbDataParameter>
        {
            _acceso.CrearParametro("@Id", id, DbType.Int32)
        };

                var filas = await _acceso.EscribirAsync(sql, parametros);
                return filas > 0;
            }
            catch (AccesoADatosExcepcion ex)
            {
                throw new RepositorioExcepcion("Error al completar misión", ex);
            }
        }
    }
}
