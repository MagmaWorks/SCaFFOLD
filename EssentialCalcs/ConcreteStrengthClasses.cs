using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcCore;

namespace EssentialCalcs
{
    [CalcName("Concrete classes")]
    [CalcAlternativeName("TestCalcs.ConcreteStrengthClasses")]
    public class ConcreteStrengthClasses : CalcBase
    {
        CalcSelectionList _grade;
        CalcDouble _fck;
        CalcDouble _fckcube;
        CalcDouble _fcm;
        CalcDouble _fctm;
        CalcDouble _fctk005;
        CalcDouble _fctk095;
        CalcDouble _Ecm;
        CalcDouble _Ec1;
        CalcDouble _Ecu1;
        CalcDouble _Ec2;
        CalcDouble _Ecu2;
        CalcDouble _n;
        CalcDouble _Ec3;
        CalcDouble _Ecu3;

        public ConcreteStrengthClasses()
        {
            _grade = inputValues.CreateCalcSelectionList("Grade", "30", new List<string> { "30", "35", "40", "45", "50", "55", "60", "70", "80", "90" });
            _fck = outputValues.CreateDoubleCalcValue("Characteristic cylinder strength", @"f_{ck}", "MPa", 0);
            _fckcube = outputValues.CreateDoubleCalcValue("Characteristic cube strength", @"f_{ck,cube}", "MPa", 0);
            _fcm = outputValues.CreateDoubleCalcValue("Mean cylinder strength", @"f_{cm}", "MPa", 0);
            _fctm = outputValues.CreateDoubleCalcValue("Mean tensile strength", @"f_{ctm}", "MPa", 0);
            _fctk005 = outputValues.CreateDoubleCalcValue("5th percentile tensile strength", @"f_{ctk,0.05}", "MPa", 0);
            _fctk095 = outputValues.CreateDoubleCalcValue("95th percentile tensile strength", @"f_{ctk,0.95}", "MPa", 0);
            _Ecm = outputValues.CreateDoubleCalcValue("Mean modulus of elasticity", @"E_{cm}", "GPa", 0);
            _Ec1 = outputValues.CreateDoubleCalcValue("Compressive strain at peak stress", @"\epsilon_{c1}", "", 0);
            _Ecu1 = outputValues.CreateDoubleCalcValue("Ultimate compressive strain", @"\epsilon_{cu1}", "", 0);
            _Ec2 = outputValues.CreateDoubleCalcValue("Compressive strain at peak stress", @"\epsilon_{c2}", "", 0);
            _Ecu2 = outputValues.CreateDoubleCalcValue("Ultimate compressive strain", @"\epsilon_{cu2}", "", 0);
            _n = outputValues.CreateDoubleCalcValue("Exponent", @"n", "", 0);
            _Ec3 = outputValues.CreateDoubleCalcValue("Compressive strain at peak stress", @"\epsilon_{c3}", "", 0);
            _Ecu3 = outputValues.CreateDoubleCalcValue("Ultimate compressive strain", @"\epsilon_{cu3}", "", 0);

            UpdateCalc();
        }

        public override List<Formula> GenerateFormulae()
        {
            formulae = new List<Formula>();
            formulae.Add(
                Formula.FormulaWithNarrative("Concrete strength properties from BS EN 1992-1 Table 3.1. Values are calculated using given formulae rather than using the rounded tabulated values.")
                .AddRef("T3.1")
                .AddExpression(_fck.Symbol +"="+ _fck.Value+_fck.Unit )
                .AddExpression(_fckcube.Symbol+"="+_fckcube.Value+_fckcube.Unit)
                .AddExpression(_fcm.Symbol + "=" + Math.Round(_fcm.Value, 3) + _fcm.Unit)
                .AddExpression(_fctm.Symbol+"="+Math.Round(_fctm.Value,3)+_fctm.Unit)
                .AddExpression(_fctk005.Symbol + "=" + Math.Round(_fctk005.Value, 3) + _fctk005.Unit)
                .AddExpression(_fctk095.Symbol + "=" + Math.Round(_fctk095.Value, 3) + _fctk095.Unit)
                .AddExpression(_Ecm.Symbol + "=" + Math.Round(_Ecm.Value, 3) + _Ecm.Unit)
                .AddExpression(_Ec1.Symbol + "=" + Math.Round(_Ec1.Value, 3) + _Ec1.Unit)
                .AddExpression(_Ecu1.Symbol + "=" + Math.Round(_Ecu1.Value, 3) + _Ecu1.Unit)
                .AddExpression(_Ec2.Symbol + "=" + Math.Round(_Ec2.Value, 3) + _Ec2.Unit)
                .AddExpression(_Ecu2.Symbol + "=" + Math.Round(_Ecu2.Value, 3) + _Ecu2.Unit)
                .AddExpression(_n.Symbol + "=" + Math.Round(_n.Value, 3) + _n.Unit)
                .AddExpression(_Ec3.Symbol + "=" + Math.Round(_Ec3.Value, 3) + _Ec3.Unit)
                .AddExpression(_Ecu3.Symbol + "=" + Math.Round(_Ecu3.Value, 3) + _Ecu3.Unit)
            );
            return formulae;
        }

        public override void UpdateCalc()
        {
            formulae = null;
            ConcProperties props = ConcProperties.ByGrade(_grade.ValueAsString);
            _fck.Value = props.fck;
            _fckcube.Value = props.fckcube;
            _fcm.Value = props.fcm;
            _fctm.Value = props.fctm;
            _fctk005.Value = props.fctk005;
            _fctk095.Value = props.fctk095;
            _Ecm.Value = props.Ecm;
            _Ec1.Value = props.Epsilonc1;
            _Ecu1.Value = props.Epsiloncu1;
            _Ec2.Value = props.Epsilonc2;
            _Ecu2.Value = props.Epsiloncu2;
            _n.Value = props.n;
            _Ec3.Value = props.Epsilonc3;
            _Ecu3.Value = props.Epsiloncu3;

        }
    }
}
