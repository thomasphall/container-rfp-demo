FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Website.csproj", "Website/"]
RUN dotnet restore "Website/Website.csproj"
COPY .. .
WORKDIR "/src/Website"
RUN dotnet build "Website.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Website.csproj" -c Release -o /app

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
ENTRYPOINT ["dotnet", "Website.dll"]
