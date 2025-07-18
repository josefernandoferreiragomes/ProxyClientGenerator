﻿namespace SoapServer.Domain.Base;

[DataContract]
public class ResponseMetadata
{
    [DataMember]
    public bool Success { get; set; } = true;
    [DataMember]
    public string? ErrorMessage { get; set; }
    [DataMember]
    public int ErrorCode { get; set; } = 0;

}
