# PruebaTekus

Backend ASP.NET Core (Onion/Clean Architecture + CQRS) y frontend Angular con tema de marca
Tekus, listo para iterar sobre el dominio real del proyecto.

## Estructura

```
backend/    .NET solution (Onion Architecture, CQRS + MediatR)
frontend/   Angular standalone + Angular Material (tema Tekus)
docker-compose.yml   levanta SQL Server + backend + frontend con `docker compose up --build`
```

## Backend (`backend/`)

Solución `PruebaTekus.slnx` con capas:

- `src/Domain` — `PruebaTekus.Domain`: entidades, value objects, reglas de negocio. Sin dependencias a otros proyectos.
- `src/Application` — `PruebaTekus.Application`: casos de uso (Commands/Queries + Handlers vía MediatR), interfaces de repositorios/servicios. Depende solo de `Domain`.
- `src/Infrastructure` — `PruebaTekus.Infrastructure`: implementaciones concretas (EF Core + SQL Server, repos, servicios externos). Depende de `Application` + `Domain`.
- `src/Api` — `PruebaTekus.Api`: ASP.NET Core Web API (controllers), composición/DI. Depende de las tres anteriores.
- `tests/Application.Tests` — xUnit, apunta a `Application` + `Domain`.

Onion Architecture: Domain en el centro, capas concéntricas hacia afuera.
Regla de dependencia: `Domain ← Application ← Infrastructure`, y `Api` amarra todo vía DI en `Program.cs`.

**CQRS con MediatR**: `Application` se organiza en Commands/Queries + Handlers (no services
tradicionales con métodos CRUD genéricos). Cada caso de uso es su propio Command/Query + Handler,
con su propio DTO de request/response. Ejemplo de referencia ya implementado en
`src/Application/Products` (`CreateProductCommand` + `GetProductByIdQuery`) — usar como plantilla
para nuevos casos de uso.

Persistencia: **SQL Server** vía EF Core en `Infrastructure` (connection string en
`Api/appsettings.Development.json`, no trackeado en git — ver sección Git más abajo). `DbContext`
en `Infrastructure/Persistence`, migraciones en `Infrastructure/Persistence/Migrations`.
`Program.cs` aplica `dbContext.Database.Migrate()` al arrancar, la DB queda lista sola.
`UseInMemoryDatabase`/SQLite solo aceptable para tests, no para el resultado final.

API documentada con **Swashbuckle** (`/swagger`), usando XML doc comments de los controllers
(`GenerateDocumentationFile` habilitado en `Api.csproj`).

Nota de build: `PruebaTekus.Api.csproj` tiene `AllowMissingPrunePackageData=true` — workaround
para un warning NETSDK1226 del SDK .NET 10 instalado en esta máquina, no afecta runtime.

## Frontend (`frontend/`)

Angular (standalone components, sin NgModules), routing habilitado, SCSS, test runner **vitest**
(default de Angular CLI reciente, no Karma/Jasmine).

- Angular Material instalado con **Material 3 theming** custom, generado desde los colores de marca Tekus:
  - Primary: `#5BB09F` (verde azulado / teal)
  - Secondary: `#16006F` (azul oscuro / navy)
  - Paleta completa en `frontend/_theme-colors.scss` (generado con `ng generate @angular/material:theme-color`, no editar a mano — regenerar si cambian los colores de marca).
  - Aplicado en `frontend/src/styles.scss` vía `mat.theme(...)`.
- Logo oficial descargado en `frontend/public/branding/tekus-logo.png` (fondo negro — usar sobre fondo claro con precaución o recortar/generar versión sobre fondo transparente si se necesita en header claro).

Si los requerimientos piden componentes que Material no cubre bien (tablas complejas,
formularios dinámicos), evaluar agregar algo puntual — no reemplazar Material por completo salvo
que sea necesario.

## Git

Repo inicializado en la raíz (`PruebaTekus/`), un solo `.gitignore` raíz + el `.gitignore` propio
de `frontend/` (generado por Angular CLI, se respeta tal cual).

**Nunca subir datos sensibles a GitHub, bajo ninguna circunstancia.** Esto incluye credenciales,
connection strings reales con contraseñas de producción, API keys, tokens, certificados, secretos
de cualquier tipo. Antes de cualquier `git add`/`commit`/`push`, revisar que no se esté incluyendo
nada de esto — usar `appsettings.Development.json` / user-secrets / variables de entorno para
config local sensible, y mantenerlos fuera del repo (`.gitignore`). Si algo sensible ya se
commiteó, avisar de inmediato en vez de simplemente sobrescribirlo.

## Convenciones

- **Idioma**: todo lo que va dentro de la app (nombres de clases, métodos, variables,
  namespaces, comentarios en código, commits, nombres de componentes/servicios Angular,
  rutas de API) va en **inglés**. La conversación con el usuario (Claude ↔ persona) va en
  **español**. No mezclar: código en inglés, chat en español.
- **Principios SOLID**, aplicados sin sobre-ingeniería:
  - **S**ingle Responsibility: cada clase/servicio hace una cosa (ej. un `OrderService` no
    valida input, persiste y envía email a la vez — separar en casos de uso pequeños).
  - **O**pen/Closed: nuevas reglas de negocio se agregan extendiendo (nueva clase, nueva
    implementación de interfaz), no editando a mano un `switch`/`if` gigante existente.
  - **L**iskov: cualquier implementación de una interfaz de `Application` (repos, servicios)
    debe poder sustituir a otra sin romper el caso de uso que la consume.
  - **I**nterface Segregation: interfaces chicas y específicas (`IOrderRepository`, no un
    `IRepository` gigante con todo el CRUD de todas las entidades si no aplica).
  - **D**ependency Inversion: `Application` define las interfaces, `Infrastructure` las
    implementa, `Api` inyecta por DI. `Domain`/`Application` nunca referencian
    `Infrastructure` directamente.
- **Sin comentarios en el código**, en ninguna capa (ni explicativos ni de sección). Código
  autoexplicativo vía nombres claros. Excepción: **XML doc comments** (`/// <summary>`) en
  controllers/endpoints para generar documentación Swagger/OpenAPI — no son comentarios
  explicativos de lógica, son metadata de API.
- **Tests con patrón AAA** (Arrange-Act-Assert): estructurar cada test en tres bloques separados
  por línea en blanco, sin comentarios `// Arrange` etc. (para no violar la regla anterior).
- Priorizar que compile y pase lo esencial sobre cobertura exhaustiva de tests, pero no
  saltarse tests cuando los requerimientos los pidan explícitamente.
- Comandos para correr backend/frontend: ver `README.md`.
