using System;
using System.Collections.Generic;

namespace BE
{
    public interface IMision : IEntidad
    {
        string Nombre { get; }
        string Descripcion { get; }
        int Dificultad { get; }
        bool EstaCompleta { get; }
        bool EsCompuesta { get; }

        /// <summary>
        /// Hijas de esta misión (en MisionSimple siempre estará vacío).
        /// </summary>
        List<IMision> Hijas { get; }

        /// <summary>
        /// Devuelve todas las recompensas asociadas a la misión.
        /// </summary>
        List<Item> ObtenerRecompensas();

        /// <summary>
        /// Devuelve una representación de texto de la misión.
        /// </summary>
        string Mostrar();
    }
}
