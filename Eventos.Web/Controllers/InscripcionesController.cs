using Eventos.Web.Models;
using Eventos.Web.Repositorios;
using Eventos.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eventos.Web.Controllers;
public class InscripcionesController(InscripcionRepositorio repositorio) : Controller
{
    public async Task<IActionResult> Index() => View(await repositorio.ListarAsync());
    public async Task<IActionResult> Create()
    {
        var modelo = new InscripcionFormulario();
        await CargarListas(modelo);
        return View(modelo);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(InscripcionFormulario modelo)
    {
        var evento = await repositorio.EventoActivoAsync(modelo.EventoId);
        if (evento is null)
            ModelState.AddModelError(nameof(modelo.EventoId), "Elige un evento activo.");
        else
        {
            if (evento.Fecha <= DateTime.Now)
                ModelState.AddModelError(nameof(modelo.EventoId), "El evento ya pasó.");
            if (await repositorio.ConfirmadosAsync(evento.Id) >= evento.Aforo)
                ModelState.AddModelError(nameof(modelo.EventoId), "No quedan cupos disponibles.");
        }
        if (!await repositorio.AsistenteActivoAsync(modelo.AsistenteId))
            ModelState.AddModelError(nameof(modelo.AsistenteId), "Elige un asistente activo.");
        else if (evento is not null && await repositorio.YaConfirmadoAsync(evento.Id, modelo.AsistenteId))
            ModelState.AddModelError(nameof(modelo.AsistenteId), "El asistente ya tiene una inscripción confirmada.");
        if (!ModelState.IsValid)
        {
            await CargarListas(modelo);
            return View(modelo);
        }
        var inscripcion = new Inscripcion
        {
            EventoId = modelo.EventoId, AsistenteId = modelo.AsistenteId,
            FechaInscripcion = DateTime.Now, Estado = "Confirmada"
        };
        await repositorio.CrearAsync(inscripcion);
        TempData["ConfirmacionId"] = inscripcion.Id;
        return RedirectToAction(nameof(Index));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancelar(int id)
    {
        if (!await repositorio.CancelarAsync(id)) return NotFound();
        TempData["Mensaje"] = "Inscripción cancelada.";
        return RedirectToAction(nameof(Index));
    }
    private async Task CargarListas(InscripcionFormulario modelo)
    {
        modelo.Eventos = (await repositorio.EventosActivosAsync())
            .Select(e => new SelectListItem(e.Titulo, e.Id.ToString())).ToList();
        modelo.Asistentes = (await repositorio.AsistentesActivosAsync())
            .Select(a => new SelectListItem(a.Nombres, a.Id.ToString())).ToList();
    }
}
