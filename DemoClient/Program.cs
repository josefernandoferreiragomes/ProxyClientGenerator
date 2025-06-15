using System;
using System.Threading.Tasks;
// Namespaces based on generated code (adjust as needed)
using DemoClient.ApiProxies;
using DemoClient.SoapProxies;

using System.ServiceModel; // For WS-* bindings
Console.WriteLine("Demo Client Starting...");
Console.WriteLine("Press any key to continue...");
Console.ReadKey();
// --- Invoke the REST API via the generated client ---
try
{
    // "MyApiClient" is assumed to be the generated client class from NSwag.
    // The constructor and methods depend on the generated code.

    //create HttpClient
    HttpClient httpClient = new HttpClient();

    var apiClient = new Client("https://localhost:7217", httpClient);
    var request = new WeatherForecastRequest() { StartDate = DateTime.Now };
    var values = await apiClient.GetWeatherForecastAsync(request); // or your specific API method
    Console.WriteLine("REST API call results:");
    foreach (var value in values)
    {
        Console.WriteLine(value.Summary);
    }
}
catch (Exception ex)
{
    Console.WriteLine("Error calling REST API: " + ex.Message);
}

// --- Invoke the SOAP service via the generated proxy ---
try
{
    // Here we assume the generated proxy is called "CalculatorServiceProxy"
    // Create an instance with a BasicHttpBinding and EndpointAddress.
    var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
    var endpoint = new EndpointAddress("https://localhost:7296/CalculatorService.svc");
    var soapClient = new DemoClient.SoapProxies.CalculatorClient(binding, endpoint);

    // Call the Add method (synchronous or asynchronous; adjust depending on generated code)
    int sum = await soapClient.AddAsync(3, 5);
    Console.WriteLine($"SOAP service result: 3 + 5 = {sum}");
}
catch (Exception ex)
{
    Console.WriteLine("Error calling SOAP service: " + ex.Message);
}

Console.WriteLine("Demo Client Completed.");
