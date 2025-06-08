using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;  // For service behaviors if needed

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServiceModelServices(); // Add CoreWCF services
builder.Services.AddServiceModelMetadata(); // Add metadata support (WSDL generation)
var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<CalculatorService>(serviceOptions =>
    {
        // Add ServiceMetadataBehavior to enable WSDL
        serviceOptions.BaseAddresses.Add(new Uri("http://localhost:5001/CalculatorService"));
    });
    serviceBuilder.AddServiceEndpoint<CalculatorService, ICalculator>(
        new BasicHttpBinding(),
        ""
    );
    // Add metadata endpoint
    serviceBuilder.ConfigureServiceHostBase<CalculatorService>(host =>
    {
        var smb = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
        if (smb == null)
        {
            smb = new ServiceMetadataBehavior { HttpGetEnabled = true };
            host.Description.Behaviors.Add(smb);
        }
    });
});


// Run the SOAP service. By default it will run on Kestrel’s port (e.g., 5001)
app.Run();