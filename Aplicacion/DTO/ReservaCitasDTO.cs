using System;

namespace Aplicacion.DTOs
{
    public class ReservaCitasDTO
    {   
        public DateOnly Fecha { get; set; }

        public string? Hora { get; set; }

        public string Turno { get; set; } = null!;

        // Cambiado a propiedad para serialización correcta
        public string Estado { get; set; } = "Pendiente";
    }
}
