using SoapServer.Services.Base;

namespace SoapServer.Services.Calculator;

[DataContract]
public class SumListResponse: BaseResponse
{
    [DataMember]
    public int Sum { get; set; }
        
}