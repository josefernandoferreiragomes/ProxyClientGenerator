using SoapServer.Domain.Base;

namespace SoapServer.Domain.Calculator;

[DataContract]
public class SumListResponse
{
    [DataMember]
    public int Sum { get; set; }

    [DataMember]
    ResponseMetadata responseMetadata { get; set; } = new ResponseMetadata();
}