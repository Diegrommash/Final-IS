using System;
using System.Collections.Generic;

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

        public List<IMision> Hijas => new();

        private readonly List<Item> _recompensas = new();

        // Constructor para creación manual
        public MisionSimple(string nombre, string descripcion, int dificultad)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Dificultad = dificultad;
            EstaCompleta = false;
        }

        // Constructor para reconstrucción desde BD
        public MisionSimple(int id, string nombre, string descripcion, int dificultad, bool completa)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            Dificultad = dificultad;
            EstaCompleta = completa;
        }

        public void Completar() => EstaCompleta = true;

        public void AgregarRecompensa(Item item) => _recompensas.Add(item);

        public List<Item> ObtenerRecompensas() => new(_recompensas);

        public string Mostrar()
        {
            return $"[Misión simple] {Nombre} - {Descripcion} (Dif: {Dificultad}) {(EstaCompleta ? "[COMPLETA]" : "")}";
        }

        public override string ToString() => Mostrar();
    }
}
