using System.Reflection;
using Scaffold.Core.Attributes;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Internals;
using Scaffold.Core.Models;
using Scaffold.Core.Static;

namespace Scaffold.Core.Abstract;

public class CalculationReader
{
    private class CacheItem
    {
        public ICalculation Calculation { get; }
        public Type CalcType { get; }
        public List<ICalculationParameter<object>> Inputs { get; }
        public List<ICalculationParameter<object>> Outputs { get; }

        public CacheItem(ICalculation calculation)
        {
            CalcType = calculation.GetType();
            Calculation = calculation;
            Inputs = new List<ICalculationParameter<object>>();
            Outputs = new List<ICalculationParameter<object>>();
        }
    }

    private List<CacheItem> Cache { get; } = new List<CacheItem>();

    private static void ReadMetadata(Type type, ICalculation calculation)
    {
        var fallbackValue = type.Name.SplitPascalCaseToString();

        var metadata = type.GetCustomAttribute<CalculationMetadataAttribute>();
        if (metadata == null)
        {
            calculation.ReferenceName = fallbackValue;
            calculation.CalculationName = fallbackValue;
        }
        else
        {
            calculation.ReferenceName = metadata.ReferenceName ?? fallbackValue;
            calculation.CalculationName = metadata.CalculationName ?? fallbackValue;
        }
    }

    private static ICalculationParameter<object> GetCalcValue(PropertyInfo property, CacheItem cacheItem)
    {
        if (property.GetValue(cacheItem.Calculation) is not ICalculationParameter<object> calcValue)
        {
            var propertyType = property.PropertyType;
            if (propertyType.IsAcceptedPrimitive() == false)
                return null;

            calcValue = new InternalCalcValue(cacheItem.Calculation, propertyType, property.Name);
        }

        calcValue.DisplayName ??= property.Name.SplitPascalCaseToString();
        return calcValue;
    }

    private static void LoadByAttributes(CacheItem cacheItem)
    {
        foreach (var property in cacheItem.CalcType.GetProperties())
        {
            var baseAttribute = property.GetCustomAttribute<CalcValueTypeAttribute>();
            if (baseAttribute == null)
                continue;

            var calcValue = GetCalcValue(property, cacheItem);
            if (calcValue == null)
                continue;

            calcValue.DisplayName = baseAttribute.DisplayName ?? calcValue.DisplayName;
            calcValue.Symbol = baseAttribute.Symbol ?? calcValue.Symbol;

            switch (baseAttribute.Type)
            {
                case CalcValueType.Input:
                    cacheItem.Inputs.InsertCalcValue(calcValue);
                    break;

                case CalcValueType.Output:
                    cacheItem.Outputs.InsertCalcValue(calcValue);
                    break;

                default:
                case CalcValueType.Undefined:
                    continue;
            }
        }
    }

    private CacheItem GetCachedItem<Calc>(Calc calculation) where Calc : class, ICalculation
    {
        var cachedCalculation = Cache.FirstOrDefault(x => ReferenceEquals(x.Calculation, calculation));
        if (cachedCalculation != null)
            return cachedCalculation;

        var newCacheItem = new CacheItem(calculation);

        var isFluentConfiguration =
            newCacheItem.CalcType
                .GetInterfaces()
                .FirstOrDefault(x => x.FullName != null && x.FullName.Contains(typeof(ICalculationConfiguration<>).Name)) != null;

        if (isFluentConfiguration)
        {
            var builderType = typeof(CalculationConfigurationBuilder<Calc>).MakeGenericType(calculation.GetType());
            var configurationBuilder = Activator.CreateInstance(builderType, calculation);

            var configurationType = typeof(ICalculationConfiguration<>).MakeGenericType(calculation.GetType());
            configurationType.GetMethod("Configure")?.Invoke(calculation, new object[] { configurationBuilder });

            if (configurationBuilder is CalculationConfigurationBuilderBase baseBuilder)
            {
                newCacheItem.Inputs.AddRange(baseBuilder.Inputs);
                newCacheItem.Outputs.AddRange(baseBuilder.Outputs);
            }
        }
        else
        {
            LoadByAttributes(newCacheItem);
        }

        ReadMetadata(newCacheItem.CalcType, newCacheItem.Calculation);
        Cache.Add(newCacheItem);

        return newCacheItem;
    }

    /// <summary>
    /// Gets metadata, loading calculation from/into cache for later manipulation.
    /// </summary>
    public CalculationMetadata GetMetadata<Calc>(Calc calculation) where Calc : class, ICalculation
    {
        var cached = GetCachedItem(calculation);
        return new CalculationMetadata { Title = cached.Calculation.ReferenceName, Type = cached.Calculation.CalculationName };
    }

    /// <summary>
    /// Gets inputs, loading calculation from/into cache for later manipulation.
    /// </summary>
    public IReadOnlyList<ICalculationParameter<object>> GetInputs<Calc>(Calc calculation) where Calc : class, ICalculation
    {
        var cached = GetCachedItem(calculation);
        return cached.Inputs;
    }

    /// <summary>
    /// Gets outputs, loading calculation from/into cache for later manipulation.
    /// </summary>
    public IReadOnlyList<ICalculationParameter<object>> GetOutputs<Calc>(Calc calculation) where Calc : class, ICalculation
    {
        var cached = GetCachedItem(calculation);
        return cached.Outputs;
    }

    /// <summary>
    /// Gets formulae, loading calculation from/into cache for later manipulation.
    /// Wrapper implemented to keep all reading features together.
    /// This method can be called directly on the calculation also.
    /// </summary>
    public IEnumerable<Formula> GetFormulae<T>(T calculation) where T : class, ICalculation
    {
        var cached = GetCachedItem(calculation);
        return cached.Calculation.GetFormulae();
    }

    /// <summary>
    /// Updates the calculation, loading calculation from/into cache for later manipulation.
    /// Wrapper implemented to keep all reading features together.
    /// This method can be called directly on the calculation also.
    /// </summary>
    public void Update<T>(T calculation) where T : class, ICalculation
    {
        var cached = GetCachedItem(calculation);
        cached.Calculation.Update();
    }
}
