using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Scaffold.VisualStudio.Models;

namespace Scaffold.VisualStudio.XamlDesigner;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private Visibility _waitingForTabVisibility = Visibility.Visible;
    private Visibility _hasActiveProjectVisibility = Visibility.Collapsed;
    private Visibility _isWatchingVisibility = Visibility.Collapsed;
    private string _activeProjectPath;

    public MainWindowViewModel()
    {
        Watching = 
        [
            new TreeItem(new CalculationResult
            {
                CalculationDetail = new CalculationDetail
                {
                    Inputs =
                    [
                        new CalcValueDetail {DisplayName = "Left assignment", Value = "2", Symbol = "L"},
                        new CalcValueDetail {DisplayName = "Right assignment", Value = "3", Symbol = "R"}
                    ],
                    Outputs =
                    [
                        new CalcValueDetail {DisplayName = "Result", Value = "5"}
                    ],
                    Formulae =
                    [
                        new DisplayFormula(@"\left(x^2 + 2 \cdot x + 2\right) = 0")
                        {
                            Conclusion = "Some text here",
                            Ref = "Some ref here",
                            Narrative = "Narrative to appear above the expression"
                        },
                        
                        new DisplayFormula(@"\left(x^2 + 2 \cdot x + 2\right) = 0")
                        {
                            Conclusion = "2. Some text here",
                            Ref = "2. Some ref here",
                            Narrative = "2. Narrative to appear above the expression"
                        },
                        
                        new DisplayFormula(new List<string>()) {Ref = "3.a"}
                    ]
                },
                Failure = null
            }) {Name = "AdditionCalculation", HasIcon = true, IsExpanded = true},
            new TreeItem(new CalculationResult
            {
                CalculationDetail = null,
                Failure = new ErrorDetail
                {
                    Message = "ArgumentException",
                    InnerException = "Full error message here"
                }
            }) {Name = "SubtractionCalculation", HasIcon = true, IsExpanded = true}
        ];

        WaitingForTabVisibility = Visibility.Visible;
        ActiveProjectPath = "file.cs";
    }
    
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }
    
    public Visibility WaitingForTabVisibility
    {
        get => _waitingForTabVisibility;
        set => SetField(ref _waitingForTabVisibility, value);
    }
    
    public Visibility HasActiveProjectVisibility
    {
        get => _hasActiveProjectVisibility;
        set => SetField(ref _hasActiveProjectVisibility, value);
    }
    
    public Visibility IsWatchingVisibility
    {
        get => _isWatchingVisibility;
        set => SetField(ref _isWatchingVisibility, value);
    }

    public List<TreeItem> Watching { get; }
    
    public string ActiveProjectPath
    {
        get => _activeProjectPath;
        set
        {
            SetField(ref _activeProjectPath, value);
            HasActiveProjectVisibility = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}