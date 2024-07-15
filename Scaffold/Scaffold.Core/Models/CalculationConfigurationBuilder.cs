using System.Linq.Expressions;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Models;

public class CalculationConfigurationBuilder<T>(T configurationContext)
    where T : class, ICalculation
{
    private class ContextMember
    {
        public ContextMember(string name, Type type)
        {
            Name = name;
            Type = type;
        }
        
        public string Name { get; set; }
        public Type Type { get; set; }
    }
    
    private T ConfigurationContext { get; } = configurationContext;
    private List<ContextMember> Members { get; } = [];
    
    internal List<ICalcValue> Inputs { get; } = [];
    internal List<ICalcValue> Outputs { get; } = [];
    
    private static void ThrowIfNotCalcValue(Type type)
    {
        var isCalcValue = type.GetInterface(nameof(ICalcValue)) != null;
        if (isCalcValue == false)
            throw new ArgumentException("Only properties and fields based on ICalcValue can be defined with calculation configuration.");
    }

    private void ReadNewExpression(NewExpression expression)
    {
        if (expression.Members == null)
            return;
        
        foreach (var arg in expression.Arguments)
        {
            ThrowIfNotCalcValue(arg.Type);

            var memberName = expression.Members[expression.Arguments.IndexOf(arg)].Name;
            Members.Add(new ContextMember(memberName, expression.Type));
        }
    }

    private void ReadMemberExpression(MemberExpression expression)
    {
        ThrowIfNotCalcValue(expression.Type);

        var member = new ContextMember(expression.Member.Name, expression.Type);
        Members.Add(member);
    }

    private ICalcValue GetCalcValue(ContextMember member)
    {
        var memberInfo = ConfigurationContext.GetType().GetProperty(member.Name);
        return memberInfo?.GetValue(ConfigurationContext) as ICalcValue;
    }

    private void AddToCalcValueCollection(List<ICalcValue> collection)
    {
        foreach (var member in Members)
        {
            var calcValue = GetCalcValue(member);
            calcValue.DisplayName ??= member.Name;
            collection.Add(calcValue);
        }
    }
    
    public void AsInput() => AddToCalcValueCollection(Inputs);
    public void AsOutput() => AddToCalcValueCollection(Outputs);
    public void SetTitle(string title) => SetMetadata(title, null);
    public void SetType(string type) => SetMetadata(null, type);
    
    public void SetMetadata(string title, string type)
    {
        var fallbackValue = typeof(T).Name;
        
        ConfigurationContext.Title = title ?? fallbackValue;
        ConfigurationContext.Type = type ?? fallbackValue;
    }
    
    public CalculationConfigurationBuilder<T> Define<TProperty>(Expression<Func<T, TProperty>> expression)
    {
        Members.Clear();
        
        switch (expression.Body)
        {
            case NewExpression newExpression:
                ReadNewExpression(newExpression);
                break;
            
            case MemberExpression memberExpression:
                ReadMemberExpression(memberExpression);
                break;
            
            default:
                throw new ArgumentException("Expression type not supported for calculation configuration.");
        }

        return this;
    }
    
    public CalculationConfigurationBuilder<T> WithDisplayName(string name)
    {
        var i = 1;
        var useIndex = Members.Count > 1;
        
        foreach (var member in Members)
        {
            var calcValue = GetCalcValue(member);
            calcValue.DisplayName = useIndex ? $"{name} ({i})" : name;
            i++;
        }
        
        return this;
    }
    
    public CalculationConfigurationBuilder<T> WithSymbol(string symbol)
    {
        foreach (var member in Members)
        {
            var calcValue = GetCalcValue(member);
            calcValue.Symbol = symbol;
        }
        
        return this;
    }
}