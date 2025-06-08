@echo off
:: This command uses NSwag CLI to generate a C# client from the ApiServer Swagger spec.

nswag openapi2csclient ^
/input:https://localhost:7217/swagger/v1/swagger.json ^
/output:DemoClient\GeneratedApiClient.cs ^
/namespace:DemoClient.ApiProxies

echo Web API client proxy generated.
pause
