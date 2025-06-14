using SoapServer.Domain.Base;

namespace SoapServer.Domain.Calculator;
public class CalculatorService : ICalculator
{
    private readonly ILogger _logger;

    public CalculatorService(ILogger<CalculatorService> logger)
    {
        _logger = logger;
        logger.LogInformation("CalculatorService instantiated");
    }

    public int Add(int a, int b) 
        => a + b;

    public SumListResponse SumList(SumListRequest request)
    {
        // Validate and log before executing the actual method
        ValidateAndLog<SumListRequest, SumListResponse>(SumListProcess, request);

        return SumList(request);
    }
    private SumListResponse SumListProcess(SumListRequest request)
    {
        _logger.LogInformation("SumList method called with request: {@Request}", request);
        int sum = 0;
        
        sum = request?.Numbers.Sum() ?? 0;

        _logger.LogInformation($"SumList calculated sum: {sum}");
        return new SumListResponse { Sum = sum };
    }

    public void ValidateAndLog<TRequest, TResponse>(
        Func<TRequest, TResponse> method,
        TRequest request
    )
    where TRequest : BaseRequest
    where TResponse : BaseResponse
    {
        if (request == null)
        {
            _logger.LogWarning("Validation failed: request is null or empty");
            throw new ArgumentException("Request cannot be null or empty", nameof(request));
        }
        //log request details as json
        _logger.LogInformation("Validating request: {@Request}", request);

        _logger.LogInformation("Validation passed, proceeding with method execution");
        var response = method(request);
        // log response details as json
        _logger.LogInformation("Validating request: {@Response}", response);
    }

}