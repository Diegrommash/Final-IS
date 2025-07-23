
using BE.Enums;
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

        public IList<Decorador> Decorados { get; set; }
        public TipoDecoradorEnum Tipo { get; set; }
        public int Poder { get; set; }
        public int Defensa { get; set; }
        public StatEnum AtributoPpal { get; set; }

        public PersonajeBase(string nombre)
        {
            Nombre = nombre;
        }

        public virtual string ObtenerDescripcion() => $"\n🧑personaje: {Nombre} ";
        public virtual int ObtenerPoder() => 0;
        public virtual int ObtenerDefensa() => 0;

        public IComponente Clonar()
        {
            return new PersonajeBase(Nombre)
            {
                Id = this.Id,
                Tipo = this.Tipo,
                Poder = this.Poder,
                Defensa = this.Defensa,
                AtributoPpal = this.AtributoPpal
            };
        }
    }
}
