using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcCore;

namespace EssentialCalcs
{
    public class StrutBucklingCheck : CalcBase
    {
        List<Formula> expressions;
        CalcSelectionList _concGrade;
        CalcSelectionList _discontinuity;
        CalcDouble _availableStrutWidth;
        CalcDouble _nodeWidth;
        CalcDouble _strutLength;
        CalcDouble _strutForce;
        CalcDouble _burstingForce;
        CalcDouble _thickness;
        CalcDouble _tensionToReinforce;

        public StrutBucklingCheck()
        {
            initialise();
        }

        private void initialise()
        {
            _concGrade = inputValues.CreateCalcSelectionList("Concrete grade", "35", new List<string> { "30", "35", "40", "45", "50", "60", "70", "80", "90" });
            _discontinuity = inputValues.CreateCalcSelectionList("Discontinuity", "Full", new List<string> { "Full", "Partial" });
            _nodeWidth = inputValues.CreateDoubleCalcValue("Node width", "a", "mm", 300);
            _availableStrutWidth = inputValues.CreateDoubleCalcValue("Available strut width", "b", "mm", 600);
            _strutLength = inputValues.CreateDoubleCalcValue("Strut length", "H", "mm", 2000);
            _strutForce = inputValues.CreateDoubleCalcValue("Strut force", "F", "kN", 1000);
            _burstingForce = outputValues.CreateDoubleCalcValue("Bursting force", "T", "kN", 0);
            _thickness = inputValues.CreateDoubleCalcValue("Thickness", "t", "mm", 300);
            _tensionToReinforce = outputValues.CreateDoubleCalcValue("Tension to reinforce", "T_{tie}", "kN", 0);
        }

        public override List<Formula> GenerateFormulae()
        {
            return expressions;
        }

        public override void UpdateCalc()
        {
            formulae = null;
            expressions = new List<Formula>();

            double T = 0;
            double F = _strutForce.Value;
            double a = _nodeWidth.Value;
            double H = _strutLength.Value;
            double b = Math.Min(_availableStrutWidth.Value, _strutLength.Value / 2);

            if (_discontinuity.ValueAsString == "Full")
            {
                T = F * (1 - (0.7 * a / H)) / 4;
                expressions.Add(
                    Formula.FormulaWithNarrative("Calculate bursting tie force")
                    .AddFirstExpression("T = " + Math.Round(T, 2) + "kN")
                    );
            }
            else
            {
                T = F * (b - a) / (4 * b);
                expressions.Add(
                    Formula.FormulaWithNarrative("Calculate bursting tie force")
                    .AddFirstExpression("T = " + Math.Round(T, 2) + "kN")
                    );
            }

            var concProps = ConcProperties.ByGrade(_concGrade.ValueAsString);
            double fctd = concProps.fctk005 / 1.5; // 1.5 is partial factor for transient loads. alpha factored is default 1 and not used here
            double t = _thickness.Value;

            if (T * 1000 >= 0.3 * t * H * fctd)
            {
                expressions.Add(
                    Formula.FormulaWithNarrative("Check tie force")
                    .AddFirstExpression(@"T \geq 0.3tHf_{fctd}")
                    .AddExpression(@"T \geq " + Math.Round((0.3 * t * H * fctd) / 1000, 2))
                    .AddConclusion("Reinforcement required")
                    .AddStatus(CalcStatus.FAIL)
                    );
                _tensionToReinforce.Value = T;
            }
            else
            {
                expressions.Add(
                    Formula.FormulaWithNarrative("Check tie force")
                    .AddFirstExpression(@"T < 0.3tHf_{fctd}")
                    .AddExpression(@"T < " + Math.Round((0.3 * t * H * fctd) / 1000, 2))
                    .AddConclusion("OK")
                    .AddStatus(CalcStatus.PASS)
                    );
                _tensionToReinforce.Value = 0;
            }
            _burstingForce.Value = T;
        }
    }
}
