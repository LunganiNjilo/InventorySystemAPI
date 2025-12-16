FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# --------------------------------------
# BUILD STAGE
# --------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all project files so EF migrations are included
COPY InventorySystemAPI/InventorySystemAPI.csproj InventorySystemAPI/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/

# Restore dependencies
RUN dotnet restore InventorySystemAPI/InventorySystemAPI.csproj

# Copy everything
COPY . .

# Build API
WORKDIR /src/InventorySystemAPI
RUN dotnet build -c Release -o /app/build

# --------------------------------------
# PUBLISH STAGE
# --------------------------------------
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# --------------------------------------
# RUNTIME STAGE
# --------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "InventorySystemAPI.dll"]
