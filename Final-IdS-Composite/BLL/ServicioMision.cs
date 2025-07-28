using BE;
using BE.Excepciones;
using DAL;

namespace BLL
{
    public class ServicioMision
    {
        private readonly RepoMision _repo;

        public ServicioMision()
        {
            _repo = new RepoMision();
        }

        //Crear misión simple
        public async Task<int> CrearMisionSimple(string nombre, string descripcion, int dificultad)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    throw new ArgumentException("El nombre de la misión no puede estar vacío.");

                if (dificultad <= 0)
                    throw new ArgumentException("La dificultad debe ser mayor a 0.");

                var mision = new MisionSimple(nombre, descripcion, dificultad);
                return await _repo.Agregar(mision);
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al crear mision simple", ex);
            }       
        }

        //Crear misión compuesta
        public async Task<int> CrearMisionCompuesta(string nombre, string descripcion)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    throw new ArgumentException("El nombre de la misión no puede estar vacío.");

                var mision = new MisionCompuesta(nombre, descripcion);
                return await _repo.Agregar(mision);
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al crear mision compuesta", ex);
            }

        }

        //Modificar misión (simple o compuesta)
        public async Task<bool> Modificar(IMision mision)
        {
            try
            {
                if (mision == null) throw new ArgumentNullException(nameof(mision));
                return await _repo.Modificar(mision);
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al modiricar mision", ex);
            }

        }

        //Eliminar misión
        public async Task<bool> Eliminar(IMision mision)
        {
            try
            {
                if (mision == null) throw new ArgumentNullException(nameof(mision));
                return await _repo.Eliminar(mision);
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al eliminar mision", ex);
            }

        }

        //Asignar hija a una misión compuesta
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

                return await _repo.Asignar(padre, hija);
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al asignar mision a una mision compuesta", ex);
            }

        }

        //Quitar misión hija
        public async Task<bool> QuitarMision(IMision padre, IMision hija)
        {
            try
            {
                if (padre == null || hija == null)
                    throw new ArgumentException("Las misiones no pueden ser nulas.");

                if (!padre.EsCompuesta)
                    throw new InvalidOperationException("No se puede quitar una misión hija de una misión simple.");

                return await _repo.Quitar(padre, hija);
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al quitar mision de una mision compuesta", ex);
            }

        }

        //Obtener toda la jerarquía de misiones
        public async Task<List<IMision>> ObtenerArbol()
        {
            try
            {
                return await _repo.ObtenerArbol();
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al buscar el arbol completo", ex);
            }
        }

        //Obtener una misión específica con su subárbol
        public async Task<IMision?> ObtenerPorId(int id)
        {
            try
            {
                return await _repo.ObtenerPorId(id);
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al buscar una mision con sus submisiones", ex);
            }
        }

        //Marcar misión como completada
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
                await _repo.MarcarComoCompleta(mision.Id);
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al completar una mision", ex);
            }
   
        }

        //Verificar si una misión es descendiente de otra
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
