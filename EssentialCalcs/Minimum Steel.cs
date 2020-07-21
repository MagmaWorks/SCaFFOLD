using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// We need to bring the functionality provided by CalcCore into scope so add a 'using' statement:
using CalcCore;



namespace EssentialCalcs
{
    // replace "Template" with the name of your calc. This name will also identify your calc when saving/loading.
    [CalcName("Minimum Steel calc")]
    public class Minimum_Steel : CalcBase
    {
        // define fields for your calc values here

        CalcSelectionList _ConcreteGrade;
        CalcDouble _depth;
        CalcDouble _Fy;
        CalcDouble _Asmin;
        Double Asmin7;
        Double Asmin9;
        CalcDouble _cover;
        CalcSelectionList _linkdia;
        CalcSelectionList _Bardia;
        double k1;
        double k2;
        double k3;
        double k4;
        double kc;
        double k;


        // parameterless constructor
        // this will be called when your calc is initialised
        public Minimum_Steel()
        {
            // initialise your calc values here
            // Symbols and units should are formatted as LaTeX
            //input values are created from the inputValues base class field
            _ConcreteGrade = inputValues.CreateCalcSelectionList("Concrete grade f_{ck}", "40", new List<string> { "30", "35", "40", "45", "50", "60", "70", "80", "90" });
            _depth = inputValues.CreateDoubleCalcValue("Depth", "h", "mm", 250);
            _Fy = inputValues.CreateDoubleCalcValue("Rebar yield", "f_{y}", "N/mm^2", 500);
            _cover = inputValues.CreateDoubleCalcValue("cover", "c", "mm", 30);
            _Bardia = inputValues.CreateCalcSelectionList("Min Bar diameter", "16", new List<string> { "10", "12", "16", "20", "25", "32", "40" });
            _linkdia = inputValues.CreateCalcSelectionList("Link allowance", "0", new List<string> { "0", "10", "12", "16", "20", "25", "32", "40" });


            //output values are created from the outputValues base class field. 
            _Asmin = outputValues.CreateDoubleCalcValue("Minimum Area", "A_{s,min}", "mm^2 / m", 10);

            // run your calc for the first time
            UpdateCalc();
        }


        public override List<Formula> GenerateFormulae()
        {
            throw new NotImplementedException();
            // don't worry about this if you are defining the formulae directly in your UpdateCalc() body
        }

        public override void UpdateCalc()
        {
            // reset the formulae
            formulae = new List<Formula>();

            //add your calc logic here
            //do something!
            Double secwdith = 1000;
            var concprop = ConcProperties.ByGrade(_ConcreteGrade.ValueAsString);
            Double linkdia = double.Parse(_linkdia.ValueAsString);
            Double bardia = double.Parse(_Bardia.ValueAsString);
            Double d = _depth.Value - _cover.Value - linkdia - bardia / 2;

            // in accordance with section 9.2.1.1
            Asmin9 = Math.Max((0.26 * concprop.fctm * secwdith * d) / (_Fy.Value), 0.0013 * secwdith * d);

            //add formulae and commentary as you work through
            formulae.Add(Formula.FormulaWithNarrative("Minimum steel for brittle failure")
                .AddExpression(_Asmin.Symbol + "= Min (0.26*f_{ctm}*b_{t}*d,0.0013*b_t*d)=" + Asmin9 + _Asmin.Unit)
                .AddRef("EN1992-1-1 9.2.1.1(1)")
                );

            //in accordance with section 7
            k1 = 0.8;
            k2 = 0.5;
            k3 = 3.4;
            k4 = 0.425;
            kc = 0.4;
            k = 1;

            //Double hcef = Math.Min(2.5 * (_depth.Value - d), Math.Min(_depth.Value / 2, (_depth.Value - x) / 3));
            Double hcef = 0.2 * _depth.Value;

            Double Aceff = secwdith * hcef;

            //minimum steel requirements
            Asmin7 = (kc * k * concprop.fctm * Aceff) / _Fy.Value;
            formulae.Add(Formula.FormulaWithNarrative("Minimum steel for cracking")
            .AddExpression("k_{c}=" + kc)
            .AddExpression("k=" + k)
            .AddExpression("A_{ct}=0.2*h=" + Aceff)
            .AddExpression(_Asmin.Symbol + "= (k_{c}*k*f_{ct,eff}*A_{ct})/f_{y} =" + Asmin7 + _Asmin.Unit)
            .AddRef("EN1992-1-1 7.3.2 (7.1)")
            );

            //worst case minimum
            _Asmin.Value = Math.Max(Asmin7, Asmin9);
            formulae.Add(Formula.FormulaWithNarrative("Minimum steel")
            .AddExpression(_Asmin.Symbol + "=" + _Asmin.Value + _Asmin.Unit)
            );

        }
    }
}
