namespace Scaffold.Core.Abstract;

public abstract class CalcMeasure : CalcValue<double>
{
    protected CalcMeasure(string name, string symbol) : base(name)
        => Symbol = symbol; 
        
    public abstract string Unit { get; }
}