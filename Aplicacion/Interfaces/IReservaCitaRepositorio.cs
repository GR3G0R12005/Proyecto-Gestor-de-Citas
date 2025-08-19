using Infraestructura.Modelos;
using System;
using System.Collections.Generic;

public interface IReservaCitaRepositorio
{
    ReservaCita GetReservaCita(int idUsuario); // Última reserva activa
    List<ReservaCita> GetReservasUsuario(int idUsuario); // Todas las reservas de un usuario
    string ReservarCita(ReservaCita reserva);

    // 🔹 Ahora valida por fecha + hora + turno
    bool CitaActiva(int idUsuario, DateOnly fecha, string hora, string turno);

    ConfiguracionReserva obtenerConfiguracionPorTurno(DateOnly fecha, string turno);
    int contarReservasPorSlot(DateOnly fecha, TimeOnly hora, string turno);
    List<Estacione> obtenerTodasLasEstaciones();
    List<int> obtenerEstacionesOcupadas(DateOnly fecha, TimeOnly hora, string turno);
}
