using System.Runtime.Serialization;

namespace Scaffold.VisualStudio.Models.Results;

// TODO: Error chunks as a list so we can add some margins.
[DataContract]
public class ErrorDetail
{
    [DataMember] public string Source { get; set; }
    [DataMember] public string Message { get; set; }
    [DataMember] public string InnerException { get; set; }
    [DataMember] public string StackTrace { get; set; }
}