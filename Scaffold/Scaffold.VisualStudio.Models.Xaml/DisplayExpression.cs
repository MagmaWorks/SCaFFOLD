using System.Runtime.Serialization;
using System.Windows;
using Microsoft.VisualStudio.Extensibility.UI;

namespace Scaffold.VisualStudio.Models.Xaml;

[DataContract]
public class DisplayExpression : NotifyPropertyChangedObject
{
    private Visibility _expressionVisibility;


    [DataMember]
    public string Expression { get; set; }

    [DataMember]
    public Visibility ExpressionVisibility
    {
        get => _expressionVisibility;
        set => SetProperty(ref _expressionVisibility, value);
    }
}