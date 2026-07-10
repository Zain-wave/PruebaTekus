# PruebaTekus

Backend ASP.NET Core + frontend Angular. Ver `CLAUDE.md` para detalle de arquitectura y decisiones.

## Todo junto con Docker

```bash
docker compose up --build
```

Levanta SQL Server, backend (migra la DB solo al arrancar) y frontend en un solo comando:

- Frontend: `http://localhost:4200`
- Backend + Swagger: `http://localhost:8080/swagger`
- SQL Server: `localhost:1433` (user `sa`, password default `YourStrong@Passw0rd`, override con `MSSQL_SA_PASSWORD`)

## Backend (.NET)

```bash
cd backend
dotnet restore
dotnet run --project src/Api
```

API disponible en `http://localhost:5088` (o `https://localhost:7183` con el perfil `https`).

Correr tests:

```bash
cd backend
dotnet test
```

## Frontend (Angular)

```bash
cd frontend
npm install
npm start
```

App disponible en `http://localhost:4200`.

Correr tests:

```bash
cd frontend
npm test
```
