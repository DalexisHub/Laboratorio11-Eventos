using Eventos.Web.Data;
using Eventos.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventos.Web.Repositorios;
public class AsistenteRepositorio(EventosContext db)
{
    public Task<List<Asistente>> ListarAsync() => db.Asistentes.Where(a => a.Activo)
        .AsNoTracking().OrderBy(a => a.Nombres).ToListAsync();
    public Task<bool> DniExisteAsync(string dni) => db.Asistentes.AsNoTracking().AnyAsync(a => a.Dni == dni);
    public Task<bool> CorreoExisteAsync(string correo) => db.Asistentes.AsNoTracking().AnyAsync(a => a.Correo == correo);
    public async Task CrearAsync(Asistente asistente)
    {
        db.Asistentes.Add(asistente);
        await db.SaveChangesAsync();
    }
}
