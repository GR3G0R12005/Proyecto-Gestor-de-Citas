using Aplicacion.Interfaces;
using Infraestructura.Contexto;
using Infraestructura.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositorios
{
    public class ConfiguracionReservaRepositorio : IConfiguracionReservaRepositorio
    {

        private readonly ReservaCitasDbContext context;
    
        public ConfiguracionReservaRepositorio(ReservaCitasDbContext context)
        {

            this.context = context;

        }

        public string crearConfiguracion(ConfiguracionReserva configuracion)
        {
            try
            {
                context.ConfiguracionReservas.Add(configuracion);
                context.SaveChanges();
                return "Cofiguracion creada exitosamente";
            }
            catch (Exception ex)
            {

                throw new Exception("Hubo un error al crear la configuacion " + ex.Message);

            }
        }

        public async Task<List<ConfiguracionReserva>> obtenerTodasAsync()
        {
            return await context.ConfiguracionReservas.ToListAsync();
        }

        public string actualizarConfiguracion(ConfiguracionReserva reserva)
        {
            try
            {
                context.ConfiguracionReservas.Entry(reserva).State = EntityState.Modified;
                context.SaveChanges();

                return "Cofiguracion actualizada correctamente";
            }
            catch (Exception ex)
            {

                throw new Exception("Hubo un error al modificar la configuracion " + ex.Message);

            }
        }

        public ConfiguracionReserva obtenerConfiguracion(DateOnly fecha, string turno)
        {

            try
            {
                var configuracion = context.ConfiguracionReservas.FirstOrDefault(x => x.Fecha == fecha && x.Turno == turno);

                if(configuracion == null)
                {
                    throw new Exception("Hubo un error al obtener la configuracion");
                }

                return configuracion;
            }
            catch (Exception ex)
            {

                throw new Exception("Hubo un error al obtener la configuracion " + ex.Message);

            }
        }

        public List<ConfiguracionReserva> obtenerTodas()
        {
            try
            {
                return context.ConfiguracionReservas.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un error al obtener todas las configuraciones " + ex.Message);
            }
        }
    }
}

