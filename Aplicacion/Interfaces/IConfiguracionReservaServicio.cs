using Aplicacion.DTOs;

namespace Aplicacion.Interfaces
{
    public interface IConfiguracionReservaServicio
    {
        string crearConfiguracion(ConfiguracionDTO configuracion,string admin);
        string actualizarConfiguracion(ConfiguracionDTO reserva,string nombre);
        ConfiguracionDTO obtenerConfiguracion(DateOnly fecha, string turno,string nombre);
        List<ConfiguracionDTO> obtenerTodas();
    }
}
