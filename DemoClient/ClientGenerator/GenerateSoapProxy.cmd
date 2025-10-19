@echo off
:: This command uses dotnet-svcutil to generate a SOAP client proxy from the CalculatorService WSDL.

:: Configuration variables
set "BaseFile=CalculatorServiceProxy"
set "ServiceUrl=https://localhost:7296/CalculatorService.svc?wsdl"

:: Create timestamp YYYY_MM_DD_HH_MM_SS
for /f "usebackq delims=" %%T in (`powershell -NoProfile -Command "Get-Date -Format 'yyyy_MM_dd_HH_mm_ss'"`) do set "timestamp=%%T"
set "WsdlFile=%BaseFile%_%timestamp%.wsdl"

:: Download the WSDL to a timestamped local file
echo Downloading WSDL to "%WsdlFile%"...
powershell -NoProfile -Command "Invoke-WebRequest -Uri '%ServiceUrl%' -OutFile '%WsdlFile%'; Write-Host 'WSDL saved to: %WsdlFile%'"

echo Deleting old file
if exist "SoapProxies\%BaseFile%.cs" (
  del /f /q "SoapProxies\%BaseFile%.cs"
) else (
  echo No existing proxy file found at "SoapProxies\%BaseFile%.cs"
)

:: Generate the SOAP client proxy using the downloaded WSDL file
dotnet-svcutil "%WsdlFile%" ^
  --outputDir "SoapProxies" ^
  --namespace "*,DemoClient.SoapProxies" ^
  --targetFramework net8.0 ^
  --outputFile "%BaseFile%.cs"

echo SOAP client proxy generated.
pause