
@echo off
echo.
echo  ╔══════════════════════════════════════════════╗
echo  ║        CPI — Deteniendo microservicios       ║
echo  ╚══════════════════════════════════════════════╝
echo.
 
echo  Buscando procesos dotnet activos...
 
taskkill /F /IM dotnet.exe 2>nul
if %errorlevel%==0 (
    echo  Todos los servicios detenidos correctamente.
) else (
    echo  No habia servicios corriendo.
)
 
echo.
echo  Liberando puertos 5258, 5131, 5035, 5215, 5300...
 
for %%p in (5258 5131 5035 5215 5300) do (
    for /f "tokens=5" %%a in ('netstat -ano ^| findstr :%%p 2^>nul') do (
        taskkill /F /PID %%a 2>nul
    )
)
 
echo  Puertos liberados.
echo.
pause
 