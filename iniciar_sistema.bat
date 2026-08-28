@echo off
echo ======================================
echo  Iniciando FrutNatura - Sistema Completo
echo ======================================

:: 1) Sobe o servidor Web
start "" cmd /c "cd /d %~dp0FrutNatura2 && dotnet run --no-build"

:: 2) Aguarda alguns segundos e abre o Desktop
timeout /t 5 >nul
start "" "%~dp0forms\Tela desktop\bin\Debug\net8.0-windows\Tela desktop.exe"

:: 3) Abre o navegador
timeout /t 2 >nul
start "" "http://localhost:5018"

exit
