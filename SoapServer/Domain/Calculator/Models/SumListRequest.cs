using SoapServer.Domain.Base;

namespace SoapServer.Domain.Calculator;

[DataContract]
public class SumListRequest: BaseRequest
{
    [DataMember]
    public List<int> Numbers { get; set; } = new List<int>();
        
}