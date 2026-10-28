using System.ComponentModel.DataAnnotations;
using Eventos.Web.Models;

namespace Eventos.Web.ViewModels;

public class AsistenteFormulario
{
    [Required(ErrorMessage = "Ingresa los nombres."), StringLength(100)]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa el DNI.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener exactamente 8 dígitos.")]
    public string Dni { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa el correo."), EmailAddress(ErrorMessage = "Ingresa un correo válido.")]
    public string Correo { get; set; } = string.Empty;

    [StringLength(9, ErrorMessage = "El teléfono admite hasta 9 caracteres.")]
    public string? Telefono { get; set; }

    public Asistente AAsistente() => new()
    {
        Nombres = Nombres, Dni = Dni, Correo = Correo, Telefono = Telefono
    };
}
