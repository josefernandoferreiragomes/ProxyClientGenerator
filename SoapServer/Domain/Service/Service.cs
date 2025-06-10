namespace SoapServer.Services.Service;

public class Service : IService
{
    private readonly ILogger _logger;
    public Service(ILogger logger)
    {
        _logger = logger;
        logger.Log(LogLevel.Information, "Service instanciated");
    }
    public string GetData(int value)
    {
        return string.Format("You entered: {0}", value);
    }

    public CompositeType GetDataUsingDataContract(CompositeType composite)
    {
        if (composite == null)
        {
            throw new ArgumentNullException("composite");
        }
        if (composite.BoolValue)
        {
            composite.StringValue += "Suffix";
        }
        return composite;
    }
}
