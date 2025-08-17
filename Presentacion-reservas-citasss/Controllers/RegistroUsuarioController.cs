using System.Security.Claims;
using Aplicacion.DTOs;
using Aplicacion.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Presentacion_reservas_citasss.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroUsuarioController : ControllerBase
    {

        private readonly RegistroUsuarioServicio servicio;
        private readonly GeneracionTokenServicio token;



        public RegistroUsuarioController(RegistroUsuarioServicio servicio, GeneracionTokenServicio token) 
        {
        
            this.servicio = servicio;
            this.token = token; 
        
        }



        [AllowAnonymous]
        [HttpPost("Registro-usuario")]
        public dynamic RegistroUsuario(RegistroUsuarioDTO usuario)
        {


          
            var RegistroUsuario = servicio.AgregarUsuario(usuario);


           
            if (RegistroUsuario == null)
            {
                return BadRequest(new { message = "No se pudo registrar el usuario (puede que ya exista)" });
            }


            string id = Convert.ToString(usuario.Id);

            string _token = token.GenerarToken(usuario.Nombre, usuario.Apellido, usuario.Correo, id);




            return Ok(new
            {
                token = _token,
                usuario = RegistroUsuario
            });


        }




        [AllowAnonymous]
        [HttpPost("Login-usuario")]
        public dynamic LoginUsuario(string nombre, string contraseña)
        {

            try
            {

              
                

                var usuariodto = servicio.ValidarLoginUsuario(nombre, contraseña);


                if (usuariodto == null)
                {
                    return new
                    {
                        Success = false,
                        message = "Credenciales incorrectas",
                        result = ""

                    };


                    
                }


                string id = Convert.ToString(usuariodto.Id);
                string tokenString = token.GenerarToken(usuariodto.Nombre,usuariodto.Apellido,usuariodto.Correo, id);




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
