namespace BE
{
    public class Jugador
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public  string Contraseña { get; set; }

        public Jugador()
        {
            
        }
        public Jugador(string nombre, string contraseña)
        {
            Nombre = nombre;
            Contraseña = contraseña;
        }
    }
}
