using System;
using System.Collections.Generic;
using System.Linq;

namespace BE
{
    public class MisionCompuesta : IMision
    {
        public int Id { get; set; }
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }

        /// <summary>
        /// La dificultad total es la suma de las dificultades de las submisiones.
        /// </summary>
        public int Dificultad => _submisiones.Sum(m => m.Dificultad);

        /// <summary>
        /// Se considera completa solo si todas sus submisiones están completas.
        /// </summary>
        public bool EstaCompleta => _forzarCompleta || _submisiones.All(m => m.EstaCompleta);

        public bool EsCompuesta => true;

        /// <summary>
        /// Hijas de esta misión compuesta.
        /// </summary>
        public List<IMision> Hijas => _submisiones;

        private readonly List<IMision> _submisiones = new();
        private readonly List<Item> _recompensas = new();
        private bool _forzarCompleta = false;

        // Constructor para creación manual
        public MisionCompuesta(string nombre, string descripcion)
        {
            Nombre = nombre;
            Descripcion = descripcion;
        }

        // Constructor para reconstrucción desde BD
        public MisionCompuesta(int id, string nombre, string descripcion, bool completa = false)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            if (completa) Completar();
        }

        public void Agregar(IMision mision) => _submisiones.Add(mision);

        public void Quitar(IMision mision)
        {
            if (_submisiones.Contains(mision))
                _submisiones.Remove(mision);
        }

        public List<IMision> ObtenerSubmisiones() => new(_submisiones);

        public void AgregarRecompensa(Item item) => _recompensas.Add(item);

        public List<Item> ObtenerRecompensas()
        {
            var todas = new List<Item>(_recompensas);
            foreach (var m in _submisiones)
                todas.AddRange(m.ObtenerRecompensas());
            return todas;
        }

        /// <summary>
        /// Permite marcar la misión compuesta como completa independientemente de sus hijos.
        /// </summary>
        public void Completar() => _forzarCompleta = true;

        public string Mostrar()
        {
            var estado = EstaCompleta ? "[COMPLETA]" : "";
            var info = $"[Misión compuesta] {Nombre} - {Descripcion} (Dif total: {Dificultad}) {estado}";

            if (_recompensas.Count > 0)
            {
                var recompensasStr = string.Join(", ", _recompensas.ConvertAll(r => r.Nombre));
                info += $" - Recompensas: {recompensasStr}";
            }
            else
            {
                info += " - Sin recompensas.";
            }

            foreach (var sub in _submisiones)
                info += "\n  -> " + sub.Mostrar();

            return info;
        }


        public override string ToString() => Mostrar();
    }
}
