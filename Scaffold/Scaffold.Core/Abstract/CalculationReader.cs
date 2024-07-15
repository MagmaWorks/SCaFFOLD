using System.Reflection;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using Scaffold.Core.Static;

namespace Scaffold.Core.Abstract;

public class CalculationReader
{
    private class CacheItem(ICalculation calculation, Type type)
    {
        public ICalculation Calculation { get; } = calculation;
        public Type Type { get; } = type;
        public List<ICalcValue> Inputs { get; } = [];
        public List<ICalcValue> Outputs { get; } = [];
    }

    private List<CacheItem> Cache { get; } = [];

    private static void ReadMetadata(CacheItem cacheItem)
    {
        var fallbackValue = cacheItem.Type.Name.SplitPascalCaseToString();
        
        var metadata = cacheItem.Type.GetCustomAttribute<CalcMetadataAttribute>();
        if (metadata == null)
        {
            cacheItem.Calculation.Title = fallbackValue;
            cacheItem.Calculation.Type = fallbackValue;
        }
        else
        {
            cacheItem.Calculation.Title = metadata.Title ?? fallbackValue;
            cacheItem.Calculation.Type = metadata.TypeName ?? fallbackValue;
        }
    }

    private static ICalcValue GetCalcValue(PropertyInfo property, CacheItem cacheItem)
    {
        if (property.GetValue(cacheItem.Calculation) is not ICalcValue calcValue)
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
        ReadMetadata(cacheItem);
        
        foreach (var property in cacheItem.Type.GetProperties())
        {
            var baseAttribute = property.GetCustomAttribute<CalcValueTypeAttribute>();
            if (baseAttribute == null)
                continue;

            var calcValue = GetCalcValue(property, cacheItem);
            if (calcValue == null)
                continue;
            
            switch (baseAttribute.Type)
            {
                case CalcValueType.Input:
                    cacheItem.Inputs.Add(calcValue);
                    break;
                
                case CalcValueType.Output:
                    cacheItem.Outputs.Add(calcValue);
                    break;
                
                default:
                case CalcValueType.Undefined:
                    continue;
            }
        }
    }
    
    private CacheItem Load<T>(T calculation) where T : class, ICalculation
    {
        var cachedCalculation = Cache.FirstOrDefault(x => ReferenceEquals(x.Calculation, calculation));        
        if (cachedCalculation != null)
            return cachedCalculation;

        var newCacheItem = new CacheItem(calculation, calculation.GetType());

        var isFluentConfiguration =
            newCacheItem.Type
                .GetInterfaces()
                .FirstOrDefault(x => x.FullName != null && x.FullName.Contains(nameof(ICalculationConfiguration<T>))) != null;

        if (isFluentConfiguration)
        {
            var configurationBuilder = new CalculationConfigurationBuilder<T>(calculation);
            var configurable = (ICalculationConfiguration<T>) calculation;
            configurable.Configure(configurationBuilder);
            
            newCacheItem.Inputs.AddRange(configurationBuilder.Inputs);
            newCacheItem.Outputs.AddRange(configurationBuilder.Outputs);
        }
        else
        {
            LoadByAttributes(newCacheItem);
        }
        
        Cache.Add(newCacheItem);
        
        return newCacheItem;
    }

    public IReadOnlyList<ICalcValue> GetInputs<T>(T calculation) where T : class, ICalculation
    {
        var cached = Load(calculation);
        return cached.Inputs;
    }
    
    public IReadOnlyList<ICalcValue> GetOutputs<T>(T calculation) where T : class, ICalculation
    {
        var cached = Load(calculation);
        return cached.Outputs;
    }
    
    /// <summary>
    /// Wrapper implemented to keep all reading features together.
    /// This method can be called directly on the calculation also.
    /// </summary>
    public IEnumerable<Formula> GetFormulae<T>(T calculation)  where T : class, ICalculation
    {
        var cached = Load(calculation);
        return cached.Calculation.GetFormulae();
    }
    
    /// <summary>
    /// Wrapper implemented to keep all reading features together.
    /// This method can be called directly on the calculation also.
    /// </summary>
    public void Update<T>(T calculation)  where T : class, ICalculation
    {
        var cached = Load(calculation);
        cached.Calculation.Update();
    }
}