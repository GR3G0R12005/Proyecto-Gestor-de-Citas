using Infraestructura.Modelos;

namespace Aplicacion.Interfaces
{
    public interface IConfiguracionReservaRepositorio
    {
        string crearConfiguracion(ConfiguracionReserva configuracion);
        string actualizarConfiguracion(ConfiguracionReserva reserva);
        ConfiguracionReserva obtenerConfiguracion(DateOnly fecha, string turno);
        List<ConfiguracionReserva> obtenerTodas();
    }
}
