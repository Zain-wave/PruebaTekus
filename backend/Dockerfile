FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY PruebaTekus.slnx .
COPY src/Domain/PruebaTekus.Domain.csproj src/Domain/
COPY src/Application/PruebaTekus.Application.csproj src/Application/
COPY src/Infrastructure/PruebaTekus.Infrastructure.csproj src/Infrastructure/
COPY src/Api/PruebaTekus.Api.csproj src/Api/
COPY tests/Application.Tests/PruebaTekus.Application.Tests.csproj tests/Application.Tests/
RUN dotnet restore src/Api/PruebaTekus.Api.csproj

COPY . .
RUN dotnet publish src/Api/PruebaTekus.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "PruebaTekus.Api.dll"]
