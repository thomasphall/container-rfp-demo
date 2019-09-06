FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Publisher.csproj", "Publisher/"]
COPY ["../Contracts/Contracts.csproj", "Contracts/"]
COPY ["../Common/Common.csproj", "Common/"]
RUN dotnet restore "Publisher/Publisher.csproj"
COPY .. .
WORKDIR "/src/Publisher"
RUN dotnet build "Publisher.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Publisher.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Publisher.dll"]
