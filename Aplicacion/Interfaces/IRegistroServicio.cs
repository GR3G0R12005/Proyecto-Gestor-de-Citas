using Aplicacion.DTOs;

namespace Aplicacion.Interfaces
{
    public interface IRegistroServicio
    {
        LoginUsuarioDTO ValidacionLogin(string correo, string contraseña);
        string AddUsuario(RegistroUsuarioDTO usuario);
    }
}
