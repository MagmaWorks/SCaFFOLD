using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// We need to bring the functionality provided by CalcCore into scope so add a 'using' statement:
using CalcCore;

namespace EssentialCalcs
{
    // The calculation class is named SimpleMoment and is based on the CalcBase class
    // CalcBase provides the functionality required for our SimpleMoment class to communicate with the rest of the WW Scaffold ecosystem
    // Two methods must be present - UpdateCalc() and GenerateFormulae(), plus a parameterless constructor

    // The below name is how it appears in the dropdown list.
    //
    //Unhide to appear in the dropdown list
    //Unhide to appear in the dropdown list
    //Unhide to appear in the dropdown list
    //
    //[CalcName("Simple Moment")]

    public class SimpleMoment : CalcBase
    {
        // We're going to need some values for our calc. Let's define them here. 

        //CalcDouble class objects will appear in the input and outputs of the scaffold app. To help keep track of them they can be named starting with a "_". 
        CalcDouble _span;
        CalcDouble _udl;
        CalcDouble _moment;
        CalcDouble _shear;

        //The below creates the narrative for the calc, the expressions formula list is what is printed
        List<Formula> expressions = new List<Formula>();

        // This is the parameterless constructor
        // A constructor creates an instance of the class - i.e. creates an object
        // Parameterless means it doesn't take any parameters, so the brackets are empty
        public SimpleMoment()
        {
            // we define our values here, creating them with 'inputvalues' and 'outputvalues'
            // these are 'factory' methods that ensure that the base class can properly manage our newly created values
            //All Calcdouble or Calclist values need to created in this way
            _span = inputValues.CreateDoubleCalcValue("span", "l", "m", 10);
            _udl = inputValues.CreateDoubleCalcValue("udl", "w", "kN/m", 10);
            _moment = outputValues.CreateDoubleCalcValue("moment", "M", "kNm", 0);
            _shear = outputValues.CreateDoubleCalcValue("Shear", "V", "kN", 0);

            // finally, call your UpdateCalc() method to run the calc for the first time
            UpdateCalc();
        }

        //The below generates the narative, expressions is defined as a list of formulas
        public override List<Formula> GenerateFormulae()
        {
            return expressions;
        }

        private void resetFields()
        {
            //to be used to reset fields each time a calc is updated, variables which are changed throughout the calculation i.e. "d" should be reset here
        }

        // this method is used to update your calc whenever input values are changed
        public override void UpdateCalc()
        {
            // reset the formule field. This ensures it will be regenerated with the GenerateFormulae method
            resetFields();
            formulae = null;
            expressions = new List<Formula>();

            // here's where your calc goes
            _moment.Value = (_udl.Value * Math.Pow(_span.Value, 2)) / 8;

            // create strings representing our calc
            Formula momentcalc = new Formula();
            momentcalc.Narrative = "Moment Calc";
            momentcalc.Expression.Add(String.Format("{0} = {1} {2}^2 / 8", _moment.Symbol, _udl.Symbol, _span.Symbol));
            momentcalc.Expression.Add(String.Format("{0} = {1} {2}^2 / 8", _moment.Symbol, _udl.Value, _span.Value));
            momentcalc.Expression.Add(String.Format("{0} = {1}{2}", _moment.Symbol, _moment.Value,_moment.Unit));
            expressions.Add(momentcalc);

            //another calc example
            _shear.Value = (_udl.Value * _span.Value) / 2;
            Formula shearCalc = new Formula();
            shearCalc.Narrative = "Shear Calc";
            shearCalc.Expression.Add(String.Format("{0} = {1}{2}/2", _shear.Symbol, _udl.Symbol, _span.Symbol));
            shearCalc.Expression.Add(String.Format("{0} = {1}{2}/2", _shear.Symbol, _udl.Value, _span.Value));
            shearCalc.Expression.Add(String.Format("{0} = {1}", _shear.Symbol, _shear.Value));

            //the below lines are further examples on what a formula can do
            //shearCalc.Conclusion = "Pass";
            //shearCalc.Ref = "EN1002";
            //shearCalc.Status = CalcStatus.PASS;

            expressions.Add(shearCalc);
        }
    }
}