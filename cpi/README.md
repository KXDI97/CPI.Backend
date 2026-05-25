# CPI Backend — Microservicios

Stack: .NET 9 · PostgreSQL · JWT

## Servicios

| Servicio | Puerto | Swagger |
|---|---|---|
| AuthService.Api | 5258 | http://localhost:5258/swagger |
| CatalogService.Api | 5131 | http://localhost:5131/swagger |
| InventoryService.Api | 5035 | http://localhost:5035/swagger |
| SalesService.Api | 5215 | http://localhost:5215/swagger |

## Requisitos previos

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL corriendo en `localhost:5432` con la base de datos `CPI`
- Schema aplicado: `psql -h localhost -U postgres -d CPI -f Database/schema.sql`

## Levantar todos los servicios

```bash
./start-all.sh
```

El script levanta los 4 microservicios en paralelo, cada uno con un color diferente en el log:

- **Cyan** → AuthService
- **Amarillo** → CatalogService
- **Verde** → InventoryService
- **Magenta** → SalesService

Presionar `Ctrl+C` detiene todos los procesos automáticamente.

## Detener todos los servicios

```bash
./stop-all.sh
```

Funciona aunque `start-all.sh` ya no esté corriendo; busca los procesos por nombre.

## Levantar un servicio individual

```bash
cd AuthService.Api && dotnet run --launch-profile http
cd CatalogService.Api && dotnet run --launch-profile http
cd InventoryService.Api && dotnet run --launch-profile http
cd SalesService.Api && dotnet run --launch-profile http
```

## Variables de entorno

Copiar `.env.example` a `.env` y ajustar:

```bash
cp .env.example .env
```

## Tests

```bash
cd AuthService.Tests && dotnet test
```

## Base de datos

- Motor: PostgreSQL local, servidor `Hidrox`
- BD: `CPI`, usuario: `postgres`
- Connection string en `appsettings.Development.json` de cada servicio
- El schema **no se aplica automáticamente** — correr `Database/schema.sql` manualmente

## Arquitectura

```
cpi/
├── AuthService.Api/          # Registro, login, JWT, gestión de usuarios
├── CatalogService.Api/       # Catálogo de productos y proveedores
├── InventoryService.Api/     # Control de inventario y movimientos
├── SalesService.Api/         # Ventas y facturación
├── SharedKernel/             # Contratos e interfaces compartidas
├── Database/                 # schema.sql
├── start-all.sh              # Levanta todos los servicios
├── stop-all.sh               # Detiene todos los servicios
└── .env.example              # Plantilla de variables de entorno
```
