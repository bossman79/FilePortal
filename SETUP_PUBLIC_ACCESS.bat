@echo off
REM Setup script to allow public access to Adept Portal
REM Run this as Administrator

echo ========================================
echo Adept Portal - Public Access Setup
echo ========================================
echo.

REM Check for admin privileges
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo ERROR: This script must be run as Administrator!
    echo.
    echo Right-click this file and select "Run as administrator"
    pause
    exit /b 1
)

echo Step 1: Adding Windows Firewall rule for port 5000...
netsh advfirewall firewall add rule name="Adept Portal (Port 5000)" dir=in action=allow protocol=TCP localport=5000
if %errorLevel% equ 0 (
    echo SUCCESS: Firewall rule added
) else (
    echo WARNING: Firewall rule may already exist or failed to add
)
echo.

echo Step 2: Adding Python to Windows Firewall...
netsh advfirewall firewall add rule name="Python (Adept Portal)" dir=in action=allow program="%LOCALAPPDATA%\Programs\Python\Python313\python.exe" enable=yes
if %errorLevel% equ 0 (
    echo SUCCESS: Python firewall rule added
) else (
    echo Trying alternate Python path...
    netsh advfirewall firewall add rule name="Python (Adept Portal)" dir=in action=allow program="C:\Python313\python.exe" enable=yes
)
echo.

echo Step 3: Checking your IP addresses...
echo.
echo Your Local IP address:
for /f "tokens=2 delims=:" %%a in ('ipconfig ^| findstr /c:"IPv4"') do echo %%a
echo.

echo Your Public IP address:
powershell -Command "(Invoke-WebRequest -Uri 'https://api.ipify.org' -UseBasicParsing).Content"
echo.

echo ========================================
echo Setup Complete!
echo ========================================
echo.
echo Next Steps:
echo 1. Start your Flask server: python app.py
echo 2. Test locally first: http://localhost:5000
echo 3. Test from another device on your network using the Local IP shown above
echo 4. For external access, configure your router's port forwarding:
echo    - Forward external port 5000 to your Local IP on port 5000
echo    - Consult your router's manual for port forwarding instructions
echo.
echo IMPORTANT: Update the "Portal Public URL" in your dashboard with
echo            your public IP address shown above
echo.
pause
