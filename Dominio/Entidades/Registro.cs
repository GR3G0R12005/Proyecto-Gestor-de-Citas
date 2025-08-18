using System.ComponentModel.DataAnnotations;

namespace Infraestructura.Modelos;

public partial class Registro
{
    [Key]
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public int Edad { get; set; }
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
    public string? Contraseña { get; set; }
    public int Dia { get; set; }
    public int Mes { get; set; }
    public int Año { get; set; }
    public bool Rol { get; set; } = false;
    public virtual ICollection<ReservaCita> ReservaCita { get; set; } = new List<ReservaCita>();
}
