using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Orden : IEntidad
    {
        public string Declaracion { get; set; }
        public int Id { get; set; }

        public Orden(string declaracion, int id)
        {
            Declaracion = declaracion;
            Id = id;
        }
        public Orden()
        {
            
        }
    }
}
