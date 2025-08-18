using Infraestructura.Modelos;

namespace Aplicacion.Interfaces
{
    public interface IRegistroRepositorio
    {
        string Registrarse(Registro usuario);
        Registro Logearse(string correo, string contraseña);
        int? BuscarId(string correo);
        bool BuscarUsuario(int id);
    }
}
