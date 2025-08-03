using BE;
using BE.Enums;
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
                    personajeActual = item.ADecorador(personajeActual);
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
                nuevoPersonaje = item.ADecorador(nuevoPersonaje);
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
                nuevoPersonaje = item.ADecorador(nuevoPersonaje);
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

        #region Validaciones
        public void ValidarAplicacionItem(IComponente personaje, Item item)
        {
            bool tieneTrabajo = ContieneTrabajo(personaje);

            if (item.Tipo == TipoDecoradorEnum.Trabajo)
            {
                if (tieneTrabajo)
                    throw new InvalidOperationException("El personaje ya tiene un trabajo asignado.");
            }
            else
            {
                if (!tieneTrabajo)
                    throw new InvalidOperationException("Debe asignar un trabajo antes de aplicar otros ítems.");
            }

            if (MaximosPorTipo.TryGetValue(item.Tipo, out int maximo))
            {
                int actuales = ContarDecoradoresDelTipo(personaje, item.Tipo);
                if (actuales >= maximo)
                    throw new InvalidOperationException($"Ya se alcanzó el máximo de {maximo} {item.Tipo}(s) permitidos.");
            }
        }

        public void ValidarQuitarItem(IComponente personaje, Item item)
        {
            if (ContieneItem(personaje, item.Id))
            {
                if (item.Tipo == TipoDecoradorEnum.Trabajo)
                {
                    if (EstaDecorado(personaje))
                        throw new InvalidOperationException("El personaje tiene armamento no podes quitarle el trabajo.");
                }
            }
            else
            {
                throw new InvalidOperationException("El personaje no contiene el item que se desea quitar.");
            }
        }

        private static readonly Dictionary<TipoDecoradorEnum, int> MaximosPorTipo = new()
        {
            { TipoDecoradorEnum.Trabajo, 1 },
            { TipoDecoradorEnum.Arma, 2 },
            { TipoDecoradorEnum.Armadura, 1 },
            { TipoDecoradorEnum.Joya, 3 },
            { TipoDecoradorEnum.Pocion, 5 }
        };

        private static int ContarDecoradoresDelTipo(IComponente personaje, TipoDecoradorEnum tipo)
        {
            int cantidad = 0;
            IComponente actual = personaje;

            while (actual is Decorador decorador)
            {
                if (decorador.Tipo == tipo)
                    cantidad++;

                actual = decorador.ObtenerPersonajeInterno();
            }

            return cantidad;
        }

        private static bool ContieneTrabajo(IComponente personaje)
        {
            IComponente actual = personaje;
            while (actual is Decorador decorador)
            {
                if (decorador.Tipo == TipoDecoradorEnum.Trabajo)
                    return true;

                actual = decorador.ObtenerPersonajeInterno();
            }
            return false;
        }

        private static bool EstaDecorado(IComponente personaje)
        {
            return personaje is Decorador;
        }

        private bool ContieneItem(IComponente personaje, int itemId)
        {
            IComponente actual = personaje;

            while (actual is Decorador decorador)
            {
                if (decorador.Id == itemId)
                    return true;

                actual = decorador.ObtenerPersonajeInterno();
            }
            return false;
        }
        #endregion


    }
}
