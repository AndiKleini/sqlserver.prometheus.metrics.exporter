FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build
WORKDIR /app
COPY "sqlserver.metrics.exporter/SqlServer.Metrics.Exporter.csproj" "sqlserver.metrics.exporter/SqlServer.Metrics.Exporter.csproj"
COPY "sqlserver.metrics.provider/SqlServer.Metrics.Provider.csproj" "sqlserver.metrics.provider/SqlServer.Metrics.Provider.csproj"
RUN dotnet restore "sqlserver.metrics.exporter/SqlServer.Metrics.Exporter.csproj"
RUN dotnet restore "sqlserver.metrics.provider/SqlServer.Metrics.Provider.csproj"

COPY "sqlserver.metrics.exporter/" "./sqlserver.metrics.exporter"
COPY "sqlserver.metrics.provider/" "./sqlserver.metrics.provider"

RUN dotnet publish "sqlserver.metrics.exporter/SqlServer.Metrics.Exporter.csproj" -c Release -o /app/published-app 

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine as runtime
WORKDIR /app
COPY --from=build /app/published-app /app
ENTRYPOINT [ "dotnet", "/app/SqlServer.Metrics.Exporter.dll" ]