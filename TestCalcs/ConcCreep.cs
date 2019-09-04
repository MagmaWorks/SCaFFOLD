using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcCore;

namespace TestCalcs
{
    public class ConcCreep : CalcCore.CalcBase
    {
        CalcDouble notionalcreepcoeff;
        CalcDouble creepCoeff;
        CalcSelectionList concGrade;
        CalcDouble meanCompStr;
        CalcDouble charCompStr;
        CalcDouble relativeHumidity;
        CalcDouble time0;
        CalcDouble time;
        CalcDouble _Ac;
        CalcDouble u;
        CalcDouble L;
        CalcDouble W;
        CalcDouble creepTimeCoeff;
        List<Formula> expressions = new List<Formula>();

        public double NotionalCreepCoefficient { get => notionalcreepcoeff.Value; }
        public double CreepCoefficient { get => creepCoeff.Value; }
        public double Ac { get => _Ac.Value; }
        public double U { get => u.Value; }
        public double CreepTimeCoeff { get => creepTimeCoeff.Value; }

        public override List<Formula> GenerateFormulae()
        {
            return expressions;
        }

        public ConcCreep()
        {
            initialise();
            UpdateCalc();
        }

        public ConcCreep(string grade, double rh, double t, double t_loadApplied, double l, double w)
        {
            initialise();
            concGrade.ValueAsString = grade;
            relativeHumidity.Value = rh;
            time.Value = t;
            time0.Value = t_loadApplied;
            L.Value = l;
            W.Value = w;
            UpdateCalc();
        }

        private void initialise()
        {
            concGrade = inputValues.CreateCalcSelectionList("Concrete grade", "35", new List<string> { "30", "35", "40", "45", "50", "60", "70", "80", "90" });
            notionalcreepcoeff = outputValues.CreateDoubleCalcValue("Notional creep coefficient", @"\varphi(t,t_0)", "", 0);
            charCompStr = outputValues.CreateDoubleCalcValue("Concrete characteristic compressive strength", "f_{ck}", @"N/mm^2", 43);
            meanCompStr = outputValues.CreateDoubleCalcValue("Concrete mean compressive strength", "f_{cm}", @"N/mm^2", 43);
            relativeHumidity = inputValues.CreateDoubleCalcValue("Relative humidity", "RH", "", 70);
            time0 = inputValues.CreateDoubleCalcValue("Time load applied", "t_0", "days", 28);
            time = inputValues.CreateDoubleCalcValue("Time", "t", "days", 10000000);
            L = inputValues.CreateDoubleCalcValue("Length", "L", "mm", 500);
            W = inputValues.CreateDoubleCalcValue("Width", "W", "mm", 500);
            _Ac = outputValues.CreateDoubleCalcValue("Cross section area", "A_c", "mm^2", 250000);
            u = outputValues.CreateDoubleCalcValue("Section perimeter", "u", "mm", 2000);
            creepTimeCoeff = outputValues.CreateDoubleCalcValue("Coefficient for creep with time", @"\beta(t,t_0)", "", 0);
            creepCoeff = outputValues.CreateDoubleCalcValue("Creep coefficient", @"\varphi_0", "", 0);
        }

        public override void UpdateCalc()
        {
            formulae = null;
            expressions = new List<Formula>();
            var concProps = ConcProperties.ByGrade(concGrade.ValueAsString);

            charCompStr.Value = concProps.fck;
            meanCompStr.Value = concProps.fcm;

            _Ac.Value = W.Value * L.Value;
            u.Value = 2d * W.Value + 2d * L.Value;

            double factorRH = 0;
            double betafcm = 0;
            double betat0 = 0;
            double betaH = 0;
            double h0 = 2 * _Ac.Value / u.Value;
            double alpha1 = Math.Pow((35 / meanCompStr.Value), 0.7);
            double alpha2 = Math.Pow((35 / meanCompStr.Value), 0.2);
            double alpha3 = Math.Pow((35 / meanCompStr.Value), 0.5);
            expressions.Add(
                Formula.FormulaWithNarrative("Calculate alpha values")
                .AddExpression(@"\alpha_1=\left[ \frac{35}{f_{cm}} \right]^{0.7}" + Math.Round(alpha1,2))
                .AddExpression(@"\alpha_2=\left[ \frac{35}{f_{cm}} \right]^{0.2}" + Math.Round(alpha2, 2))
                .AddExpression(@"\alpha_3=\left[ \frac{35}{f_{cm}} \right]^{0.5}" + Math.Round(alpha3, 2))
                .AddRef("B.8c")
                );

            if (meanCompStr.Value <= 35)
            {
                factorRH = 1 + (1 - relativeHumidity.Value / 100) / (0.1 * Math.Pow(h0, (double)(1d / 3d)));
                expressions.Add(
                    Formula.FormulaWithNarrative("Calculate factor to allow for effect of relative humidity")
                    .AddRef("B.3a")
                    .AddExpression(@"f_{cm}\leq35 \Rightarrow")
                    .AddExpression(@"\phi_{RH}=1+\frac{1-RH/100}{0.1\sqrt[3]{h_0}}")
                    );
            }
            else
            {
                factorRH = (1 + ((1 - relativeHumidity.Value / 100) / (0.1 * Math.Pow(h0, (double)(1d / 3d)))) * alpha1) * alpha2;
                expressions.Add(
                    Formula.FormulaWithNarrative("Calculate factor to allow for effect of relative humidity")
                    .AddRef("B.3b")
                    .AddExpression(@"f_{cm}>35 \Rightarrow")
                    .AddExpression(@"\phi_{RH}=\left[ 1+\frac{1-RH/100}{0.1\sqrt[3]{h_0}}\alpha_1 \right]\alpha_2")
                    );
            }

            betafcm = 16.8 / Math.Sqrt(meanCompStr.Value);
            expressions.Add(
                Formula.FormulaWithNarrative("")
                .AddExpression(@"\beta(f_{cm})=\frac{16.8}{\sqrt{f_{cm}}}="+Math.Round(betafcm,2))
                .AddRef("B.4")
                );

            betat0 = 1 / (0.1 + Math.Pow(time0.Value, 0.20));
            expressions.Add(
                Formula.FormulaWithNarrative("Factor to allow for effect of concrete" +
                "strength on the notional creep coefficient")
                .AddExpression(@"\beta(f_{cm})")
                );

            if (meanCompStr.Value <= 35)
            {
                betaH = Math.Min(1.5 * (1 + Math.Pow((0.012 * relativeHumidity.Value), 18)) * h0 + 250, 1500);
                expressions.Add(
                    Formula.FormulaWithNarrative("Calculate coefficient depending on relative humidity and notional member size.")
                    .AddExpression(meanCompStr.Symbol + @"\leq 35 \Rightarrow")
                    .AddExpression(@"\beta_H = 1.5\left[ 1 + (0.012 RH)^{18} \right]h_0 + 250 \leq 1500")
                    .AddRef("B.8a")
                    );
            }
            else
            {
                betaH = Math.Min(1.5 * (1 + Math.Pow((0.012 * relativeHumidity.Value), 18)) * h0 + 250 * alpha3, 1500 * alpha3);
                expressions.Add(
                    Formula.FormulaWithNarrative("Calculate coefficient depending on relative humidity and notional member size.")
                    .AddExpression(meanCompStr.Symbol + @"> 35 \Rightarrow")
                    .AddExpression(@"\beta_H = 1.5\left[ 1 + (0.012 RH)^{18} \right]h_0 + 250\alpha_3 \leq 1500\alpha_3")
                    .AddRef("B.8b")
                    );
            }

            notionalcreepcoeff.Value = factorRH * betafcm * betat0;

            creepTimeCoeff.Value = Math.Pow((time.Value - time0.Value) / (betaH + time.Value - time0.Value), 0.3);

            creepCoeff.Value = notionalcreepcoeff.Value * creepTimeCoeff.Value;
        }
    }
}
