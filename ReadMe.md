---
# Walkthrough 
Demonstration of a .NET solution that shows how to use command scripts to generate both REST API proxies (using NSwag) and SOAP service proxies (using the .NET Core version of SvcUtil) for a server�client scenario. In this demo, we create three projects:

- **ApiServer** � an ASP.NET Core Web API with Swagger enabled.
- **SoapServer** � a simple SOAP service using CoreWCF.
- **DemoClient** � a Console App that consumes both the generated API client and SOAP service proxy.

Feel free to follow (or adapt) these steps.

---

```md
# Demo: Generating API and SOAP Proxies with .NET

This demo creates a .NET solution with:
- An **ApiServer** project exposing REST endpoints (with Swagger),
- A **SoapServer** project exposing a SOAP endpoint using CoreWCF, and
- A **DemoClient** project that consumes both services via generated proxy clients.

> **Note:** You will use command scripts (.cmd files) to run:
> - **NSwag** commands for generating a REST client proxy, and  
> - **dotnet-svcutil** commands for generating a SOAP proxy.

---

## 1. Create the .NET Solution and Projects

Open a terminal (or PowerShell) and run the following commands:

```cmd
:: Create the solution
dotnet new sln -n DemoProxies

:: Create the Web API server (ApiServer)
dotnet new webapi -n ApiServer

:: Create the console client project (DemoClient)
dotnet new console -n DemoClient

```

Then add all projects to the solution:

```cmd
dotnet sln add ApiServer/ApiServer.csproj
dotnet sln add DemoClient/DemoClient.csproj
```

---

## 2. Set Up the Projects

### 2.1. **ApiServer** � Enable Swagger

Open the NuGet Package Manager in Visual Studio or use the Package Manager Console to install the Swashbuckle.AspNetCore package:
Using Package Manager Console:
```bash
   Install-Package Swashbuckle.AspNetCore
```
.NET CLI
```bash
   dotnet add package Swashbuckle.AspNetCore
```
Open the `Program.cs` file in **ApiServer** and ensure Swagger is configured. For example, modify it as follows:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();  // (Enables Swagger)

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
```

> **Tip:** The default Web API template often includes a WeatherForecast endpoint. You may use it or add your own controllers.

### 2.2. **SoapServer** � Create a Simple SOAP Service Using CoreWCF

Make sure to have Windows Communication Foundation workload installed
Make sure to have the coreWCF template installed
```bash
dotnet new install CoreWCF.Templates
```
New project
```bash
dotnet new corewcf --name MyService
```

Add project to solution
```bash
dotnet sln add SoapServer/SoapServer.csproj
```

Reference: https://github.com/CoreWCF/CoreWCF

1. **Add CoreWCF Packages, in case they-re not included already**  
In **SoapServer**, add the following NuGet packages:
   
   ```cmd
   dotnet add SoapServer package CoreWCF
   dotnet add SoapServer package CoreWCF.Http
   dotnet add SoapServer package CoreWCF.Primitives

   ```

2. **Define the Service Contract and Implementation**  
In the **SoapServer** project, add a new file `ICalculator.cs`:

```csharp
using System.ServiceModel;

[ServiceContract]
public interface ICalculator
{
    [OperationContract]
    int Add(int a, int b);
}
```

Then add a file `CalculatorService.cs`:

```csharp
public class CalculatorService : ICalculator
{
    public int Add(int a, int b) => a + b;
}
```

3. **Configure the SOAP Endpoint**  
Replace the content of `Program.cs` in **SoapServer** with the following code:

```csharp
using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;  // For service behaviors if needed

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<CalculatorService>();
    serviceBuilder.AddServiceEndpoint<CalculatorService, ICalculator>(
        new BasicHttpBinding(),
        "/CalculatorService"
    );
});

// Run the SOAP service. By default it will run on Kestrel�s port (e.g., 5001)
app.Run();
```

> **Note:** You can test the SOAP service in a browser or WCF test client by browsing to:  
> `https://localhost:7296/CalculatorService?wsdl`

!!! Beware there is a glitch in visual studio 2022 when downloading the WSDL definition. Do not use it in debug mode.

### 2.3. **DemoClient** � Prepare the Client Project

In **DemoClient**, you will later add the generated code files. In the meantime, you can create a basic `Program.cs` to test the proxies.

---

## 3. Create Command Files to Generate Proxies

### 3.1. Generate the Web API Client Proxy Using NSwag

Create a file named `GenerateApiProxy.cmd` (in the solution folder or in a dedicated �scripts� folder):

```cmd
@echo off
:: This command uses NSwag CLI to generate a C# client from the ApiServer Swagger spec.
nswag run /command:swagger2csclient ^
  /input:"http://localhost:5000/swagger/v1/swagger.json" ^
  /output:"DemoClient\GeneratedApiClient.cs" ^
  /language:CSharp ^
  /namespace:DemoClient.ApiProxies

echo Web API client proxy generated.
pause
```

> **Before running this command:**  
> � Run **ApiServer** so that the swagger.json is available at the specified URL (adjust the port if needed).  
> � Ensure NSwag CLI is installed (or use the NSwag Studio tool).

---

### 3.2. Generate the SOAP Service Proxy Using dotnet-svcutil

Create a file named `GenerateSoapProxy.cmd`:

```cmd
@echo off
:: This command uses dotnet-svcutil to generate a SOAP client proxy from the CalculatorService WSDL.
dotnet-svcutil http://localhost:5001/CalculatorService?wsdl ^
  --outputDir "DemoClient" ^
  --namespace "DemoClient.SoapProxies" ^
  --targetFramework net8.0

echo SOAP client proxy generated.
pause
```

> **Before running this command:**  
> � Start **SoapServer** so that its WSDL is accessible at the URL provided.

---

## 4. Consume the Generated Proxies in the Client Application

Install Newtonsoft.Json
```bash
dotnet add democlient package newtonsoft.json
```

After running the command files, the **DemoClient** project will contain:

- A file named `GeneratedApiClient.cs` (from NSwag) in the namespace `DemoClient.ApiProxies`.
- A generated SOAP proxy (e.g., a class like `CalculatorServiceClient`) in the namespace `DemoClient.SoapProxies`.

Now, update **DemoClient**�s `Program.cs` to use these proxies. For example:

```csharp
using System;
using System.Threading.Tasks;
// Namespaces based on generated code (adjust as needed)
using DemoClient.ApiProxies;
using DemoClient.SoapProxies;
using System.ServiceModel; // For WS-* bindings

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Demo Client Starting...");

        // --- Invoke the REST API via the generated client ---
        try
        {
            // "MyApiClient" is assumed to be the generated client class from NSwag.
            // The constructor and methods depend on the generated code.
            var apiClient = new MyApiClient("http://localhost:5000");
            var values = await apiClient.GetWeatherForecastAsync(); // or your specific API method
            Console.WriteLine("REST API call results:");
            foreach (var value in values)
            {
                Console.WriteLine(value);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error calling REST API: " + ex.Message);
        }

        // --- Invoke the SOAP service via the generated proxy ---
        try
        {
            // Here we assume the generated proxy is called "CalculatorServiceClient"
            // Create an instance with a BasicHttpBinding and EndpointAddress.
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress("http://localhost:5001/CalculatorService");
            var soapClient = new CalculatorServiceClient(binding, endpoint);

            // Call the Add method (synchronous or asynchronous; adjust depending on generated code)
            int sum = await soapClient.AddAsync(3, 5);
            Console.WriteLine($"SOAP service result: 3 + 5 = {sum}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error calling SOAP service: " + ex.Message);
        }

        Console.WriteLine("Demo Client Completed.");
    }
}
```

> **Important:** The names of the generated classes and their methods depend on the specific Swagger and WSDL definitions. Adjust the code accordingly if they differ.

---

## 5. Running the Demo

1. **Start the Servers:**
   - Run **ApiServer** (ensure Swagger is enabled, and the API is serving on the expected port, e.g., `http://localhost:5000`).
   - Run **SoapServer** (verify that `http://localhost:5001/CalculatorService?wsdl` works).

2. **Generate Proxies:**
   - Open a terminal and run `GenerateApiProxy.cmd`.  
   - Then run `GenerateSoapProxy.cmd`.

3. **Test the Client:**
   - Build and run **DemoClient**. You should see output from both the REST API call and the SOAP service call.

---

## 6. Additional Customizations

- **Authentication Headers:**  
  To pass authentication information when generating the API client, add a `/headers` parameter to the NSwag command (e.g., `/headers:"Authorization=Bearer YOUR_TOKEN"`).

- **Async Methods & Advanced Bindings:**  
  If you want asynchronous methods on the SOAP side or need special configuration, consult the [dotnet-svcutil documentation](https://learn.microsoft.com/en-us/dotnet/core/additional-tools/dotnet-svcutil-guide) for advanced options.

- **Batch Script for Proxy Updates:**  
  You can create a master batch file (`GenerateAllProxies.cmd`) to run both commands sequentially:

  ```cmd
  @echo off
  call GenerateApiProxy.cmd
  call GenerateSoapProxy.cmd
  echo All service proxies updated.
  pause
  ```

---

By following these steps, you now have a demonstration app that illustrates how to use command-line scripts to generate both API and web service proxies and consume them from a client project.
```
