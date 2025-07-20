using BE;
using BE.Enums;
using BLL.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Decoradores
{
    public class JoyaDecorador : Decorador
    {
        public JoyaDecorador(IComponente personaje, Item item) : base(personaje, item)
        {
        }

        public override string ObtenerDescripcion()
        {
            bool bonus = this.TieneTrabajoCompatible();
            string textobonus = bonus ? " (bonus x2!)" : "";

            return _personajeDecorado.ObtenerDescripcion()
                + $"\n💍(joya): {_nombre} (+{(bonus ? _poder * 2 : _poder)} poder, +{(bonus ? _defensa * 2 : _defensa)} defensa){textobonus}";
        }
    }
}
