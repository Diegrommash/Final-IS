using BE;
using BE.Enums;
using BLL.Abstracciones;

namespace BLL.Decoradores
{
    public abstract class Decorador : IComponente
    {
        protected IComponente _personajeDecorado;

 
        protected string _nombre;
        protected TipoDecoradorEnum _tipo;
        protected int _poder;
        protected int _defensa;
        protected StatEnum _atributoPpal;

        public string Nombre => _nombre;
        public TipoDecoradorEnum Tipo => _tipo;
        public int Poder => _poder;
        public int Defensa => _defensa;

        public int Orden { get; set; }

        public int Id { get; set; }

        public Decorador(IComponente personaje, Item item)
        {
            Id = item.Id;
            _personajeDecorado = personaje ?? throw new ArgumentNullException(nameof(personaje));
            _nombre = item.Nombre;
            _tipo = item.Tipo;
            _poder = item.Poder;
            _defensa = item.Defensa;
            _atributoPpal = item.AtributoPpl;
        }

        public virtual string ObtenerDescripcion()
        {

            return _personajeDecorado.ObtenerDescripcion();
               
        }

        public virtual int ObtenerPoder()
        {
            int bonus = TieneTrabajoCompatible() ? _poder * 2 : _poder;
            return _personajeDecorado.ObtenerPoder() + bonus;
        }

        public virtual int ObtenerDefensa()
        {
            int bonus = TieneTrabajoCompatible() ? _defensa * 2 : _defensa;
            return _personajeDecorado.ObtenerDefensa() + bonus;
        }

        protected bool TieneTrabajoCompatible()
        {
            IComponente actual = _personajeDecorado;

            while (actual is Decorador decorador)
            {
                if (decorador.Tipo == TipoDecoradorEnum.Trabajo &&
                    decorador._atributoPpal == this._atributoPpal)
                {
                    return true;
                }

                actual = decorador.ObtenerPersonajeInterno();
            }

            return false;
        }

        /// <summary>
        /// a futuro, no lo pense bien todavia
        /// </summary>
        /// <returns></returns>
        public IComponente ObtenerPersonajeInterno() => _personajeDecorado;

        public void ReemplazarPersonajeInterno(IComponente nuevo)
        {
            _personajeDecorado = nuevo;
        }

    }
}
