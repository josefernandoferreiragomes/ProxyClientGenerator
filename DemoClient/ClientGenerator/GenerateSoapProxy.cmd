@echo off
:: This command uses dotnet-svcutil to generate a SOAP client proxy from the CalculatorService WSDL.
dotnet-svcutil https://localhost:7296/CalculatorService.svc?wsdl ^
  --outputDir "SoapProxies" ^
  --namespace "*,DemoClient.SoapProxies" ^
  --targetFramework net8.0 ^
  --outputFile "CalculatorServiceProxy.cs"

echo SOAP client proxy generated.
pause
