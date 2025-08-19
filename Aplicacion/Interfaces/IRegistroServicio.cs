using Aplicacion.DTOs;
using Infraestructura.Modelos;

namespace Aplicacion.Interfaces
{
    public interface IRegistroServicio
    {
        LoginUsuarioDTO ValidacionLogin(string correo, string contraseña);
        Registro AddUsuario(RegistroUsuarioDTO usuario);
    }
}
