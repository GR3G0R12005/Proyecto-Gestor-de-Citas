using Aplicacion.Interfaces;

namespace Aplicacion.Servicios
{
    public class LeerLogServicio : ILeerLogServicio
    {
        private static string ruta = Path.Combine(AppContext.BaseDirectory, "LoggTXT.txt");

        public string LeerLogg(string admin)
        {
            try
            {
                string contenido = File.ReadAllText(ruta);

                if (string.IsNullOrEmpty(contenido))
                {
                    LoggerServicio.getInstancia().Error($"Fallo al leer el log por admin:{admin} en {DateTime.Now}");
                    throw new Exception("Error al leer el log");
                }

                LoggerServicio.getInstancia().Info($"Admin '{admin}' consultó el log a {DateTime.Now}");
                return contenido;
            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un error leyendo el log: " + ex.Message);
            }
        }
    }
}
