using BE;
using BLL.Decoradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mapper
{
    public static class PersonajeMapper
    {
        public static Personaje AEntidad(this PersonajeBase personaje)
        {
            return new Personaje
            {
                Nombre = personaje.Nombre
            };
        }
    }
}
