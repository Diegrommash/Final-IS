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
    public class ArmaDecorador : Decorador
    {
        public ArmaDecorador(IComponente personaje, Item item) : base(personaje, item)
        {
        }

        public override string ObtenerDescripcion()
        {
            bool bonus = this.TieneTrabajoCompatible();
            string textobonus = bonus ? " (bonus x2!)" : "";

            return _personajeDecorado.ObtenerDescripcion() 
                + $"\n⚔(arma):  {_nombre} (+{(bonus ? _poder * 2 : _poder)} poder, +{(bonus ? _defensa * 2 : _defensa)} defensa){textobonus}";
        }

    }
}
