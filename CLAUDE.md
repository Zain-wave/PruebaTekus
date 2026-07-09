# PruebaTekus — Prueba técnica ASP.NET / Angular (Tekus)

Repo de preparación para prueba técnica de Tekus. El enunciado real del problema
se entrega el día de la prueba; este repo trae el ambiente ya listo (backend,
frontend, tema de marca, repo git) para no perder tiempo en setup.

## Estructura

```
backend/    .NET solution (Clean Architecture / DDD-friendly)
frontend/   Angular standalone + Angular Material (tema Tekus)
PruebaC6-ASP.NET.pdf   Instrucciones originales de Tekus
```

## Backend (`backend/`)

Solución `PruebaTekus.slnx` con capas:

- `src/Domain` — `PruebaTekus.Domain`: entidades, value objects, reglas de negocio. Sin dependencias a otros proyectos.
- `src/Application` — `PruebaTekus.Application`: casos de uso, interfaces de repositorios/servicios. Depende solo de `Domain`.
- `src/Infrastructure` — `PruebaTekus.Infrastructure`: implementaciones concretas (EF Core, repos, servicios externos). Depende de `Application` + `Domain`.
- `src/Api` — `PruebaTekus.Api`: ASP.NET Core Web API (controllers), composición/DI. Depende de las tres anteriores.
- `tests/Application.Tests` — xUnit, apunta a `Application` + `Domain`.

Regla de dependencia: `Domain ← Application ← Infrastructure`, y `Api` amarra todo vía DI en `Program.cs`.
Esto es un punto de partida flexible — si el problema real de la prueba pide otra forma
(ej. CQRS con MediatR, un solo proyecto, vertical slices), ajusta la estructura sin apegarte
a esto de forma dogmática.

Persistencia: aún no se decide motor de base de datos. Arranca con EF Core + `UseInMemoryDatabase`
o SQLite para iterar rápido; si el problema pide algo relacional real, migrar a SQL Server es
directo cambiando el proveedor en `Infrastructure` + connection string en `Api/appsettings.json`.

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

Si el problema de la prueba requiere componentes que Material no cubre bien (tablas complejas,
formularios dinámicos), evaluar agregar algo puntual — no reemplazar Material por completo salvo
que el enunciado lo pida.

## Git

Repo inicializado en la raíz (`PruebaTekus/`), un solo `.gitignore` raíz + el `.gitignore` propio
de `frontend/` (generado por Angular CLI, se respeta tal cual). Recordar: compartir el repo con el
contacto de Tekus apenas se cree, según piden las instrucciones — no esperar a terminar la prueba.

## Convenciones al resolver el problema real

- Backend: nombres en inglés para código (clases, métodos, variables); commits y comentarios en
  español está bien si el resto del repo los tiene así — seguir lo que ya exista.
- Priorizar que compile y pase lo esencial sobre cobertura exhaustiva de tests, dado el tiempo
  limitado de la prueba — pero no saltarse tests si el enunciado los pide explícitamente.
