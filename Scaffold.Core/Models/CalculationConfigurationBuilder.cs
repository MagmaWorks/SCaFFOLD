using System.Linq.Expressions;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Internals;
using Scaffold.Core.Static;

namespace Scaffold.Core.Models;

public abstract class CalculationConfigurationBuilderBase
{
    internal List<ICalculationParameter<object>> Inputs { get; } = new List<ICalculationParameter<object>>();
    internal List<ICalculationParameter<object>> Outputs { get; } = new List<ICalculationParameter<object>>();

    public CalculationConfigurationBuilderBase()
    {
    }
}

public class CalculationConfigurationBuilder<Calc> : CalculationConfigurationBuilderBase
    where Calc : class, ICalculation
{
    public CalculationConfigurationBuilder(Calc configurationContext)
    {
        ConfigurationContext = configurationContext;
    }

    private class ContextMember
    {
        public ContextMember(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public Type Type { get; }
        public ICalculationParameter<object> CalcValue { get; set; }
    }

    private Calc ConfigurationContext { get; }
    private List<ContextMember> Members { get; } = new List<ContextMember>();

    private static void ThrowIfNotCalcValueCapable(Type type)
    {
        if (type.IsAcceptedPrimitive())
            return;

        var isCalcValue = type.GetInterface(nameof(ICalculationParameter<object>)) != null;
        if (isCalcValue == false)
            throw new ArgumentException("Only properties and fields based on ICalcValue can be defined with calculation configuration.");
    }

    private void ReadNewExpression(NewExpression expression)
    {
        if (expression.Members == null)
            return;

        foreach (var arg in expression.Arguments)
        {
            ThrowIfNotCalcValueCapable(arg.Type);

            var memberName = expression.Members[expression.Arguments.IndexOf(arg)].Name;
            Members.Add(new ContextMember(memberName, arg.Type));
        }
    }

    private void ReadMemberExpression(MemberExpression expression)
    {
        ThrowIfNotCalcValueCapable(expression.Type);

        var member = new ContextMember(expression.Member.Name, expression.Type);
        Members.Add(member);
    }

    private ICalculationParameter<object> GetCalcValue(ContextMember member)
    {
        if (member.CalcValue != null)
            return member.CalcValue;

        if (member.Type.IsAcceptedPrimitive())
            member.CalcValue = new InternalCalcValue(ConfigurationContext, member.Type, member.Name);
        else
            member.CalcValue =
                ConfigurationContext
                    .GetType()
                    .GetProperty(member.Name)?
                    .GetValue(ConfigurationContext) as ICalculationParameter<object>;

        return member.CalcValue;
    }

    private void AddToCalcValueCollection(List<ICalculationParameter<object>> collection)
    {
        foreach (var member in Members)
        {
            var calcValue = GetCalcValue(member);
            calcValue.DisplayName ??= member.Name.SplitPascalCaseToString();

            collection.InsertCalcValue(calcValue);
        }
    }

    public void AsInput() => AddToCalcValueCollection(Inputs);
    public void AsOutput() => AddToCalcValueCollection(Outputs);

    public CalculationConfigurationBuilder<Calc> Define<TProperty>(Expression<Func<Calc, TProperty>> expression)
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

    public CalculationConfigurationBuilder<Calc> WithDisplayName(string name)
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

    public CalculationConfigurationBuilder<Calc> WithSymbol(string symbol)
    {
        foreach (var member in Members)
        {
            var calcValue = GetCalcValue(member);
            calcValue.Symbol = symbol;
        }

        return this;
    }
}
