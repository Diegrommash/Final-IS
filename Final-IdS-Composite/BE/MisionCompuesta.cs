using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class MisionCompuesta : IMision
    {
        public int Id { get; set; }
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }
        public int Dificultad => _submisiones.Sum(m => m.Dificultad);
        public bool EstaCompleta => _submisiones.All(m => m.EstaCompleta);

        public bool EsCompuesta => true;

        private readonly List<IMision> _submisiones = new();
        private readonly List<Item> _recompensas = new();

        public MisionCompuesta(string nombre, string descripcion)
        {
            Nombre = nombre;
            Descripcion = descripcion;
        }

        public void Agregar(IMision mision) => _submisiones.Add(mision);
        public void Quitar(IMision mision) => _submisiones.Remove(mision);
        public List<IMision> ObtenerSubmisiones() => _submisiones;

        public void AgregarRecompensa(Item item) => _recompensas.Add(item);

        public List<Item> ObtenerRecompensas()
        {
            var todas = new List<Item>(_recompensas);
            foreach (var m in _submisiones)
                todas.AddRange(m.ObtenerRecompensas());
            return todas;
        }

        public void Mostrar()
        {
            Console.WriteLine($"[Misión compuesta] {Nombre} - {Descripcion} (Dif total: {Dificultad})");
            foreach (var sub in _submisiones)
                sub.Mostrar();
        }
    }
}
