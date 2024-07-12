using Scaffold.Core.Enums;

namespace Scaffold.Core.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class CalcValueTypeAttribute : Attribute
{
    public CalcValueType Type { get; set; }
    protected CalcValueTypeAttribute(){}
    
    public CalcValueTypeAttribute(CalcValueType type) => Type = type;
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class InputCalcValueAttribute : CalcValueTypeAttribute
{
    
    public InputCalcValueAttribute() => Type = CalcValueType.Input;
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class OutputCalcValueAttribute : CalcValueTypeAttribute
{
    public OutputCalcValueAttribute() => Type = CalcValueType.Output;
}