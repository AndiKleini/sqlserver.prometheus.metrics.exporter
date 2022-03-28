FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build
WORKDIR /app
COPY "sqlserver.metrics.exporter/Sqlserver.Metrics.Exporter.csproj" "sqlserver.metrics.exporter/"
COPY "sqlserver.metrics.provider/Sqlserver.Metrics.Provider.csproj" "SqlServer.metrics.provider/SqlServer.Metrics.Provider.csproj"
RUN dotnet restore "sqlserver.metrics.exporter/Sqlserver.Metrics.Exporter.csproj"
RUN dotnet restore "SqlServer.metrics.provider/SqlServer.Metrics.Provider.csproj"

COPY "sqlserver.metrics.exporter/" "./sqlserver.metrics.exporter"
COPY "sqlserver.metrics.provider/" "./SqlServer.metrics.provider"
#RUN dotnet build "sqlserver.metrics.exporter/Sqlserver.Metrics.Exporter.csproj" -c Release -o /app

RUN dotnet publish "sqlserver.metrics.exporter/Sqlserver.Metrics.Exporter.csproj" -c Release -o /app/published-app 

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine as runtime
WORKDIR /app
COPY --from=build /app/published-app /app
ENTRYPOINT [ "dotnet", "/app/Sqlserver.Metrics.Exporter.dll" ]