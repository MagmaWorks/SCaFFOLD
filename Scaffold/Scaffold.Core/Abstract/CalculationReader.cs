using System.Reflection;
using Scaffold.Core.Attributes;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Abstract;

public class CalculationReader
{
    private class CacheItem(ICalculation calculation, Type type)
    {
        public ICalculation Calculation { get; set; } = calculation;
        public Type Type { get; set; } = type;
        public List<ICalcValue> Inputs { get; set; } = [];
        public List<ICalcValue> Outputs { get; set; } = [];
    }

    private List<CacheItem> Cache { get; } = [];

    private static void ReadMetadata(CacheItem cacheItem)
    {
        var metadata = cacheItem.Type.GetCustomAttribute<CalcMetadataAttribute>();
        if (metadata == null)
            return;
        
        cacheItem.Calculation.Title = metadata.Title;
        cacheItem.Calculation.Type = metadata.TypeName;
    }
    
    private static void LoadByAttributes(CacheItem cacheItem)
    {
        foreach (var property in cacheItem.Type.GetProperties())
        {
            var baseAttribute = property.GetCustomAttribute<CalcValueTypeAttribute>();
            if (baseAttribute == null)
                continue;
            
            switch (baseAttribute.Type)
            {
                case CalcValueType.Input:
                    cacheItem.Inputs.Add(property.GetValue(cacheItem.Calculation) as ICalcValue);
                    break;
                
                case CalcValueType.Output:
                    cacheItem.Outputs.Add(property.GetValue(cacheItem.Calculation) as ICalcValue);
                    break;
                
                default:
                case CalcValueType.Undefined:
                    continue;
            }
        }
    }
    
    private CacheItem Load(ICalculation calculation)
    {
        var cachedCalculation = Cache.FirstOrDefault(x => ReferenceEquals(x.Calculation, calculation));        
        if (cachedCalculation != null)
            return cachedCalculation;

        var newCacheItem = new CacheItem(calculation, calculation.GetType());
        
        var isFluentConfiguration =
            newCacheItem.Type
                .GetInterfaces()
                .FirstOrDefault(x => x == typeof(ICalculationConfiguration<>)) != null;

        if (isFluentConfiguration)
        {
            // todo
        }
        else
        {
            LoadByAttributes(newCacheItem);
        }

        ReadMetadata(newCacheItem);
        Cache.Add(newCacheItem);
        
        return newCacheItem;
    }

    public IReadOnlyList<ICalcValue> GetInputs(ICalculation calculation)
    {
        var cached = Load(calculation);
        return cached.Inputs;
    }
    
    public IReadOnlyList<ICalcValue> GetOutputs(ICalculation calculation)
    {
        var cached = Load(calculation);
        return cached.Outputs;
    }
}