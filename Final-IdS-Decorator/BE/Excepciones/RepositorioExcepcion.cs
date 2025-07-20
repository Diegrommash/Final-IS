using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Excepciones
{
    public class RepositorioExcepcion : Exception
    {
        public RepositorioExcepcion(string mensaje, Exception exceptionInterna = null) : base(mensaje, exceptionInterna)
        {
            
        }
    }
}
