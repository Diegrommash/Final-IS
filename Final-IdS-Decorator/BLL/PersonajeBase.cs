
using BLL.Abstracciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Decoradores
{
    public class PersonajeBase : IComponente
    {
        public string Nombre { get; set; }
        public int Id { get; set; }
        public PersonajeBase(string nombre)
        {
            Nombre = nombre;
        }

        public IList<Decorador> Decorados { get; set; }

        public virtual string ObtenerDescripcion() => $"\n🧑personaje: {Nombre} ";
        public virtual int ObtenerPoder() => 0;
        public virtual int ObtenerDefensa() => 0;
        public int ObtenerNivel() => 0;

    }
}
