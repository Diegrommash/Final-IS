using BE;
using BE.Excepciones;
using DAL;

namespace BLL
{
    public class ServicioPersonaje
    {
        private readonly RepoPersonaje _repoPersonaje;
        private readonly RepoItem _repoItem;
        private readonly Acceso _acceso;
        public ServicioPersonaje()
        {
            _acceso = new Acceso();
            _repoPersonaje = new RepoPersonaje(_acceso);
            _repoItem = new RepoItem(_acceso);
        }
           
        public async Task<List<Personaje>> BuscarPersonajes(Jugador jugador)
        {
            try
            {
                List<Personaje> personajesDecorados = new List<Personaje>();

                var personajes = await _repoPersonaje.BuscarPersonajesDeJugador(jugador.Id);
                if (personajes == null || personajes.Count == 0)
                {
                    return new List<Personaje>();
                }

                foreach (var personaje in personajes)
                {
                    var items = await _repoItem.BuscarItemsDePersonajeOrdenados(personaje.Id);
                    personaje.Items = items;

                    personajesDecorados.Add(personaje);
                }

                return personajesDecorados;
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al buscar los items", ex);
            }
        }


    }
}
