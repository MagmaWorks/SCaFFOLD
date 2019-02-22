using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// We need to bring the functionality provided by CalcCore into scope so add a 'using' statement:
using CalcCore;

namespace TestCalcs
{
    // The calculation class is named SimpleMoment and is based on the CalcBase class
    // CalcBase provides the functionality required for our SimpleMoment class to communicate with the rest of the WW calcs ecosystem
    // Two methods have to be present - UpdateCalc() and GenerateFormulae(), plus a parameterless constructor
    public class SimpleMoment : CalcBase
    {
        // We're going to need some values for our calc. Let's define them here
        CalcDouble span;
        CalcDouble udl;
        CalcDouble moment;
        CalcDouble shear;

        // This is the parameterless constructor
        // A constructor creates an instance of the class - i.e. creates an object
        // Parameterless means it doesn't take any parameters, so the brackets are empty
        public SimpleMoment()
        {
            // we define our values here, creating them with 'inputvalues' and 'outputvalues'
            // these are 'factory' methods that ensure that the base class can properly manage our newly created values
            span = inputValues.CreateDoubleCalcValue("span", "l", "m", 10);
            udl = inputValues.CreateDoubleCalcValue("udl", "w", "kN/m", 10);
            moment = outputValues.CreateDoubleCalcValue("moment", "M", "kNm", 0);
            shear = outputValues.CreateDoubleCalcValue("Shear", "V", "kN", 0);

            // finally, call your UpdateCalc() method to run the calc for the first time
            UpdateCalc();
        }
        
        // this method is used to update your calc whenever input values are changed
        public override void UpdateCalc()
        {
            // reset the formule field. This ensures it will be regenerated with the GenerateFormulae method
            formulae = null;

            // here's where your calc goes
            moment.Value = (udl.Value * Math.Pow(span.Value, 2)) / 8;
            shear.Value = (udl.Value * span.Value) / 2;
        }

        // this method generates formulae, including narrative and images if required
        public override List<Formula> GenerateFormulae()
        {
            // create strings representing our calc
            List<string> momentCalc = new List<string>();
            momentCalc.Add(String.Format("{0} = {1} {2}^2 / 8", moment.Symbol, udl.Symbol, span.Symbol));
            momentCalc.Add(String.Format("{0} = {1} {2}^2 / 8", moment.Symbol, udl.Value, span.Value));
            momentCalc.Add(String.Format("{0} = {1}", moment.Symbol, moment.Value));
            List<string> shearCalc = new List<string>();
            shearCalc.Add(String.Format("{0} = {1}{2}/2", shear.Symbol, udl.Symbol, span.Symbol));
            shearCalc.Add(String.Format("{0} = {1}{2}/2", shear.Symbol, udl.Value, span.Value));
            shearCalc.Add(String.Format("{0} = {1}", shear.Symbol, shear.Value));

            // output a list of Formula objects representing our calc
            return new List<Formula>()
            {
                new Formula(){Ref="Moment calc", Expression = momentCalc},
                new Formula(){Ref="Shear calc", Expression=shearCalc}
            };
        }
    }
}