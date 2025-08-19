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
        public IActionResult RegistroUsuario([FromBody] RegistroUsuarioDTO usuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario.Nombre) ||
                    string.IsNullOrWhiteSpace(usuario.Correo) ||
                    string.IsNullOrWhiteSpace(usuario.Contraseña) ||
                    usuario.Edad <= 0 ||
                    usuario.Dia <= 0 || usuario.Mes <= 0 || usuario.Año <= 0)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Faltan campos obligatorios o contienen valores inválidos.",
                        usuario = (object)null,
                        token = ""
                    });
                }

                var registroUsuario = servicio.AddUsuario(usuario);

                if (registroUsuario == null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "No se pudo registrar el usuario. Verifica los datos.",
                        usuario = (object)null,
                        token = ""
                    });
                }

                string id = registroUsuario.Id.ToString();
                string _token = token.GenerarToken(registroUsuario.Nombre!, registroUsuario.Correo!, id);

                return Ok(new
                {
                    success = true,
                    message = "Usuario registrado correctamente",
                    token = _token,
                    usuario = new { rol = registroUsuario.Rol, nombre = registroUsuario.Nombre }
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    message = "Error interno del servidor",
                    error = ex.Message,
                    usuario = (object)null,
                    token = ""
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("Logearse")]
        public IActionResult LoginUsuario([FromBody] LoginDTO login)
        {
            try
            {
                var usuariodto = servicio.ValidacionLogin(login.Correo, login.Contraseña);

                if (usuariodto == null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Usuario o contraseña incorrectos",
                        usuario = (object)null,
                        token = ""
                    });
                }

                string id = Convert.ToString(usuariodto.Id);
                string tokenString = token.GenerarToken(usuariodto.Nombre, usuariodto.Correo, id);

                return Ok(new
                {
                    success = true,
                    token = tokenString,
                    usuario = new { rol = usuariodto.Rol, nombre = usuariodto.Nombre }
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    message = "Error interno del servidor",
                    error = ex.Message,
                    usuario = (object)null,
                    token = ""
                });
            }
        }
    }
}
