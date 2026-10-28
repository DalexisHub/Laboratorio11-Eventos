using Eventos.Web.Data;
using Eventos.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventos.Web.Repositorios;
public class EventoRepositorio(EventosContext db)
{
    public Task<List<Evento>> ListarAsync(int? categoriaId) => db.Eventos
        .Where(e => e.Activo && (!categoriaId.HasValue || e.CategoriaId == categoriaId))
        .Include(e => e.Categoria).Include(e => e.Inscripciones)
        .AsNoTracking().OrderBy(e => e.Fecha).ToListAsync();
    public Task<Evento?> BuscarAsync(int id) => db.Eventos.Include(e => e.Categoria)
        .Include(e => e.Inscripciones).AsNoTracking().FirstOrDefaultAsync(e => e.Id == id && e.Activo);
    public Task<List<Categoria>> CategoriasActivasAsync() => db.Categorias.Where(c => c.Activo)
        .AsNoTracking().OrderBy(c => c.Nombre).ToListAsync();
    public Task<bool> CategoriaActivaAsync(int id) => db.Categorias.AsNoTracking()
        .AnyAsync(c => c.Id == id && c.Activo);
    public async Task CrearAsync(Evento evento)
    {
        db.Eventos.Add(evento);
        await db.SaveChangesAsync();
    }
    public async Task<bool> ActualizarAsync(Evento nuevo)
    {
        var actual = await db.Eventos.FirstOrDefaultAsync(e => e.Id == nuevo.Id && e.Activo);
        if (actual is null) return false;
        actual.Titulo = nuevo.Titulo;
        actual.Fecha = nuevo.Fecha;
        actual.Lugar = nuevo.Lugar;
        actual.Aforo = nuevo.Aforo;
        actual.Precio = nuevo.Precio;
        actual.Modalidad = nuevo.Modalidad;
        actual.CategoriaId = nuevo.CategoriaId;
        await db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DesactivarAsync(int id)
    {
        var evento = await db.Eventos.FirstOrDefaultAsync(e => e.Id == id && e.Activo);
        if (evento is null) return false;
        evento.Activo = false;
        await db.SaveChangesAsync();
        return true;
    }
}
