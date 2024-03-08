using System.Diagnostics;
using OasysUnits;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Abstract;

public abstract class CalcValue<T> : ICalcValue
{
    protected CalcValue(string name)
    {
        Name = name;

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
        }
    }

    public IoDirection Direction { get; }
    public string Name { get; }
    public string Symbol { get; protected set; }
    public T Value { get; protected set; }
    public IQuantity Unit { get; set; }
    public CalcStatus Status { get; set; }
        
    public abstract void SetValue(string strValue);
    public abstract override string ToString();
}