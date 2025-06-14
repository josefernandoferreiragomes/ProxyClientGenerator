using System.ServiceModel;
namespace SoapServer.Domain.Calculator;

[ServiceContract]
public interface ICalculator
{
    [OperationContract]
    int Add(int a, int b);

    [OperationContract]
    SumListResponse SumList(SumListRequest request);
}