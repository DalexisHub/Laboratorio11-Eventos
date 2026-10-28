using Eventos.Web.Data;
using Eventos.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventos.Web.Repositorios;
public class InscripcionRepositorio(EventosContext db)
{
    public Task<List<Inscripcion>> ListarAsync() => db.Inscripciones.Include(i => i.Evento)
        .Include(i => i.Asistente).AsNoTracking().OrderByDescending(i => i.FechaInscripcion).ToListAsync();
    public Task<List<Evento>> EventosActivosAsync() => db.Eventos.Where(e => e.Activo)
        .AsNoTracking().OrderBy(e => e.Fecha).ToListAsync();
    public Task<List<Asistente>> AsistentesActivosAsync() => db.Asistentes.Where(a => a.Activo)
        .AsNoTracking().OrderBy(a => a.Nombres).ToListAsync();
    public Task<Evento?> EventoActivoAsync(int id) => db.Eventos.AsNoTracking()
        .FirstOrDefaultAsync(e => e.Id == id && e.Activo);
    public Task<bool> AsistenteActivoAsync(int id) => db.Asistentes.AsNoTracking()
        .AnyAsync(a => a.Id == id && a.Activo);
    public Task<bool> YaConfirmadoAsync(int eventoId, int asistenteId) => db.Inscripciones.AsNoTracking()
        .AnyAsync(i => i.EventoId == eventoId && i.AsistenteId == asistenteId && i.Estado == "Confirmada");
    public Task<int> ConfirmadosAsync(int eventoId) => db.Inscripciones.AsNoTracking()
        .CountAsync(i => i.EventoId == eventoId && i.Estado == "Confirmada");
    public async Task CrearAsync(Inscripcion inscripcion)
    {
        db.Inscripciones.Add(inscripcion);
        await db.SaveChangesAsync();
    }
    public async Task<bool> CancelarAsync(int id)
    {
        var inscripcion = await db.Inscripciones.FirstOrDefaultAsync(i => i.Id == id && i.Estado == "Confirmada");
        if (inscripcion is null) return false;
        inscripcion.Estado = "Cancelada";
        await db.SaveChangesAsync();
        return true;
    }
}
