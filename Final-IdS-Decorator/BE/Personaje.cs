using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Personaje
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public IList<Item> Items { get; set; }

    }
}
