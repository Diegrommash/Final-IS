using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BE;
using BE.Enums;
using BLL.Abstracciones;
using BLL.Decoradores;

namespace BLL.Mapper
{
    public static class DecoMapper
    {
        public static IComponente AplicarItem(this Item item, IComponente personaje)
        {
            return item.Tipo switch
            {
                TipoDecoradorEnum.Arma => new ArmaDecorador(personaje, item),
                TipoDecoradorEnum.Armadura => new ArmaduraDecorador(personaje, item),
                TipoDecoradorEnum.Joya => new JoyaDecorador(personaje, item),
                TipoDecoradorEnum.Pocion => new PocionDecorador(personaje, item),
                TipoDecoradorEnum.Trabajo => new TrabajoDecorador(personaje, item),
                _ => throw new NotSupportedException($"Tipo no soportado: {item.Tipo}")
            };
        }



    }
}
