using Eventos.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Eventos.Web.Data;
public static class DatosIniciales
{
    public static async Task CargarAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<EventosContext>();
        if (!await db.Categorias.AnyAsync())
        {
            db.Categorias.AddRange(
                new Categoria { Nombre = "Tecnología" },
                new Categoria { Nombre = "Cultura" },
                new Categoria { Nombre = "Negocios" });
            await db.SaveChangesAsync();
        }
        if (!await db.Eventos.AnyAsync())
        {
            var categorias = await db.Categorias.AsNoTracking().ToDictionaryAsync(c => c.Nombre, c => c.Id);
            db.Eventos.AddRange(
                new Evento { Titulo = "Foro de innovación", Fecha = DateTime.Today.AddDays(30).AddHours(9), Lugar = "Centro de convenciones", Aforo = 120, Precio = 40, CategoriaId = categorias["Tecnología"], Modalidad = "Presencial" },
                new Evento { Titulo = "Encuentro cultural", Fecha = DateTime.Today.AddDays(45).AddHours(18), Lugar = "Teatro municipal", Aforo = 180, Precio = 25, CategoriaId = categorias["Cultura"], Modalidad = "Presencial" },
                new Evento { Titulo = "Ideas que emprenden", Fecha = DateTime.Today.AddDays(60).AddHours(10), Lugar = "Auditorio central", Aforo = 90, Precio = 0, CategoriaId = categorias["Negocios"], Modalidad = "Virtual" },
                new Evento { Titulo = "Taller de desarrollo web", Fecha = DateTime.Today.AddDays(75).AddHours(15), Lugar = "Laboratorio digital", Aforo = 35, Precio = 30, CategoriaId = categorias["Tecnología"], Modalidad = "Virtual" });
            await db.SaveChangesAsync();
        }
        if (!await db.Asistentes.AnyAsync())
        {
            db.Asistentes.AddRange(
                new Asistente { Nombres = "Ana Torres", Dni = "71234567", Correo = "ana.torres@ejemplo.com" },
                new Asistente { Nombres = "Luis García", Dni = "72345678", Correo = "luis.garcia@ejemplo.com" },
                new Asistente { Nombres = "María Rojas", Dni = "73456789", Correo = "maria.rojas@ejemplo.com" },
                new Asistente { Nombres = "Carlos Vega", Dni = "74567890", Correo = "carlos.vega@ejemplo.com" },
                new Asistente { Nombres = "Sofía Díaz", Dni = "75678901", Correo = "sofia.diaz@ejemplo.com" });
            await db.SaveChangesAsync();
        }
    }
}
