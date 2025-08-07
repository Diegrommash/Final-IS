using BE;
using BE.Excepciones;
using DAL;

namespace BLL
{
    public class ServicioMision
    {
        private readonly Acceso _acceso;
        private readonly RepoMision _repoMision;
        private readonly RepoRecompensa _repoRecompensa;

        public ServicioMision()
        {
            _acceso = new Acceso();
            _repoMision = new RepoMision(_acceso);
            _repoRecompensa = new RepoRecompensa(_acceso);
        }

        public async Task<int> CrearMisionSimple(string nombre, string descripcion, int dificultad, List<Item>? recompensas = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    throw new ArgumentException("El nombre de la misión no puede estar vacío.");

                if (dificultad <= 0)
                    throw new ArgumentException("La dificultad debe ser mayor a 0.");

                var mision = new MisionSimple(nombre, descripcion, dificultad);

                await _acceso.ComenzarTransaccionAsync();

                var idMision = await _repoMision.Agregar(mision);

                if (recompensas != null && recompensas.Count > 0)
                {
                    foreach (var item in recompensas)
                    {
                        await _repoRecompensa.Agregar(idMision, item.Id);
                    }
                }
                
                await _acceso.ConfirmarTransaccionAsync();

                return idMision;
            }
            catch (RepositorioExcepcion ex)
            {
                await  _acceso.CancelarTransaccionAsync();
                throw new ServicioExcepcion("Error al crear mision simple", ex);
            }       
        }

        public async Task<int> CrearMisionCompuesta(string nombre, string descripcion, List<Item>? recompensas = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    throw new ArgumentException("El nombre de la misión no puede estar vacío.");

                var mision = new MisionCompuesta(nombre, descripcion);
                await _acceso.ComenzarTransaccionAsync();

                var idMision = await _repoMision.Agregar(mision);

                if (recompensas != null && recompensas.Count > 0)
                {
                    foreach (var item in recompensas)
                    {
                        await _repoRecompensa.Agregar(idMision, item.Id);
                    }
                }

                await _acceso.ConfirmarTransaccionAsync();

                return idMision;
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al crear mision compuesta", ex);
            }

        }

        public async Task<bool> Modificar(IMision mision)
        {
            try
            {
                if (mision == null) throw new ArgumentNullException(nameof(mision));
                return await _repoMision.Modificar(mision);
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al modiricar mision", ex);
            }

        }

        public async Task<bool> Eliminar(IMision mision)
        {
            try
            {
                if (mision == null) throw new ArgumentNullException(nameof(mision));
                return await _repoMision.Eliminar(mision);
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al eliminar mision", ex);
            }

        }

        public async Task<bool> AsignarMision(IMision padre, IMision hija)
        {
            try
            {
                if (padre == null || hija == null)
                    throw new ArgumentException("Las misiones no pueden ser nulas.");

                if (!padre.EsCompuesta)
                    throw new InvalidOperationException("No se puede asignar una misión hija a una misión simple.");

                if (padre.Id == hija.Id)
                    throw new ServicioExcepcion("Una misión no puede ser asignada como hija de sí misma.");

                if (EsDescendiente(padre, hija))
                    throw new ServicioExcepcion("No se puede asignar esta misión porque generaría un ciclo en la jerarquía.");

                return await _repoMision.Asignar(padre, hija);
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al asignar mision a una mision compuesta", ex);
            }

        }

        public async Task<bool> QuitarMision(IMision padre, IMision hija)
        {
            try
            {
                if (padre == null || hija == null)
                    throw new ArgumentException("Las misiones no pueden ser nulas.");

                if (!padre.EsCompuesta)
                    throw new InvalidOperationException("No se puede quitar una misión hija de una misión simple.");

                return await _repoMision.Quitar(padre, hija);
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al quitar mision de una mision compuesta", ex);
            }

        }

        public async Task<List<IMision>> ObtenerArbol()
        {
            try
            {
                return await _repoMision.ObtenerArbol();
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al buscar el arbol completo", ex);
            }
        }

        public async Task<IMision?> ObtenerPorId(int id)
        {
            try
            {
                return await _repoMision.ObtenerPorId(id);
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al buscar una mision con sus submisiones", ex);
            }
        }

        public async Task CompletarMision(IMision mision)
        {
            try
            {
                if (mision == null) throw new ArgumentNullException(nameof(mision));

                if (mision.EsCompuesta && mision.Hijas.Count > 0)
                {
                    foreach (var hija in mision.Hijas)
                    {
                        if (!hija.EstaCompleta)
                            throw new InvalidOperationException($"No se puede completar la misión '{mision.Nombre}' porque la hija '{hija.Nombre}' no está completa.");
                    }
                }
                await _repoMision.MarcarComoCompleta(mision.Id);
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al completar una mision", ex);
            }
   
        }

        public bool EsDescendiente(IMision padre, IMision posibleHija)
        {
            if (posibleHija.Hijas == null)
                return false;

            foreach (var hija in posibleHija.Hijas)
            {
                if (hija.Id == padre.Id || EsDescendiente(padre, hija))
                    return true;
            }

            return false;
        }


    }
}
