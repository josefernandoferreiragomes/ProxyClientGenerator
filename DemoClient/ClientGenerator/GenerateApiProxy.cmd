@echo off
:: This command uses NSwag CLI to generate a C# client from the ApiServer Swagger spec.

:: Configuration variables
set "BaseFile=GeneratedApiClient"
set "SwaggerUrl=https://localhost:7217/swagger/v1/swagger.json"

:: Create timestamp YYYY_MM_DD_HH_MM_SS
for /f "usebackq delims=" %%T in (`powershell -NoProfile -Command "Get-Date -Format 'yyyy_MM_dd_HH_mm_ss'"`) do set "timestamp=%%T"
set "JsonFile=%BaseFile%_%timestamp%.json"

:: Download the Swagger JSON to a timestamped local file
echo Downloading swagger JSON to "%JsonFile%"...
powershell -NoProfile -Command "Invoke-WebRequest -Uri '%SwaggerUrl%' -OutFile '%JsonFile%'; Write-Host 'Swagger JSON saved to: %JsonFile%'"

echo Deleting old file
if exist "SoapProxies\%BaseFile%.cs" (
  del /f /q "SoapProxies\%BaseFile%.cs"
) else (
  echo No existing proxy file found at "SoapProxies\%BaseFile%.cs"
)

:: Generate the C# client using the downloaded JSON file
echo Generating C# client from "%JsonFile%"...
nswag openapi2csclient ^
/input:"%JsonFile%" ^
/output:"ApiProxies\%BaseFile%.cs" ^
/namespace:DemoClient.ApiProxies

echo Web API client proxy generated.
pause