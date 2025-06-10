using SoapServer.Services.Base;

namespace SoapServer.Services.Calculator;

[DataContract]
public class SumListRequest: BaseRequest
{
    [DataMember]
    public List<int> Numbers { get; set; } = new List<int>();
        
}