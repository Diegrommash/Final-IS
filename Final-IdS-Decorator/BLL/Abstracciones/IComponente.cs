using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstracciones
{
    public interface IComponente
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        string ObtenerDescripcion();
        int ObtenerPoder();
        int ObtenerDefensa();
    }
}
