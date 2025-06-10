using SoapServer.Services.Calculator;
using SoapServer.Services.Service;

var builder = WebApplication.CreateBuilder();

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    //test it by using SoapServer lauch profie, and pasting the URL in a browser: https://localhost:7296/Service.svc?wsdl
    serviceBuilder.AddService<Service>();
    serviceBuilder.AddServiceEndpoint<Service, IService>(
        new BasicHttpBinding(BasicHttpSecurityMode.Transport),
        "/Service.svc"
    );

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
