using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class MisionSimple : IMision
    {
        public int Id { get; set; }
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }
        public int Dificultad { get; private set; }
        public bool EstaCompleta { get; private set; }

        public bool EsCompuesta => false;

        private readonly List<Item> _recompensas = new();

        public MisionSimple(string nombre, string descripcion, int dificultad)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Dificultad = dificultad;
            EstaCompleta = false;
        }

        public void Completar() => EstaCompleta = true;

        public void AgregarRecompensa(Item item) => _recompensas.Add(item);

        public List<Item> ObtenerRecompensas() => _recompensas;

        public void Mostrar()
        {
            Console.WriteLine($"[Misión simple] {Nombre} - {Descripcion} (Dif: {Dificultad})");
        }
    }
}
