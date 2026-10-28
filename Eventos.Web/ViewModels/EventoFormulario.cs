using System.ComponentModel.DataAnnotations;
using Eventos.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eventos.Web.ViewModels;
public class EventoFormulario
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Ingresa un título."), StringLength(100)]
    public string Titulo { get; set; } = string.Empty;
    [Required(ErrorMessage = "Indica la fecha.")]
    public DateTime Fecha { get; set; } = DateTime.Today.AddDays(1).AddHours(9);
    [Required(ErrorMessage = "Ingresa un lugar."), StringLength(100)]
    public string Lugar { get; set; } = string.Empty;
    [Range(1, 5000, ErrorMessage = "El aforo debe estar entre 1 y 5000.")]
    public int Aforo { get; set; } = 1;
    [Range(0, double.MaxValue, ErrorMessage = "El precio no puede ser negativo.")]
    public decimal Precio { get; set; }
    [Required, StringLength(20), RegularExpression("^(Presencial|Virtual)$", ErrorMessage = "Elige una modalidad.")]
    public string Modalidad { get; set; } = "Presencial";
    [Range(1, int.MaxValue, ErrorMessage = "Elige una categoría.")]
    public int CategoriaId { get; set; }
    public List<SelectListItem> Categorias { get; set; } = new();
    public Evento AEvento() => new()
    {
        Id = Id, Titulo = Titulo, Fecha = Fecha, Lugar = Lugar,
        Aforo = Aforo, Precio = Precio, CategoriaId = CategoriaId, Modalidad = Modalidad
    };
    public static EventoFormulario Desde(Evento e) => new()
    {
        Id = e.Id, Titulo = e.Titulo, Fecha = e.Fecha, Lugar = e.Lugar,
        Aforo = e.Aforo, Precio = e.Precio, CategoriaId = e.CategoriaId, Modalidad = e.Modalidad
    };
}
