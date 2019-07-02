FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Migrator/Migrator.csproj Migrator/
COPY Common/Common.csproj Common/
COPY Migrations/Migrations.csproj Migrations/
RUN dotnet restore Migrator/Migrator.csproj
COPY . .
WORKDIR /src/Migrator
RUN dotnet build Migrator.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Migrator.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Migrator.dll"]
