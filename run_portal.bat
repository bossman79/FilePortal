@echo off
:: Set code page to UTF-8 for clean output rendering
chcp 65001 >nul
title Adept Document Portal

:: Set the current directory to the folder containing this script
cd /d "%~dp0"

echo =====================================================================
echo                     ADEPT DOCUMENT PORTAL LAUNCHER
echo =====================================================================
echo.
echo [*] Checking Python installation...
python --version >nul 2>&1
if %errorlevel% neq 0 (
    echo [ERROR] Python is not installed or not found in your system PATH.
    echo Please install Python and ensure it is added to your PATH.
    pause
    exit /b 1
)

echo [*] Starting Adept Document Portal server...
echo [*] To STOP the portal, press Ctrl+C in this window or simply CLOSE it.
echo.

:: Automatically open the portal URL in the default browser after 2 seconds
start /b cmd /c "timeout /t 2 >nul && start http://localhost:5000"

:: Start the Flask app
python app.py

echo.
echo [*] Portal stopped.
pause
