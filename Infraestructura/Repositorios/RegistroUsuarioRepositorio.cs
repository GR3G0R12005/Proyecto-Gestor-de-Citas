using Aplicacion.Interfaces;
using Infraestructura.Contexto;
using Infraestructura.Modelos;

namespace Infraestructura.Repositorios
{
    public class RegistroRepositorio : IRegistroRepositorio
    {
        private readonly ReservaCitasDbContext context;

        public RegistroRepositorio(ReservaCitasDbContext context)
        {
            this.context = context;
        }

        public string Registrarse(Registro usuario)
        {
            context.RegistroUsuarios.Add(usuario);
            context.SaveChanges();

            return "Registrado correctamente";
        }
        
        public Registro Logearse(string correo, string contraseña)
        {
            var usuario = context.RegistroUsuarios.FirstOrDefault(u => u.Correo == correo && u.Contraseña == contraseña);

            if (usuario == null)
            {

                throw new Exception("El usuario no esta registrados");
            }
            return usuario;
        }

        public int? BuscarId(string correo)
        {
            var user = context.RegistroUsuarios.FirstOrDefault(u => u.Correo == correo);

            if (user == null)
            {

                return null;
            }
            return user.Id;
        }

        public bool BuscarUsuario(int id)
        {
            var usuario = context.RegistroUsuarios.FirstOrDefault(x => x.Id == id);

            if (usuario == null)
            {
                return false;
            }
            return true;
        }
    }
}
