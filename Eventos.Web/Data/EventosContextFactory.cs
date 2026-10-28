using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Eventos.Web.Data;

// Permite generar migraciones sin iniciar la aplicación ni ejecutar la carga inicial.
public class EventosContextFactory : IDesignTimeDbContextFactory<EventosContext>
{
    public EventosContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Local.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
        var options = new DbContextOptionsBuilder<EventosContext>()
            .UseSqlServer(configuration.GetConnectionString("EventosDB"))
            .Options;
        return new EventosContext(options);
    }
}
