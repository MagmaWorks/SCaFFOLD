using System.Runtime.Serialization;
using System.Windows;
using Microsoft.VisualStudio.Extensibility.UI;
using Scaffold.VisualStudio.Models.Scaffold;

namespace Scaffold.VisualStudio.Models.Xaml;

[DataContract]
public class DisplayValueDetail : NotifyPropertyChangedObject
{
    private Visibility _symbolVisibility;
    private string _symbol;

    public DisplayValueDetail(CalcValueDetail detail)
    {
        DisplayName = detail.DisplayName;
        Value = detail.Value;
        Status = detail.Status;
        Symbol = detail.Symbol;
        SymbolVisibility = string.IsNullOrEmpty(Symbol) ? Visibility.Collapsed : Visibility.Visible;
    }
    
    [DataMember] public string DisplayName { get; set; }
    [DataMember] public string Value { get; set; }
    [DataMember] public string Status { get; set; }
    
    [DataMember] 
    public Visibility SymbolVisibility
    {
        get => _symbolVisibility;
        set => SetProperty(ref _symbolVisibility, value);
    }
    
    [DataMember]
    public string Symbol
    {
        get => _symbol;
        set => SetProperty(ref _symbol, value);
    }
}