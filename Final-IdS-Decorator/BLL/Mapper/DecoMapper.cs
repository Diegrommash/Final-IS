using BE.Enums;
using BE;
using BLL.Abstracciones;
using BLL.Decoradores;

public static class DecoMapper
{
    public static IComponente ADecorador(this Item item, IComponente personaje)
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

    //public static Item AItem()
}
