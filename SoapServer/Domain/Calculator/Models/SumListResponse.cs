using SoapServer.Domain.Base;

namespace SoapServer.Domain.Calculator;

[DataContract]
public class SumListResponse: BaseResponse
{
    [DataMember]
    public int Sum { get; set; }
        
}