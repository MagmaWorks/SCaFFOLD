using System.Runtime.Serialization;
using System.Windows;
using Microsoft.VisualStudio.Extensibility.UI;
using Scaffold.VisualStudio.Models.Scaffold;

namespace Scaffold.VisualStudio.Models.Xaml;

// TODO: Images
[DataContract]
public class DisplayFormula : NotifyPropertyChangedObject, IFormula
{
    private Visibility _expressionVisibility;

    public DisplayFormula(IEnumerable<string> expressions)
    {
        Expressions = expressions?.ToList();
        ExpressionVisibility = Expressions is { Count: 0 }
            ? Visibility.Collapsed : Visibility.Visible;
    }

    public DisplayFormula(string expression) : this([expression]) { }

    [DataMember] public List<string> Expressions { get; }
    [DataMember] public string Ref { get; set; }
    [DataMember] public string Narrative { get; set; }
    [DataMember] public string Conclusion { get; set; }
    [DataMember] public string Status { get; set; }

    [DataMember]
    public Visibility ExpressionVisibility
    {
        get => _expressionVisibility;
        set => SetProperty(ref _expressionVisibility, value);
    }
}