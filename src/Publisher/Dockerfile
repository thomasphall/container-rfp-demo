FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src

COPY ["Publisher/Publisher.csproj", "Publisher/"]
COPY ["../Contracts/Contracts.csproj", "Contracts/"]
COPY ["../Common/Common.csproj", "Common/"]
RUN dotnet restore "Publisher/Publisher.csproj"
COPY .. .
WORKDIR "/src/Publisher"
RUN dotnet build "Publisher.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Publisher.csproj" -c Release -o /app

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
ENTRYPOINT ["dotnet", "Publisher.dll"]
