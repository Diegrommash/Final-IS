using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Enums;

namespace BLL.Abstracciones
{
    public interface IComponente : IClonable<IComponente>
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public TipoDecoradorEnum Tipo { get; set; }
        public int Poder { get; set; }
        public int Defensa { get; set; }
        public StatEnum AtributoPpal { get; set; }

        string ObtenerDescripcion();
        int ObtenerPoder();
        int ObtenerDefensa();
    }
}
