using Eventos.Web.Repositorios;
using Eventos.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eventos.Web.Controllers;
public class EventosController(EventoRepositorio repositorio) : Controller
{
    public async Task<IActionResult> Index(int? categoriaId)
    {
        ViewBag.CategoriaId = categoriaId;
        ViewBag.Categorias = await repositorio.CategoriasActivasAsync();
        return View(await repositorio.ListarAsync(categoriaId));
    }
    public async Task<IActionResult> Details(int id)
    {
        var evento = await repositorio.BuscarAsync(id);
        return evento is null ? NotFound() : View(evento);
    }
    public async Task<IActionResult> Create()
    {
        var modelo = new EventoFormulario();
        await CargarCategorias(modelo);
        return View(modelo);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EventoFormulario modelo)
    {
        if (!await repositorio.CategoriaActivaAsync(modelo.CategoriaId))
            ModelState.AddModelError(nameof(modelo.CategoriaId), "Elige una categoría activa.");
        if (!ModelState.IsValid)
        {
            await CargarCategorias(modelo);
            return View(modelo);
        }
        await repositorio.CrearAsync(modelo.AEvento());
        TempData["Mensaje"] = "Evento creado correctamente.";
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id)
    {
        var evento = await repositorio.BuscarAsync(id);
        if (evento is null) return NotFound();
        var modelo = EventoFormulario.Desde(evento);
        await CargarCategorias(modelo);
        return View(modelo);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EventoFormulario modelo)
    {
        if (id != modelo.Id) return BadRequest();
        if (!await repositorio.CategoriaActivaAsync(modelo.CategoriaId))
            ModelState.AddModelError(nameof(modelo.CategoriaId), "Elige una categoría activa.");
        if (!ModelState.IsValid)
        {
            await CargarCategorias(modelo);
            return View(modelo);
        }
        if (!await repositorio.ActualizarAsync(modelo.AEvento())) return NotFound();
        TempData["Mensaje"] = "Evento actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id)
    {
        var evento = await repositorio.BuscarAsync(id);
        return evento is null ? NotFound() : View(evento);
    }
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!ModelState.IsValid) return BadRequest();
        if (!await repositorio.DesactivarAsync(id)) return NotFound();
        TempData["Mensaje"] = "Evento desactivado.";
        return RedirectToAction(nameof(Index));
    }
    private async Task CargarCategorias(EventoFormulario modelo)
    {
        modelo.Categorias = (await repositorio.CategoriasActivasAsync())
            .Select(c => new SelectListItem(c.Nombre, c.Id.ToString())).ToList();
    }
}
