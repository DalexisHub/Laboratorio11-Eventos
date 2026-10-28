using Eventos.Web.Repositorios;
using Eventos.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Eventos.Web.Controllers;
public class AsistentesController(AsistenteRepositorio repositorio) : Controller
{
    public async Task<IActionResult> Index() => View(await repositorio.ListarAsync());
    public IActionResult Create() => View(new AsistenteFormulario());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AsistenteFormulario asistente)
    {
        if (await repositorio.DniExisteAsync(asistente.Dni))
            ModelState.AddModelError(nameof(asistente.Dni), "El DNI ya está registrado.");
        if (await repositorio.CorreoExisteAsync(asistente.Correo))
            ModelState.AddModelError(nameof(asistente.Correo), "El correo ya está registrado.");
        if (!ModelState.IsValid) return View(asistente);
        await repositorio.CrearAsync(asistente.AAsistente());
        TempData["Mensaje"] = "Asistente creado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
