using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.DTOs;

namespace Aplicacion.Interfaces
{
    public interface IReservaCitasServicio
    {
        Task<string> ReservarCita(string correo, string nombre, int idUsuario, ReservaCitasDTO reserva);

        ReservaCitasDTO GetReservaCita(string nombre, int idUsuario);

        List<ReservaCitasDTO> GetReservasUsuario(int idUsuario, string nombre);
    }
}
