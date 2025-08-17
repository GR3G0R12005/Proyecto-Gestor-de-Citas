using System;
using System.IO;

namespace Aplicacion.Servicios
{
    public class LoggerServicio
    {
        private static readonly string ruta = Path.Combine(AppContext.BaseDirectory, "LoggTXT.txt");
        private static readonly object _lockControl = new object();
        private static LoggerServicio? instancia = null;

        private LoggerServicio() { }

        public static LoggerServicio getInstancia()
        {
            if (instancia == null)
            {
                lock (_lockControl)
                {
                    if (instancia == null)
                        instancia = new LoggerServicio();
                }
            }
            return instancia;
        }

        public void Error(string mensaje) => EscribirLog("ERROR", mensaje);
        public void Info(string mensaje) => EscribirLog("INFO", mensaje);
        public void Warning(string mensaje) => EscribirLog("WARNING", mensaje);
        public void Modificacion(string mensaje) => EscribirLog("MODIFICACION", mensaje);

        private void EscribirLog(string tipo, string mensaje)
        {
            lock (_lockControl)
            {
                string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string actividad = $"{date} [{tipo}] - {mensaje}";
                File.AppendAllText(ruta, actividad + Environment.NewLine);
            }
        }

        public string LeerLog()
        {
            try
            {
                return File.ReadAllText(ruta);
            }
            catch (Exception ex)
            {
                return $"Error leyendo log: {ex.Message}";
            }
        }
    }
}
