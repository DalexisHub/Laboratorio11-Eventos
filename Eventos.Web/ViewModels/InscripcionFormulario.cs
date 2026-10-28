using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eventos.Web.ViewModels;
public class InscripcionFormulario
{
    [Range(1, int.MaxValue, ErrorMessage = "Elige un evento.")]
    public int EventoId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Elige un asistente.")]
    public int AsistenteId { get; set; }
    public List<SelectListItem> Eventos { get; set; } = new();
    public List<SelectListItem> Asistentes { get; set; } = new();
}
