using System.Runtime.Serialization;

namespace Scaffold.VisualStudio.Models.Scaffold;

[DataContract]
public class CalcValueDetail
{
    [DataMember] public string DisplayName { get; set; }
    [DataMember] public string Symbol { get; set; }
    [DataMember] public string Value { get; set; }
    [DataMember] public string Status { get; set; }
}