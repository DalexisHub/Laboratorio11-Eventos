# Encuentro · Laboratorio semana 11

Proyecto ASP.NET Core MVC (`Eventos.Web`) con Entity Framework Core Code First y SQL Server. La guía Word y la presentación de la semana están en la carpeta raíz como referencias.

## Preparación local

El proyecto usa .NET SDK 10.0.401, `dotnet-ef` 10.0.13, `Microsoft.EntityFrameworkCore.SqlServer` 10.0.13 y `Microsoft.EntityFrameworkCore.Design` 10.0.13.

La conexión pública de `Eventos.Web/appsettings.json` apunta a `Database=EventosDB` en `127.0.0.1,14333` y confía en el certificado de desarrollo. Las credenciales se guardan en `Eventos.Web/appsettings.Local.json`, excluido de Git. Si usas otro SQL Server, ajusta solo ese archivo local o usa configuración local equivalente. Nunca publiques una contraseña.

El servidor SQL Server de `Biblioteca.Web/compose.yaml` se inició con:

```bash
docker compose -f /Users/alexis/Documents/Biblioteca.Web/compose.yaml up -d sqlserver
```

No se ejecutaron migraciones ni comandos de datos contra Biblioteca. `EventosDB` se creó exclusivamente con migraciones.

## Migraciones ejecutadas

Desde `Eventos.Web`:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run --no-build -- --seed-only
dotnet ef migrations add AgregarModalidadEvento
# En la migración generada se cambió solo defaultValue a "Presencial".
dotnet ef database update
dotnet ef migrations add AgregarTelefonoAsistente
dotnet ef database update
dotnet ef migrations list
```

`--seed-only` insertó 3 categorías, 4 eventos futuros y 5 asistentes después de `InitialCreate` y terminó antes de iniciar el servidor web. La carga solo inserta cuando cada tabla está vacía. Los archivos generados y `EventosContextModelSnapshot.cs` están en `Eventos.Web/Migrations`.

Para capturar la evidencia SQL pedida en la guía, conecta la extensión SQL Server a **EventosDB** y ejecuta una consulta de lectura:

```sql
SELECT MigrationId, ProductVersion FROM __EFMigrationsHistory ORDER BY MigrationId;
```

Los tres identificadores coinciden con los nombres de los archivos de migración. Las capturas listas para adjuntar están en [Evidencias](Evidencias/README.md).

## Ejecutar

```bash
cd Eventos.Web
dotnet restore
dotnet ef database update
dotnet run
```

Abre [http://127.0.0.1:5071](http://127.0.0.1:5071). La página inicial es Eventos. El puerto está fijado en `Properties/launchSettings.json`; si una pestaña anterior apunta a otro puerto, usa esta dirección. Antes de arrancar, confirma que SQL Server esté iniciado y que `appsettings.Local.json` contenga la conexión válida. No es necesario volver a ejecutar `--seed-only`: la aplicación comprueba las tablas al iniciar y solo carga los datos cuando están vacías.

## Comprobaciones manuales para el estudiante

1. En Eventos, confirma que aparecen 4 eventos, la categoría y modalidad, el orden por fecha, el precio y los cupos. Prueba el filtro por categoría y que mantiene la selección.
2. Crea y edita un evento. Prueba campos obligatorios, aforo fuera de 1–5000, precio negativo y categorías no válidas. Revisa la página de detalles y la desactivación lógica.
3. En Asistentes, confirma los 5 registros iniciales. Crea uno con teléfono y otro sin teléfono; verifica el límite de 9 caracteres, DNI de 8 dígitos y correo válido. Intenta repetir DNI y correo y comprueba los mensajes.
4. Crea una inscripción válida. Verifica el estado Confirmada y el descenso del cupo disponible en Eventos. Intenta registrar al mismo asistente otra vez, un evento pasado y uno sin cupos.
5. Cancela una inscripción mediante el botón de la lista; comprueba el estado Cancelada y que el cupo vuelve a estar disponible. Registra de nuevo al mismo asistente y confirma que se permite tras cancelar.
6. En SQL Server, captura las tres filas de `__EFMigrationsHistory` y la carpeta `Migrations` con sus archivos Designer y snapshot.

El proyecto fue compilado y las migraciones se aplicaron; las pantallas y los flujos funcionales quedan para estas comprobaciones manuales.
