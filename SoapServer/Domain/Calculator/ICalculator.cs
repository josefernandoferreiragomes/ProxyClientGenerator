using System.ServiceModel;
namespace SoapServer.Services.Calculator;

[ServiceContract]
public interface ICalculator
{
    [OperationContract]
    int Add(int a, int b);

    [OperationContract]
    SumListResponse SumList(SumListRequest request);
}