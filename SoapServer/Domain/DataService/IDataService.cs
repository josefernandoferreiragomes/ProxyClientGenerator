namespace SoapServer.Domain.DataService
{
    [ServiceContract]
    public interface IDataService
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }
}
