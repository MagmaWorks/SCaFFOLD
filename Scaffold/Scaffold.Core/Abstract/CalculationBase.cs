using System.Reflection;
using Scaffold.Core.Attributes;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;

namespace Scaffold.Core.Abstract;

// TODO: This model should determine if it calculates on each change or later in the pipeline.
public abstract class CalculationBase : ICalculation, ICalculationMetadata
{
    private List<ICalcValue> _inputs;
    private List<ICalcValue> _outputs;
    private IEnumerable<Formula> _formulae;

    protected CalculationBase()
    {
        var metadata = GetType().GetMetadata();
        
        Title = metadata.Title;
        Type = metadata.Type;
        Status = CalcStatus.None;
    }
    
    public string Title { get; set; }
    public string Type { get; }
    public CalcStatus Status { get; protected set; }
    
    public IReadOnlyList<ICalcValue> GetInputs() => _inputs ?? [];
    public IReadOnlyList<ICalcValue> GetOutputs() => _outputs ?? [];
    public IEnumerable<Formula> GetFormulae() => _formulae ??= GenerateFormulae();

    protected abstract void DefineInputs();
    protected abstract void DefineOutputs();
    public abstract void Update();

    protected abstract IEnumerable<Formula> GenerateFormulae();

    private static bool IsCalcValueType(Type type)
    {
        while (true)
        {
            if (type.BaseType == null) 
                return false;

            if (type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(CalcValue<>)) 
                return true;

            type = type.BaseType;
        }
    }

    public void LoadIoCollections()
    {
        if (_inputs != null && _outputs != null)
            return;

        _inputs = [];
        _outputs = [];

        DefineInputs();
        DefineOutputs();

        var allProps = GetType().GetRuntimeProperties();
        foreach (var prop in allProps)
        {
            if (IsCalcValueType(prop.PropertyType) == false)
                continue;

            var value = (ICalcValue)prop.GetValue(this);
            
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (value?.Direction)
            {
                case IoDirection.Input:
                    _inputs.Add(value);
                    break;

                case IoDirection.Output:
                    _outputs.Add(value);
                    break;
            }
        }
    }
}

public static class CalculationBaseExtensions
{
    public static ICalculationMetadata GetMetadata(this Type implementationType)
    {
        var attr = implementationType.GetCustomAttribute<CalcMetadataAttribute>();

        return new CalculationMetadata
        {
            Title = attr?.Title ?? implementationType.Name,
            Type = attr?.TypeName ?? implementationType.Name
        };
    }
}