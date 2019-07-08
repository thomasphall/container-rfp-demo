FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Statistics/Statistics.csproj", "Statistics/"]
COPY ["Common/Common.csproj", "Common/"]
RUN dotnet restore "Statistics/Statistics.csproj"
COPY . .
WORKDIR "/src/Statistics"
RUN dotnet build "Statistics.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Statistics.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Statistics.dll"]