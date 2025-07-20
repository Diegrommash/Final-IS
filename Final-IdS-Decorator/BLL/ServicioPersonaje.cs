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

        public async Task<bool> GuardarPersonajeAsync(IComponente personajeDecorado)
        {
            try
            {
                var personajeBaje = ObtenerPersonajeBase(personajeDecorado);
                var items = ExtraerDecoradores(personajeDecorado);

                await _acceso.ComenzarTransaccionAsync();
                var idPersonaje = await _repoPersonaje.GuardarPersonaje(personajeBaje.AEntidad());

                foreach (var (item, orden) in items)
                {
                    await _repoItem.Guardar(idPersonaje, item.Id, orden);
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

        public async Task<IComponente> RearmarPersonaje(Personaje personaje)
        {
            try
            {
                var persona = await _repoPersonaje.BuscarPersonaje(personaje.Id);
                var items = await _repoItem.BuscarItemsDePersonajeOrdenados(personaje.Id);

                IComponente personajeActual = new PersonajeBase(persona.Nombre);

                foreach (var item in items)
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


        private static List<(Item Item, int Orden)> ExtraerDecoradores(IComponente personajeDecorado)
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

        private PersonajeBase ObtenerPersonajeBase(IComponente componente)
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
