# ---- Build ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restore first so the package layer is cached until a .csproj changes
COPY CollectiveMemory/CollectiveMemory.csproj CollectiveMemory/
COPY CollectiveMemory.Core/CollectiveMemory.Core.csproj CollectiveMemory.Core/
RUN dotnet restore CollectiveMemory/CollectiveMemory.csproj

COPY . .
RUN dotnet publish CollectiveMemory/CollectiveMemory.csproj \
    -c Release -o /app/publish --no-restore /p:UseAppHost=false

# ---- Runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Non-root user built into the .NET 8 images
USER $APP_UID

# Render terminates TLS and forwards X-Forwarded-*; this makes ASP.NET trust them
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

EXPOSE 8080

# Render injects $PORT (default 10000); fall back to 8080 for local runs
CMD ["sh", "-c", "ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080} exec dotnet CollectiveMemory.dll"]
