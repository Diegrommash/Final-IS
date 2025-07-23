using BE;
using BE.Enums;
using BLL.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL.Decoradores
{
    public  class PocionDecorador : Decorador
    {
        public PocionDecorador(IComponente personaje, Item item) : base(personaje, item)
        {
        }

        public override string ObtenerDescripcion()
        {
            bool bonus = this.TieneTrabajoCompatible();
            string textobonus = bonus ? " (bonus x2!)" : "";

            return _personajeDecorado.ObtenerDescripcion()
                + $"\n🧪(pocion): {Nombre} (+{(bonus ? Poder * 2 : Poder)}  poder, + {(bonus ? Defensa * 2 : Defensa)} defensa){textobonus}";

        }
    }
}
