using System.Reflection;
using Scaffold.Core.Attributes;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;

namespace Scaffold.Core.Abstract;

// TODO: This model should determine if it calculates on each change or later in the pipeline.
public abstract class CalcBase : ICalc
{
    private List<ICalcValue> _inputs;
    private List<ICalcValue> _outputs;
    protected List<Formula> _formulae;

    protected CalcBase()
    {
        var classType = GetType();
        var attr = classType.GetCustomAttribute<CalcMetadataAttribute>();

        Title = attr?.Title ?? classType.Name;
        Type = attr?.TypeName ?? classType.Name;
        Status = CalcStatus.None;
    }
    
    public string Title { get; set; }
    public string Type { get; }
    public CalcStatus Status { get; protected set; }

    public IReadOnlyList<ICalcValue> Inputs => _inputs;
    public IReadOnlyList<ICalcValue> Outputs => _outputs;
    public List<Formula> GetFormulae() => _formulae ??= GenerateFormulae();

    protected abstract void DefineInputs();
    protected abstract void DefineOutputs();
    protected abstract void Update();

    protected abstract List<Formula> GenerateFormulae();

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
            if (!typeof(CalcValue<>).IsAssignableFrom(prop.PropertyType))
                continue;

            var value = (ICalcValue)prop.GetValue(this);
            switch (value?.Direction)
            {
                case IoDirection.Input:
                    _inputs.Add(value);
                    break;

                case IoDirection.Output:
                    _outputs.Add(value);
                    break;

                default:
                    continue;
            }
        }
    }
}