

using Microsoft.Extensions.Configuration;

namespace Servicios
{
    public static class Configuracion
    {
        public static string ObtenerCadenaConexion(string nombre = "DefaultConnection")
        {
            var configuracion = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            return configuracion.GetConnectionString(nombre);
        }
    }
}
