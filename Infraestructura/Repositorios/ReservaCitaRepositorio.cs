using Infraestructura.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using Infraestructura.Contexto;
public class ReservaCitaRepositorio : IReservaCitaRepositorio
{
    private readonly ReservaCitasDbContext _context;

    public ReservaCitaRepositorio(ReservaCitasDbContext context)
    {
        _context = context;
    }

    // 🔹 Devuelve la última reserva activa de un usuario
    public ReservaCita GetReservaCita(int idUsuario)
    {
        return _context.ReservaCitas
            .Where(r => r.IdUsuario == idUsuario && r.Estado == "Pendiente")
            .OrderByDescending(r => r.Fecha)
            .ThenByDescending(r => r.Hora)
            .FirstOrDefault();
    }

    // 🔹 Devuelve todas las reservas del usuario
    public List<ReservaCita> GetReservasUsuario(int idUsuario)
    {
        return _context.ReservaCitas
            .Where(r => r.IdUsuario == idUsuario)
            .OrderByDescending(r => r.Fecha)
            .ThenByDescending(r => r.Hora)
            .ToList(); // ✅ Aquí está la diferencia (antes seguro tenías FirstOrDefault)
    }

    // 🔹 Registrar nueva reserva
    public string ReservarCita(ReservaCita reserva)
    {
        _context.ReservaCitas.Add(reserva);
        _context.SaveChanges();
        return "OK";
    }

    // 🔹 Validar si ya tiene una cita activa en esa fecha + hora + turno
    public bool CitaActiva(int idUsuario, DateOnly fecha, string hora, string turno)
    {
        var horaParsed = TimeOnly.Parse(hora);

        return _context.ReservaCitas.Any(r =>
            r.IdUsuario == idUsuario &&
            r.Fecha == fecha &&
            r.Hora == horaParsed &&
            r.Turno == turno &&
            r.Estado == "Pendiente");
    }

    // 🔹 Obtener configuración del turno
    public ConfiguracionReserva obtenerConfiguracionPorTurno(DateOnly fecha, string turno)
    {
        return _context.ConfiguracionReservas
            .FirstOrDefault(c => c.Turno == turno && c.Fecha == fecha);
    }

    // 🔹 Contar reservas en un slot
    public int contarReservasPorSlot(DateOnly fecha, TimeOnly hora, string turno)
    {
        return _context.ReservaCitas.Count(r =>
            r.Fecha == fecha &&
            r.Hora == hora &&
            r.Turno == turno &&
            r.Estado == "Pendiente");
    }

    // 🔹 Obtener todas las estaciones
    public List<Estacione> obtenerTodasLasEstaciones()
    {
        return _context.Estaciones.ToList();
    }

    // 🔹 Obtener estaciones ocupadas en un slot
    public List<int> obtenerEstacionesOcupadas(DateOnly fecha, TimeOnly hora, string turno)
    {
        return _context.ReservaCitas
            .Where(r => r.Fecha == fecha && r.Hora == hora && r.Turno == turno && r.Estado == "Pendiente")
            .Select(r => r.IdEstacion)
            .ToList();
    }
}
