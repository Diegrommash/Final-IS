using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Excepciones
{
    public class ServicioExcepcion : Exception
    {
        public ServicioExcepcion(string mensaje, Exception exceptionInterna = null) : base(mensaje, exceptionInterna)
        {
            
        }
    }
}
