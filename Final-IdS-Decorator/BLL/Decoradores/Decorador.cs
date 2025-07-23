using BE;
using BE.Enums;
using BLL.Abstracciones;

namespace BLL.Decoradores
{
    public abstract class Decorador : IComponente
    {
        protected IComponente _personajeDecorado;

        public int Orden { get; set; }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public TipoDecoradorEnum Tipo { get; set; }
        public int Poder { get; set; }
        public int Defensa { get; set; }
        public StatEnum AtributoPpal { get; set; }

        private Item _item;

        public Decorador(IComponente personaje, Item item)
        {
            _personajeDecorado = personaje ?? throw new ArgumentNullException(nameof(personaje));

            Id = item.Id;
            Nombre = item.Nombre;
            Tipo = item.Tipo;
            Poder = item.Poder;
            Defensa = item.Defensa;
            AtributoPpal = item.AtributoPpl;
            _item = item ?? throw new ArgumentNullException(nameof(item));
        }

        public virtual string ObtenerDescripcion()
        {

            return _personajeDecorado.ObtenerDescripcion();
               
        }

        public virtual int ObtenerPoder()
        {
            int bonus = TieneTrabajoCompatible() ? Poder * 2 : Poder;
            return _personajeDecorado.ObtenerPoder() + bonus;
        }

        public virtual int ObtenerDefensa()
        {
            int bonus = TieneTrabajoCompatible() ? Defensa * 2 : Defensa;
            return _personajeDecorado.ObtenerDefensa() + bonus;
        }

        protected bool TieneTrabajoCompatible()
        {
            IComponente actual = _personajeDecorado;

            while (actual is Decorador decorador)
            {
                if (decorador.Tipo == TipoDecoradorEnum.Trabajo &&
                    decorador.AtributoPpal == this.AtributoPpal)
                {
                    return true;
                }

                actual = decorador.ObtenerPersonajeInterno();
            }

            return false;
        }

        public IComponente ObtenerPersonajeInterno() => _personajeDecorado;

        public void ReemplazarPersonajeInterno(IComponente nuevo)
        {
            _personajeDecorado = nuevo;
        }

        public virtual IComponente Clonar()
        {
            var clonInterno = (_personajeDecorado as IClonable<IComponente>)?.Clonar();

            if (clonInterno == null)
                throw new InvalidOperationException("No se pudo clonar el personaje interno.");

            var clonDecorador = (Decorador)Activator.CreateInstance(this.GetType(), clonInterno, _item)!;

            clonDecorador.Id = this.Id;
            clonDecorador.Nombre = this.Nombre;
            clonDecorador.Tipo = this.Tipo;
            clonDecorador.Poder = this.Poder;
            clonDecorador.Defensa = this.Defensa;
            clonDecorador.AtributoPpal = this.AtributoPpal;
            clonDecorador.Orden = this.Orden;

            return clonDecorador;
        }
    }
}
