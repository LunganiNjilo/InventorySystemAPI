FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

# Copy solution and project files
COPY InventorySystemAPI.sln .
COPY InventorySystemAPI/InventorySystemAPI.csproj InventorySystemAPI/
COPY Domain/Domain.csproj Domain/
COPY Application/Application.csproj Application/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY Tests/T1/Inventory.Tests/Inventory.Tests.csproj Tests/T1/Inventory.Tests/
COPY Tests/T3/Inventory.E2E.Tests/Inventory.E2E.Tests.csproj Tests/T3/Inventory.E2E.Tests/

# Restore once
RUN dotnet restore InventorySystemAPI.sln

# Copy remaining files
COPY . .

# Build the functional tests
RUN dotnet build Tests/T3/Inventory.E2E.Tests/Inventory.E2E.Tests.csproj -c Debug

# 🔑 IMPORTANT: Run dotnet test AGAINST THE CSPROJ
CMD ["dotnet","test","Tests/T3/Inventory.E2E.Tests/Inventory.E2E.Tests.csproj","--no-build","--logger:console;verbosity=normal"]
