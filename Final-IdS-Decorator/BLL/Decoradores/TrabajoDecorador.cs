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
    public class TrabajoDecorador : Decorador
    {

        public TrabajoDecorador(IComponente personaje, Item item) : base(personaje, item)
        {
        }

        public override string ObtenerDescripcion() => _personajeDecorado.ObtenerDescripcion() + $"\n⚒(oficio): {Nombre} (+{Poder} poder, +{Defensa} defensa)";

    }
}
