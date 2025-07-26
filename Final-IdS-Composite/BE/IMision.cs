using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public interface IMision : IEntidad
    {
        string Nombre { get; }
        string Descripcion { get; }
        int Dificultad { get; }
        bool EstaCompleta { get; }
        bool EsCompuesta { get; }   

        List<Item> ObtenerRecompensas();  
        void Mostrar();

    }
}
