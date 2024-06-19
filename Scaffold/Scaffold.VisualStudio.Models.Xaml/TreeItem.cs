using System.Runtime.Serialization;
using System.Windows;
using Microsoft.VisualStudio.Extensibility.UI;
using Scaffold.VisualStudio.Models.Results;

namespace Scaffold.VisualStudio.Models.Xaml;

[DataContract]
public class TreeItem : NotifyPropertyChangedObject
{
    private Visibility _isSuccessVisibility;
    private Visibility _isFailedVisibility;
    private Visibility _isExpandedVisibility;
    private Visibility _isCollapsedVisibility;
    private bool _isExpanded;
    private string _name;

    private TreeItem()
    {
        ChangeTreeItemExpansionCommand = new AsyncCommand((_, _, _) =>
        {
            IsExpanded = !IsExpanded;
            return Task.CompletedTask;
        });
    }

    public TreeItem(CalculationResult<DisplayFormula> result) : this()
    {
        Name = result.IsSuccess ? result.CalculationDetail.Title : result.Failure.Source ?? "Unhandled exception";
        AssemblyQualifiedTypeName = result.AssemblyQualifiedTypeName;
        IsSuccessVisibility = result.IsSuccess ? Visibility.Visible : Visibility.Collapsed;
        IsFailedVisibility = result.IsSuccess ? Visibility.Collapsed : Visibility.Visible;
        IsExpanded = false;

        SetLists(result);
    }

    public TreeItem(ErrorDetail error) : this()
    {
        Name = "Run failed";
        
        Error = error;
        ErrorSourceVisibility = string.IsNullOrEmpty(Error.Source) ? Visibility.Collapsed : Visibility.Visible;
        ErrorMessageVisibility = string.IsNullOrEmpty(Error.Message) ? Visibility.Collapsed : Visibility.Visible;
        ErrorInnerExceptionVisibility = string.IsNullOrEmpty(Error.InnerException) ? Visibility.Collapsed : Visibility.Visible;
        ErrorStackTraceVisibility = string.IsNullOrEmpty(Error.StackTrace) ? Visibility.Collapsed : Visibility.Visible;
        
        IsSuccessVisibility = Visibility.Collapsed;
        IsFailedVisibility = Visibility.Visible;
        IsExpanded = true;
    }
    
    [DataMember] public string AssemblyQualifiedTypeName { get; set; }
    [DataMember] public ObservableList<DisplayValueDetail> Inputs { get; } = [];
    [DataMember] public ObservableList<DisplayValueDetail> Outputs { get; } = [];
    [DataMember] public ObservableList<DisplayFormula> Formulae { get; } = [];
    [DataMember] public ErrorDetail Error { get; }
    // TODO: Move these into the detail class, once more is merged together.
    [DataMember] public Visibility ErrorSourceVisibility { get; set; }
    [DataMember] public Visibility ErrorMessageVisibility { get; set; }
    [DataMember] public Visibility ErrorInnerExceptionVisibility { get; set; }
    [DataMember] public Visibility ErrorStackTraceVisibility { get; set; }
    
    [DataMember] public AsyncCommand ChangeTreeItemExpansionCommand { get; set; }

    [DataMember]
    public Visibility IsSuccessVisibility
    {
        get => _isSuccessVisibility;
        set => SetProperty(ref _isSuccessVisibility, value);
    }

    [DataMember]
    public Visibility IsFailedVisibility
    {
        get => _isFailedVisibility;
        set => SetProperty(ref _isFailedVisibility, value);
    }

    [DataMember]
    public Visibility IsExpandedVisibility
    {
        get => _isExpandedVisibility;
        set => SetProperty(ref _isExpandedVisibility, value);
    }

    [DataMember]
    public Visibility IsCollapsedVisibility
    {
        get => _isCollapsedVisibility;
        set => SetProperty(ref _isCollapsedVisibility, value);
    }

    [DataMember]
    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            SetProperty(ref _isExpanded, value);
            IsExpandedVisibility = _isExpanded ? Visibility.Visible : Visibility.Collapsed;
            IsCollapsedVisibility = _isExpanded ? Visibility.Collapsed : Visibility.Visible;
        }
    }

    [DataMember]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private void SetLists(CalculationResult<DisplayFormula> result)
    {
        foreach (var input in result.CalculationDetail.Inputs)
            Inputs.Add(new DisplayValueDetail(input));

        foreach (var output in result.CalculationDetail.Outputs)
            Outputs.Add(new DisplayValueDetail(output));

        foreach (var formula in result.CalculationDetail.Formulae)
        {
            var newFormula = new DisplayFormula
            {
                Ref = formula.Ref,
                Narrative = formula.Narrative,
                Conclusion = formula.Conclusion,
                Status = formula.Status
            };
            
            newFormula.ExpressionVisibility = newFormula.Expressions is { Count: 0 }
                ? Visibility.Collapsed : Visibility.Visible;
            
            Formulae.Add(newFormula);
        }
        

    }

    public void SetExpanderState(bool alwaysExpandCalculations, TreeItem existingTreeItem)
    {
        if (alwaysExpandCalculations)
        {
            IsExpanded = true;
        }
        else
        {
            IsExpanded = existingTreeItem is { IsExpanded: true };
        }
    }
}