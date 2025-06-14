using SoapServer.Domain.Calculator;
using SoapServer.Domain.DataService;

var builder = WebApplication.CreateBuilder();

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<DataService>();
    serviceBuilder.AddServiceEndpoint<DataService, IDataService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/Service.svc");
    
    serviceBuilder.AddService<CalculatorService>();
    serviceBuilder.AddServiceEndpoint<CalculatorService, ICalculator>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/CalculatorService.svc");
    
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.Run();
