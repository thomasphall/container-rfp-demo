FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Migrator.csproj Migrator/
COPY ../Common/Common.csproj Common/
COPY ../Migrations/Migrations.csproj Migrations/
RUN dotnet restore Migrator/Migrator.csproj
COPY .. .
WORKDIR /src/Migrator
RUN dotnet build Migrator.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Migrator.csproj -c Release -o /app

FROM base AS final

ENV APP_ROOT=/app
ENV PATH=${APP_ROOT}/bin:${PATH} HOME=${APP_ROOT}
RUN chmod -R u+x ${APP_ROOT} && \
    chgrp -R 0 ${APP_ROOT} && \
    chmod -R g=u ${APP_ROOT} /etc/passwd

### Containers should NOT run as root as a good practice
USER 10001
WORKDIR ${APP_ROOT}
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Migrator.dll"]
