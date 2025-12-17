# 📦 Inventory Management System API

InventorySystemAPI is the backend for a modern **Inventory Management System**.  
It provides a **.NET 8 Clean Architecture Web API** that exposes endpoints for:

- Product Categories
- Products
- Inventory
- Suppliers
- Product–Supplier relationships

The API supports:

- Managing product categories
- Managing products with pricing and categorisation
- Tracking inventory levels and thresholds
- Supplier management
- Full CRUD operations across the domain

A separate **Nuxt 3 / Vue.js frontend (InventorySystemUI)** consumes this API.

---

## 🚀 Tech Stack

- **API:** .NET 8 Web API (Clean Architecture)
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Containerisation:** Docker & Docker Compose
- **Frontend:** Nuxt 3 + Vue + Vuetify (separate repository)
- **Testing:**
  - NUnit
  - WebApplicationFactory + NSubstitute (Integrated tests)
  - End-to-End Functional Tests (T3)

---

## 📦 Quick Start

The **recommended** way to run the Inventory Management System is via  
**Docker Compose**, so that the API, SQL Server, and automated tests run together.

Local execution is also available for development and debugging.

---

## 🐳 Run API + SQL via Docker Compose 
please ensure that you have docker downloaded and open:

From the **repository root**:

```bash
docker compose up --build
```

This will start the following containers:

| Container                       | Purpose                | Status After Startup        |
| ------------------------------- | ---------------------- | --------------------------- |
| inventory-sql                   | SQL Server database    | 🟢 Running                  |
| inventorysystem-api             | Inventory backend API  | 🟢 Running                  |
| inventory-functional-tests (T3) | End-to-end test runner | 🔴 Exits after tests finish |

## ➡️ Swagger UI:
http://localhost:8080/swagger
---
### 🧪 End-to-End Functional Tests (T3)

T3 tests validate real API behavior against a real SQL database, covering:

- Category → Product → Inventory lifecycle
- CRUD happy paths
- Update verification
- Cleanup of created data

Example output:
```text
Passed! - Failed: 0, Passed: 8, Skipped: 0, Total: 8
```
Tests run sequentially to avoid database conflicts.
---
### Running T3 Tests Manually
Using Docker:
```bash
docker start inventory-functional-tests
docker logs -f inventory-functional-tests
```
Locally
```
dotnet test Tests/T3/Inventory.E2E.Tests/Inventory.E2E.Tests.csproj
```
---

### 🧪 Integrated Tests (T1)
The backend includes integrated tests that run in a Testing environment which:

- Does not require SQL Server
- Uses mocked repositories (NSubstitute)
- Uses real controllers, filters, and AutoMapper profiles

Run all integrated tests:
```bash
dotnet test
```

## 🔗 Related Repository

Frontend SPA for this API:

➡️ InventorySystemUI
https://github.com/LunganiNjilo/InventorySystemUI
