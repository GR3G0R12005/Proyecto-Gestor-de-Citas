using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.DTOs;
using Aplicacion.Interfaces;
using Infraestructura.Modelos;

namespace Aplicacion.Servicios
{
    public class ReservaCitasServicio : IReservaCitasServicio
    {
        private readonly IReservaCitaRepositorio _repo;

        public ReservaCitasServicio(IReservaCitaRepositorio repo)
        {
            _repo = repo;
        }

        // Registrar una nueva cita
        public async Task<string> ReservarCita(string correo, string nombre, int idUsuario, ReservaCitasDTO reservaModel)
        {
            try
            {
                if (string.IsNullOrEmpty(reservaModel.Turno))
                    throw new ArgumentException("El campo turno no puede estar vacío");

                if (reservaModel.Turno != "Matutino" && reservaModel.Turno != "Vespertino")
                    throw new ArgumentException("El turno debe ser válido");

                if (reservaModel.Fecha < DateOnly.FromDateTime(DateTime.Now))
                    throw new ArgumentException("La fecha no puede ser menor a la fecha actual");

                // 🔹 Ahora valida por fecha + hora + turno
                if (_repo.CitaActiva(idUsuario, reservaModel.Fecha, reservaModel.Hora!, reservaModel.Turno))
                    throw new ArgumentException("Ya tiene una reserva en esa fecha, turno y hora");

                var config = _repo.obtenerConfiguracionPorTurno(reservaModel.Fecha, reservaModel.Turno);
                if (config == null)
                    throw new ArgumentException("Configuración no encontrada");

                var horaInput = TimeOnly.ParseExact(reservaModel.Hora!, "HH:mm", CultureInfo.InvariantCulture);

                var nuevaReserva = new ReservaCita
                {
                    IdUsuario = idUsuario,
                    Turno = reservaModel.Turno,
                    Fecha = reservaModel.Fecha,
                    Hora = horaInput,
                    Estado = "Pendiente"
                };

                _repo.ReservarCita(nuevaReserva);

                LoggerServicio.getInstancia().Info($"Usuario {nombre} reservó una cita para {reservaModel.Fecha} (turno:{reservaModel.Turno})");

                return "Reserva registrada correctamente";
            }
            catch (Exception ex)
            {
                LoggerServicio.getInstancia().Error($"Error al registrar reserva para {nombre}: {ex.Message}");
                return "Error al registrar la reserva: " + ex.Message;
            }
        }

        // Obtener la reserva activa más reciente
        public ReservaCitasDTO GetReservaCita(string nombre, int idUsuario)
        {
            var cita = _repo.GetReservaCita(idUsuario);
            if (cita == null)
                throw new Exception("No tiene reservas activas");

            return new ReservaCitasDTO
            {
                Fecha = cita.Fecha,
                Turno = cita.Turno,
                Hora = cita.Hora?.ToString("HH:mm"),
                Estado = cita.Estado
            };
        }

        // Obtener todas las reservas de un usuario
        public List<ReservaCitasDTO> GetReservasUsuario(int idUsuario, string nombre)
        {
            var reservas = _repo.GetReservasUsuario(idUsuario);
            if (reservas == null || reservas.Count == 0)
                throw new Exception("El usuario no tiene reservas.");

            return reservas.Select(c => new ReservaCitasDTO
            {
                Fecha = c.Fecha,
                Turno = c.Turno,
                Hora = c.Hora?.ToString("HH:mm"),
                Estado = c.Estado
            }).ToList();
        }
    }
}
