using Aplicacion.DTOs;
using Aplicacion.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion_reservas_citasss.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroUsuarioController : ControllerBase
    {
        private readonly RegistroServicio servicio;
        private readonly GeneracionTokenServicio token;

        public RegistroUsuarioController(RegistroServicio servicio, GeneracionTokenServicio token) 
        {
            this.servicio = servicio;
            this.token = token; 
        }

        [AllowAnonymous]
        [HttpPost("Registrarse")]
        public dynamic RegistroUsuario(RegistroUsuarioDTO usuario)
        {

            var RegistroUsuario = servicio.AddUsuario(usuario);

            if (RegistroUsuario == null)
            {
                return BadRequest(new { message = "No se pudo registrar el usuario (puede que ya exista)" });
            }

            string id = Convert.ToString(usuario.Id);

            string _token = token.GenerarToken(usuario.Nombre, usuario.Correo, id);

            return Ok(new
            {
                token = _token,
                usuario = RegistroUsuario
            });
        }

        [AllowAnonymous]
        [HttpPost("Logearse")]
        public dynamic LoginUsuario(string correo, string contraseña)
        {
            try
            {           
                var usuariodto = servicio.ValidacionLogin(correo, contraseña);

                if (usuariodto == null)
                {
                    return new
                    {
                        Success = false,
                        message = "Error en credenciales, probablemente incorrecta",
                        result = ""
                    };
                }

                string id = Convert.ToString(usuariodto.Id);
                string tokenString = token.GenerarToken(usuariodto.Nombre, usuariodto.Correo, id);

                return Ok(new
                {
                    token = tokenString,
                    usuario = usuariodto
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
