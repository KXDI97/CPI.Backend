@echo off
echo Iniciando todos los servicios CPI...

start "AuthService     :5258" cmd /k "dotnet run --project AuthService.Api --launch-profile http"
start "CatalogService  :5131" cmd /k "dotnet run --project CatalogService.Api --launch-profile http"
start "InventoryService:5035" cmd /k "dotnet run --project InventoryService.Api --launch-profile http"
start "SalesService    :5215" cmd /k "dotnet run --project SalesService.Api --launch-profile http"
start "PurchaseService :5300" cmd /k "dotnet run --project PurchaseService.Api --launch-profile http"

echo.
echo Swaggers disponibles:
echo   Auth     -^> http://localhost:5258/swagger
echo   Catalog  -^> http://localhost:5131/swagger
echo   Inventory-^> http://localhost:5035/swagger
echo   Sales    -^> http://localhost:5215/swagger
echo   Purchase -^> http://localhost:5300/swagger
pause