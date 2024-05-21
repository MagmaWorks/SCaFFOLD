using System.Diagnostics;
using OasysUnits;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Abstract;

public abstract class CalcValue<T> : ICalcValue
{
    protected CalcValue(string name)
    {
        DisplayName = name;

        for (var i = 0; i < 5; i++)
        {
            var trace = new StackFrame(i);
            var method = trace.GetMethod();
            
            Direction = method?.Name switch
            {
                "DefineInputs" => IoDirection.Input,
                "DefineOutputs" => IoDirection.Output,
                _ => IoDirection.Undefined
            };
            
            if (Direction != IoDirection.Undefined)
                break;
        }
    }

    public IoDirection Direction { get; }
    public string DisplayName { get; }
    public string Symbol { get; protected set; }
    public string UnitName => Unit?.QuantityInfo.Name ?? "";
    public CalcStatus Status { get; set; }
    public T Value { get; set; }
    public IQuantity Unit { get; set; }
        
    public abstract void SetValue(string strValue);
    public virtual string GetValue(string format = "") => Value.ToString();
    public abstract override string ToString();
}