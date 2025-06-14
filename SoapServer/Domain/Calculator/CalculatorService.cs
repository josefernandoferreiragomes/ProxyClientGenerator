using SoapServer.Domain.Base;
using System.Text.Json;

namespace SoapServer.Domain.Calculator;

[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
public class CalculatorService : ICalculator
{

    public int Add(int a, int b)
        => a + b; 
        
    public SumListResponse SumList(SumListRequest request)
    {
        // Validate and log before executing the actual method
        var result = ValidateAndLog<SumListRequest, SumListResponse>(SumListProcess, request);

        return result;
    }
    private SumListResponse SumListProcess(SumListRequest request)
    {
        Console.WriteLine(string.Format("SumList method called with request: {0}", JsonSerializer.Serialize<SumListRequest>(request)));
        int sum = 0;
        
        sum = request?.Numbers.Sum() ?? 0;

        Console.WriteLine($"SumList calculated sum: {sum}");
        return new SumListResponse { Sum = sum };
    }

    public TResponse ValidateAndLog<TRequest, TResponse>(
        Func<TRequest, TResponse> method,
        TRequest request
    )
    {
        if (request == null)
        {
            Console.WriteLine("Validation failed: request is null or empty");
            throw new ArgumentException("Request cannot be null or empty", nameof(request));
        }
        //log request details as json
        Console.WriteLine(string.Format("Validating request: {0}", JsonSerializer.Serialize<TRequest>(request)));

        Console.WriteLine("Validation passed, proceeding with method execution");
        var response = method(request);
        // log response details as json
        Console.WriteLine(string.Format("Validating request: {0}", JsonSerializer.Serialize<TResponse>(response)));

        return response;
    }

}