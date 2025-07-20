namespace Servicios
{
    public static class Logger
    {
        public static void LogInfo(string mensaje)
        {
            Console.WriteLine($"[INFO] {DateTime.Now} - {mensaje}");
        }

        public static void LogError(Exception ex, string mensaje)
        {
            Console.WriteLine($"[ERROR] {DateTime.Now} - {mensaje}");
            Console.WriteLine($"Exception: {ex.GetType().Name} - {ex.Message}");
        }
    }
}
