using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Abstract;

public class CalculationReader
{
    private List<ICalcValue> _inputs;
    private List<ICalcValue> _outputs;
    private List<ICalculation> LoadedCalculations { get; } = [];

    
    private void Load(ICalculation calculation)
    {
        var hasLoaded = LoadedCalculations.Any(x => ReferenceEquals(x, calculation));        
        if (hasLoaded)
            return;
        
         
        // calculation.DefineInputs();
        // calculation.DefineOutputs(); 
        // TODO: Set them to lists which can be returned by GetInputs and GetOutputs.
        
        LoadedCalculations.Add(calculation);
    }

    public IReadOnlyList<ICalcValue> GetInputs(ICalculation calculation)
    {
        Load(calculation);

        return null;
    }
}