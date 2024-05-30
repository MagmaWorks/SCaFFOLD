using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Scaffold.Core.Images.Models;
using Scaffold.Core.Models;
using Scaffold.XamlDesigner.Models;

namespace Scaffold.XamlDesigner;
// TODO: Swap visibilities with converters (e.g. when active project is or is not null)
public class MainWindowViewModel : INotifyPropertyChanged
{
    private Visibility _waitingForTabVisibility;
    private Visibility _activeProjectVisibility;
    private Visibility _notWatchingVisibility;
    private Visibility _watchingVisibility;
    private string _activeProjectPath;

    public MainWindowViewModel()
    {
        Watching = 
        [
            new TreeItem {Name = "AdditionCalculation", HasIcon = true, IsExpanded = true, Result = new CalculationResult
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
                        Formula.New("Narrative to appear above the expression")
                            .WithConclusion("Some text here")
                            .WithReference("Some ref here")
                            .AddExpression(@"\left(x^2 + 2 \cdot x + 2\right) = 0"),



                        Formula.New("2. Narrative to appear above the expression")
                            .WithConclusion("2. Some text here")
                            .WithReference("2. Some ref here")
                            .AddExpression(@"\left(x^2 + 2 \cdot x + 2\right) = 0"),


                        Formula.New("Final narrative").WithReference("3.a")
                    ]
                },
                Failure = null
            }},
            new TreeItem {Name = "SubtractionCalculation", HasIcon = true,IsExpanded = true, Result = new CalculationResult
            {
                CalculationDetail = null,
                Failure = new ExceptionDetail
                {
                    Message = "ArgumentException",
                    InnerException = "Full error message here"
                }
            }}
        ];

        ActiveProjectPath = "file.cs";
        NotWatchingVisibility = Visibility.Collapsed;
        WatchingVisibility = Visibility.Visible;
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

    public List<TreeItem> Watching { get; }
    
    public Visibility WaitingForTabVisibility
    {
        get => _waitingForTabVisibility;
        set => SetField(ref _waitingForTabVisibility, value);
    }

    public Visibility ActiveProjectVisibility
    {
        get => _activeProjectVisibility;
        set => SetField(ref _activeProjectVisibility, value);
    }

    public Visibility NotWatchingVisibility
    {
        get => _notWatchingVisibility;
        set => SetField(ref _notWatchingVisibility, value);
    }


    public Visibility WatchingVisibility
    {
        get => _watchingVisibility;
        set => SetField(ref _watchingVisibility, value);
    }


    public string ActiveProjectPath
    {
        get => _activeProjectPath;
        set
        {
            SetField(ref _activeProjectPath, value);
            WaitingForTabVisibility = value != null ? Visibility.Collapsed : Visibility.Visible;
            ActiveProjectVisibility = value != null ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}