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
            if (MaximosPorTipo.TryGetValue(item.Tipo, out int maximo))
            {
                int actuales = ContarDecoradoresDelTipo(personaje, item.Tipo);
                if (actuales >= maximo)
                    throw new InvalidOperationException($"Ya se alcanzó el máximo de {maximo} {item.Tipo}(s) permitidos.");
            }

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

    }
}
