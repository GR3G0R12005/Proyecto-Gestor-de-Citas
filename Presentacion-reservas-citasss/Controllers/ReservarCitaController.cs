using Aplicacion.DTOs;
using Aplicacion.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentacion_reservas_citasss.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReservarCitaController : ControllerBase
    {
        private readonly ReservaCitasServicio servicio;

        public ReservarCitaController(ReservaCitasServicio servicio)
        {
            this.servicio = servicio;
        }

        // 📌 Registrar una nueva reserva
        [HttpPost("Registrar-Reserva")]
        public async Task<IActionResult> ReservarCita([FromBody] ReservaCitasDTO dto)
        {
            try
            {
                var correo = User.FindFirstValue("Correo") ?? throw new Exception("Correo no encontrado");
                var nombre = User.FindFirstValue("Nombre") ?? throw new Exception("Nombre no encontrado");
                int idUsuario = int.Parse(User.FindFirstValue("id") ?? throw new Exception("Usuario no autenticado"));

                var resultado = await servicio.ReservarCita(correo, nombre, idUsuario, dto);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

     
        [HttpGet("Ver-mi-Reserva-Activa")]
        public IActionResult VerReservaActiva()
        {
            try
            {
                int idUsuario = int.Parse(User.FindFirstValue("id") ?? throw new Exception("Usuario no autenticado"));
                string nombre = User.FindFirstValue("Nombre") ?? throw new Exception("Nombre no encontrado");

                var reserva = servicio.GetReservaCita(nombre, idUsuario);
                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 📌 Ver TODAS mis reservas
        [HttpGet("Ver-mis-Reservas")]
        public IActionResult VerReservas()
        {
            try
            {
                int idUsuario = int.Parse(User.FindFirstValue("id") ?? throw new Exception("Usuario no autenticado"));
                string nombre = User.FindFirstValue("Nombre") ?? throw new Exception("Nombre no encontrado");

                var reservas = servicio.GetReservasUsuario(idUsuario, nombre);
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
