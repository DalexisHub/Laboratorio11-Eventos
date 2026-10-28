using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventos.Web.Models;
public class Evento
{
    public int Id { get; set; }
    [Required, StringLength(100)] public string Titulo { get; set; } = string.Empty;
    [Required] public DateTime Fecha { get; set; }
    [Required, StringLength(100)] public string Lugar { get; set; } = string.Empty;
    [Range(1, 5000)] public int Aforo { get; set; }
    [Range(0, double.MaxValue), Column(TypeName = "decimal(10,2)")] public decimal Precio { get; set; }
    [Required, StringLength(20), RegularExpression("^(Presencial|Virtual)$", ErrorMessage = "Elige Presencial o Virtual.")]
    public string Modalidad { get; set; } = "Presencial";
    [Range(1, int.MaxValue)] public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;
    public bool Activo { get; set; } = true;
    public List<Inscripcion> Inscripciones { get; set; } = new();
}
