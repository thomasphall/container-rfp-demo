FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Subscriber.csproj", "Subscriber/"]
COPY ["../Contracts/Contracts.csproj", "Contracts/"]
COPY ["../Common/Common.csproj", "Common/"]
RUN dotnet restore "Subscriber/Subscriber.csproj"
COPY .. .
WORKDIR "/src/Subscriber"
RUN dotnet build "Subscriber.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Subscriber.csproj" -c Release -o /app

FROM base AS final
USER root
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Subscriber.dll"]
