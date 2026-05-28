# Proyecto CPI — Contexto para Claude

## Stack tecnológico
- **Backend:** .NET 9 (C#), arquitectura de microservicios
- **Base de datos:** PostgreSQL local, servidor `Hidrox`, base de datos `CPI`
- **Frontend:** HTML / CSS / JavaScript (sin framework)
- **Auth:** JWT Bearer tokens

## Estructura de la solución (`cpi.sln`)

| Servicio | Proyectos |
|---|---|
| Auth | `AuthService.Api`, `AuthService.Domain`, `AuthService.Infrastructure`, `AuthService.Tests` |
| Catalog | `CatalogService.Api`, `CatalogService.Application`, `CatalogService.Domain`, `CatalogService.Infrastructure` |
| Inventory | `InventoryService.Api`, `InventoryService.Application`, `InventoryService.Domain`, `InventoryService.Infrastructure` |
| Sales | `SalesService.Api`, `SalesService.Application`, `SalesService.Domain`, `SalesService.Infrastructure` |
| Shared | `SharedKernel`, `Abstractions`, `CPI.SharedKernel` |

## Base de datos

- **Motor:** PostgreSQL (local)
- **Servidor:** Hidrox — `Host=localhost;Port=5432`
- **BD:** `CPI`
- **Usuario:** `postgres`
- **Script de creación:** `Database/schema.sql` — correr manualmente antes de levantar la API

```bash
psql -h localhost -U postgres -d CPI -f Database/schema.sql
```

- La BD **no se crea desde el código** — `EnsureCreatedAsync` y `MigrateAsync` fueron eliminados de `Program.cs`
- Columnas e identificadores en **PascalCase** con comillas (`"Users"`, `"Username"`, etc.)

### Tablas principales
`Users`, `Suppliers`, `Clients`, `Products`, `PurchaseOrders`, `PurchaseOrderDetails`, `LogicalCosts`, `Transactions`, `Sales`, `SalesDetails`, `PurchaseReceipts`, `PurchaseReceiptDetails`, `InventoryMovements`

### Connection strings
- **Development** (`appsettings.Development.json`): `Host=localhost;Port=5432;Database=CPI;Username=postgres;Password=Freedom0925,`
- **Production** (`appsettings.json`): placeholder `TuPassword` — actualizar antes de deploy

## Lo que ya funciona

- **Register** — `POST /api/auth/register` (hash con PBKDF2, guarda en `Users`)
- **Login** — `POST /api/auth/login` (devuelve JWT)
- **Me** — `GET /api/auth/me` (requiere token)
- **CRUD usuarios** — `GET/PUT/DELETE /api/users` (requiere rol Admin)
- **JWT** con issuer/audience validados, expiración 8 horas
- **23 tests en verde** en `AuthService.Tests`

## Pendiente

- Conectar el frontend (HTML/CSS/JS) con la API de autenticación
- Validación de roles en el frontend (ocultar/mostrar según `Admin | Seller | Viewer`)
- Implementar y conectar los demás microservicios (Catalog, Inventory, Sales)

## Convenciones del proyecto

- `AuthService.Api` corre en `http://localhost:5258` (perfil `http`)
- Swagger disponible en `http://localhost:5258/swagger/index.html`
- El rol por defecto al registrar es `Viewer`; roles válidos: `Admin`, `Seller`, `Viewer`
- La entidad `User` vive en `AuthService.Domain/User.cs`
- El mapeo EF Core está en `AuthService.Infrastructure/Data/AuthDbContext.cs`

## Comandos frecuentes

```bash
# Levantar AuthService
cd AuthService.Api && dotnet run --launch-profile http

# Correr tests
cd AuthService.Tests && dotnet test

# Aplicar schema desde cero
psql -h localhost -U postgres -d CPI -f Database/schema.sql
```
