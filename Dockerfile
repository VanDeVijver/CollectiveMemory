# Use official ASP.NET Core runtime as base image for production
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Configure ASP.NET Core to listen on port 8080 (Render expects 10000/8080 style)
ENV ASPNETCORE_URLS=http://+:8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution/project files
COPY ["CollectiveMemory/CollectiveMemory.csproj", "CollectiveMemory/"]

# Restore dependencies
RUN dotnet restore "CollectiveMemory/CollectiveMemory.csproj"

# Copy the rest of the source code
COPY . .

WORKDIR "/src/CollectiveMemory"

# Build the app
RUN dotnet publish "CollectiveMemory.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CollectiveMemory.dll"]

