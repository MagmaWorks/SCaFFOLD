using System.Runtime.Serialization;
using Microsoft.VisualStudio.Extensibility.UI;

namespace Scaffold.VisualStudio.Models.Xaml;

[DataContract]
public class DisplayExpression : NotifyPropertyChangedObject
{
    private string _expression;

    [DataMember]
    public string Expression
    {
        get => _expression;
        set => SetProperty(ref _expression, value);
    }
}