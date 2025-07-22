using BE;
using BE.Excepciones;
using BLL.Abstracciones;
using BLL.Decoradores;
using BLL.Mapper;
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

        public async Task<bool> GuardarPersonajeAsync(IComponente personajeDecorado, Jugador jugador)
        {
            try
            {
                var personajeBase = ObtenerPersonajeBase(personajeDecorado);
                var items = ExtraerDecoradores(personajeDecorado);

                await _acceso.ComenzarTransaccionAsync();

                personajeBase.Id = await _repoPersonaje.GuardarPersonaje(personajeBase.AEntidad());

                await _repoPersonaje.GuardarPersonajeDeJugador(personajeBase.AEntidad(), jugador);

                foreach (var (item, orden) in items)
                {
                    await _repoItem.Guardar(personajeBase.Id, item.Id, orden);
                }
                await _acceso.ConfirmarTransaccionAsync();
                return true;
            }
            catch (RepositorioExcepcion ex)
            {
                await _acceso.CancelarTransaccionAsync();
                throw new ServicioExcepcion("Error al guardar el personaje con sus items");
            }
        }

        public async Task<List<IComponente>> BuscarPersonajes(Jugador jugador)
        {
            try
            {
                List<IComponente> personajesDecorados = new List<IComponente>();

                var personajes = await _repoPersonaje.BuscarPersonajesDeJugador(jugador.Id);
                if (personajes == null || personajes.Count == 0)
                {
                    return new List<IComponente>();
                }

                foreach (var personaje in personajes)
                {
                    var items = await _repoItem.BuscarItemsDePersonajeOrdenados(personaje.Id);
                    personaje.Items = items;

                    personajesDecorados.Add(RearmarPersonaje(personaje));
                }

                return personajesDecorados;
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al buscar los items", ex);
            }

        }

        private  IComponente RearmarPersonaje(Personaje personaje)
        {
            try
            {
                IComponente personajeActual = new PersonajeBase(personaje.Nombre);

                foreach (var item in personaje.Items)
                {
                    personajeActual = item.AplicarItem(personajeActual);
                }

                return personajeActual;
            }
            catch (RepositorioExcepcion ex)
            {
                throw new ServicioExcepcion("Error al buscar los items", ex);
            }

        }

        public IComponente QuitarDecoradorPorOrden(IComponente personaje, int ordenAEliminar)
        {
            var decoradores = ExtraerDecoradores(personaje)
                .Where(d => d.Orden != ordenAEliminar)
                .OrderBy(d => d.Orden)
                .Select(d => d.Item)
                .ToList();

            IComponente nuevoPersonaje = ObtenerPersonajeBase(personaje);

            foreach (var item in decoradores)
            {
                nuevoPersonaje = item.AplicarItem(nuevoPersonaje);
            }

            return nuevoPersonaje;
        }

        public IComponente ReemplazarDecoradorPorOrden(IComponente personaje, int ordenAReemplazar, Item nuevoItem)
        {
            var decoradores = ExtraerDecoradores(personaje)
                .Select(d =>
                {
                    if (d.Orden == ordenAReemplazar)
                        return (Item: nuevoItem, Orden: d.Orden);
                    return d;
                })
                .OrderBy(d => d.Orden)
                .Select(d => d.Item)
                .ToList();

            IComponente nuevoPersonaje = ObtenerPersonajeBase(personaje);

            foreach (var item in decoradores)
            {
                nuevoPersonaje = item.AplicarItem(nuevoPersonaje);
            }

            return nuevoPersonaje;
        }


        public static List<(Item Item, int Orden)> ExtraerDecoradores(IComponente personajeDecorado)
        {
            var resultado = new List<(Item, int)>();
            int orden = 1;

            while (personajeDecorado is Decorador decorador)
            {
                var item = new Item
                {
                    Id = decorador.Id,
                    Nombre = decorador.Nombre,
                    Poder = decorador.Poder,
                    Defensa = decorador.Defensa,
                    Tipo = decorador.Tipo
                };

                resultado.Add((item, orden));
                orden++;

                personajeDecorado = decorador.ObtenerPersonajeInterno();
            }

            resultado.Reverse();
            return resultado;
        }

        public static PersonajeBase ObtenerPersonajeBase(IComponente componente)
        {
            while (componente is Decorador decorador)
            {
                componente = decorador.ObtenerPersonajeInterno();
            }

            return componente as PersonajeBase
                ?? throw new InvalidCastException("No se pudo encontrar el personaje base");
        }
    }
}
