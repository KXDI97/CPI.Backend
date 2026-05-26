# CPI Backend — Microservicios

Stack: .NET 9 · PostgreSQL · JWT

## Servicios

| Servicio | Puerto HTTP | Swagger | Responsable |
|---|---|---|---|
| AuthService.Api | 5258 | http://localhost:5258/swagger | — |
| CatalogService.Api | 5131 | http://localhost:5131/swagger | — |
| InventoryService.Api | 5035 | http://localhost:5035/swagger | — |
| SalesService.Api | 5215 | http://localhost:5215/swagger | — |
| PurchaseService.Api | 5300 | http://localhost:5300/swagger | Bryams |

## Requisitos previos

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL corriendo en `localhost:5432` con la base de datos `CPI`
- Schema aplicado:
  ```bash
  psql -h localhost -U postgres -d CPI -f Database/schema.sql
  ```

## Levantar todos los servicios

### Windows
```bat
start-all.bat
```

### Linux / Mac
```bash
./start-all.sh
```

## Detener todos los servicios

### Windows
```bat
stop-all.bat
```

### Linux / Mac
```bash
./stop-all.sh
```

## Levantar un servicio individual

```bash
cd AuthService.Api      && dotnet run --launch-profile http
cd CatalogService.Api   && dotnet run --launch-profile http
cd InventoryService.Api && dotnet run --launch-profile http
cd SalesService.Api     && dotnet run --launch-profile http
cd PurchaseService.Api  && dotnet run --launch-profile http
```

## Base de datos

- Motor: PostgreSQL local
- BD: `CPI`, usuario: `postgres`
- Puerto: `5432`
- Connection string en `appsettings.Development.json` de cada servicio:
  ```
  Host=localhost;Port=5432;Database=CPI;Username=postgres;Password=TU_PASSWORD
  ```
- El schema **no se aplica automáticamente** — correr `Database/schema.sql` manualmente en pgAdmin

## Tests

```bash
cd AuthService.Tests && dotnet test
```

## Arquitectura

```
cpi/
├── AuthService.Api/             # Registro, login, JWT, gestión de usuarios
├── AuthService.Domain/
├── AuthService.Infrastructure/
├── AuthService.Tests/
│
├── CatalogService.Api/          # Catálogo: productos, proveedores, clientes
├── CatalogService.Domain/
├── CatalogService.Application/
├── CatalogService.Infrastructure/
│
├── InventoryService.Api/        # Control de inventario y movimientos
├── InventoryService.Domain/
├── InventoryService.Application/
├── InventoryService.Infrastructure/
│
├── SalesService.Api/            # Ventas y facturación
├── SalesService.Domain/
├── SalesService.Application/
├── SalesService.Infrastructure/
│
├── PurchaseService.Api/         # Compras, órdenes, recibos, transacciones
├── PurchaseService.Domain/
├── PurchaseService.Application/
├── PurchaseService.Infrastructure/
│
├── SharedKernel/                # Contratos e interfaces compartidas
├── Abstractions/                # Abstracciones comunes
├── Database/                    # schema.sql
│
├── start-all.bat                # Levanta todos (Windows)
├── start-all.sh                 # Levanta todos (Linux/Mac)
├── stop-all.bat                 # Detiene todos (Windows)
├── stop-all.sh                  # Detiene todos (Linux/Mac)
└── .env.example                 # Plantilla de variables de entorno
```

## Dependencias entre servicios

```
Frontend (file://)
    │
    ├──▶ AuthService      :5258  — login / JWT
    ├──▶ CatalogService   :5131  — productos / suppliers / clientes
    ├──▶ InventoryService :5035  — stock / movimientos
    ├──▶ SalesService     :5215  — ventas
    └──▶ PurchaseService  :5300  — órdenes de compra / recibos / transacciones
             │
             └── consume CatalogService (Suppliers via SupplierId)
                 consume InventoryService (Products via ProductId)
```

## Notas

- **Suppliers** viven en `CatalogService` — son datos maestros del catálogo
- **PurchaseService** referencia `SupplierId` y `ProductId` pero no los crea
- Todos los servicios comparten la misma BD `CPI` en PostgreSQL
- CORS habilitado en todos los servicios para desarrollo local (`file://`, `localhost`)