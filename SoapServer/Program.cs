using SoapServer.Services.Calculator;

var builder = WebApplication.CreateBuilder();

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();

// Add logging middleware before UseServiceModel
app.Use(async (context, next) =>
{
    Console.WriteLine($"[{DateTime.Now}] {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
});

app.UseServiceModel(serviceBuilder =>
{

    //test it by using SoapServer lauch profie, and pasting the URL in a browser: https://localhost:7296/CalculatorService.svc?wsdl
    serviceBuilder.AddService<CalculatorService>();
    serviceBuilder.AddServiceEndpoint<CalculatorService, ICalculator>(
        new BasicHttpBinding(BasicHttpSecurityMode.Transport),
        "/CalculatorService.svc"
    );

    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.Run();
