using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Enums;

namespace BE
{
    public class Item
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Defensa { get; set; }
        public int Poder { get; set; }
        public StatEnum AtributoPpl { get; set; }
        public TipoDecoradorEnum Tipo { get; set; }
    }
}
