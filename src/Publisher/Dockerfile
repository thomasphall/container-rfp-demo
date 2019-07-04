FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Publisher/Publisher.csproj Publisher/
COPY Contracts/Contracts.csproj Contracts/
COPY Common/Common.csproj Common/
RUN dotnet restore Publisher/Publisher.csproj
COPY . .
WORKDIR /src/Publisher
RUN dotnet build Publisher.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Publisher.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Publisher.dll"]
