using Aplicacion.Interfaces;
using Aplicacion.DTOs;
using Infraestructura.Modelos;

namespace Aplicacion.Servicios
{
    public class RegistroServicio : IRegistroServicio
    {
        private readonly IRegistroRepositorio repo;

        public RegistroServicio(IRegistroRepositorio repo)
        {
            this.repo = repo;
        }

        public Registro AddUsuario(RegistroUsuarioDTO usuario)
        {
            try
            {
                var _usuario = new Registro
                {
                    Nombre = usuario.Nombre,
                    Edad = usuario.Edad,
                    Telefono = usuario.Telefono,
                    Correo = usuario.Correo,
                    Contraseña = usuario.Contraseña,
                    Dia = usuario.Dia,
                    Mes = usuario.Mes,
                    Año = usuario.Año,
                    Rol = false
                };

                repo.Registrarse(_usuario);
                return _usuario;
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo registrar: " + ex.Message, ex);
            }
        }

        
        public LoginUsuarioDTO ValidacionLogin(string correo, string contraseña)
        {
            try
            {
                var usuario = repo.Logearse(correo, contraseña);

                if (usuario == null)
                {
                    LoggerServicio.getInstancia().Error($"Error al registrar");
                    throw new Exception("Usuario o contraseña incorrecta, inténtelo de nuevo");
                }

                if (usuario.Rol && !usuario.Rol)
                {
                    LoggerServicio.getInstancia().Warning("Error de autenticacion");
                    throw new Exception("Error al autenticarte");
                }

                var usuarioLoguiado = new LoginUsuarioDTO
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre!,
                    Correo = usuario.Correo!,
                    Rol = usuario.Rol
                };

                LoggerServicio.getInstancia().Info($"{usuario.Nombre} se autentico correctamente el {DateTime.Now}");
                return usuarioLoguiado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al logearse " + ex.Message);
            }
        }
            
    }
}
    