using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Scaffold.VisualStudio.Models;

// TODO: Images
public class DisplayFormula : INotifyPropertyChanged
{
    private Visibility _expressionVisibility;

    public DisplayFormula(IEnumerable<string> expressions)
    {
        Expressions = expressions?.ToList();
        ExpressionVisibility = Expressions is { Count: 0 }
            ? Visibility.Collapsed : Visibility.Visible;
    }
    
    public DisplayFormula(string expression) : this([expression]) { }

    public List<string> Expressions { get; }
    public string Ref { get; set; }
    public string Narrative { get; set; }
    public string Conclusion { get; set; } 
    public string Status { get; set; }

    public Visibility ExpressionVisibility
    {
        get => _expressionVisibility;
        set => SetField(ref _expressionVisibility, value);
    }
    
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}