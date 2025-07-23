using BE.Enums;
using BE;
using BLL.Abstracciones;
using BLL.Decoradores;

public static class DecoMapper
{
    public static IComponente AplicarItem(this Item item, IComponente personaje)
    {
        // Reglas de aplicación de trabajo
        bool tieneTrabajo = ContieneTrabajo(personaje);
        bool estaDecorado = EstaDecorado(personaje);

        if (item.Tipo == TipoDecoradorEnum.Trabajo)
        {
            if (tieneTrabajo)
                throw new InvalidOperationException("El personaje ya tiene un trabajo asignado.");

            if (estaDecorado)
                throw new InvalidOperationException("El trabajo debe ser el primer ítem asignado.");
        }
        else
        {
            if (!tieneTrabajo)
                throw new InvalidOperationException("Debe asignar un trabajo antes de aplicar otros ítems.");
        }

        // Validación de máximos permitidos
        if (MaximosPorTipo.TryGetValue(item.Tipo, out int maximo))
        {
            int actuales = ContarDecoradoresDelTipo(personaje, item.Tipo);
            if (actuales >= maximo)
                throw new InvalidOperationException($"Ya se alcanzó el máximo de {maximo} {item.Tipo}(s) permitidos.");
        }

        // Aplicar decorador correspondiente
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
}
