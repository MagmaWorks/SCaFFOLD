using System;
using System.Collections.Generic;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using UnitsNet;

namespace Scaffold.Calculations
{
    public class ConcCreep : ICalculation
    {
        public string ReferenceName { get; set; } = "";
        public string CalculationName { get; set; } = "Test Calculation";
        public CalcStatus Status { get; set; } = CalcStatus.None;

        [InputCalcValue("Notional Creep Coefficient", @"\varphi(t,t_0)")]
        CalcDouble notionalcreepcoeff;
        [InputCalcValue("Concrete grade", "")]
        CalcSelectionList concGrade = new CalcSelectionList("Concrete grade", "35", new List<string> { "30", "35", "40", "45", "50", "60", "70", "80", "90" });
        [InputCalcValue]
        CalcStress charCompStr = new CalcStress(43, UnitsNet.Units.PressureUnit.NewtonPerSquareMillimeter, "Concrete characteristic Compressive strength", "f_{ck}");
        [InputCalcValue]
        CalcStress meanCompStr = new CalcStress(43, UnitsNet.Units.PressureUnit.NewtonPerSquareMillimeter, "Concrete mean compressive strength", "f_{cm}");
        [InputCalcValue]
        CalcDouble relativeHumidity = new CalcDouble(70, "Relative humidity", "RH");
        [InputCalcValue]
        CalcDouble time0 = new CalcDouble(28, @"Time load applied", @"t_0\");
        [InputCalcValue]
        CalcDouble time = new CalcDouble(10000000, "Time", "t");
        [InputCalcValue]
        CalcLength L = new CalcLength(500, UnitsNet.Units.LengthUnit.Millimeter, "Length", "L");
        [InputCalcValue]
        CalcLength W = new CalcLength(500, UnitsNet.Units.LengthUnit.Millimeter, "Width", "W");
        [OutputCalcValue]
        CalcArea _Ac = new CalcArea(250000, UnitsNet.Units.AreaUnit.SquareMillimeter, "Cross section area", "A_c");
        [OutputCalcValue]
        CalcLength u = new CalcLength(2000, UnitsNet.Units.LengthUnit.Millimeter, "Section perimeter", "u");
        [OutputCalcValue]
        CalcDouble creepTimeCoeff = new CalcDouble(0, "Coefficient for creep with time", @"\beta(t,t_0)");
        [OutputCalcValue]
        CalcDouble creepCoeff = new CalcDouble(0, "Creep coefficient", @"\varphi_0");

        //CalcDouble creepCoeff;
        //CalcSelectionList concGrade;
        //CalcDouble meanCompStr;
        //CalcDouble charCompStr;
        //CalcDouble relativeHumidity;
        //CalcDouble time0;
        //CalcDouble time;
        //CalcDouble _Ac;
        //CalcDouble u;
        //CalcDouble L;
        //CalcDouble W;
        //CalcDouble creepTimeCoeff;

        List<IFormula> expressions = new List<IFormula>();

        //public double NotionalCreepCoefficient { get => notionalcreepcoeff.Value; }
        //public double CreepCoefficient { get => creepCoeff.Value; }
        //public double Ac { get => _Ac.Value; }
        //public double U { get => u.Value; }
        //public double CreepTimeCoeff { get => creepTimeCoeff.Value; }

        //public override List<Formula> GenerateFormulae()
        //{
        //    return expressions;
        //}

        public ConcCreep()
        {
            initialise();
            Calculate();
        }

        public ConcCreep(string grade, double rh, double t, double t_loadApplied, double l, double w)
        {
            //initialise();
            concGrade.TryParse(grade);
            relativeHumidity.Value = rh;
            time.Value = t;
            time0.Value = t_loadApplied;
            L.Quantity = new Length(l, UnitsNet.Units.LengthUnit.Millimeter);
            W.Quantity = new Length(w, UnitsNet.Units.LengthUnit.Millimeter);
            Calculate();
        }

        private void initialise()
        {
            //concGrade = inputValues.CreateCalcSelectionList("Concrete grade", "35", new List<string> { "30", "35", "40", "45", "50", "60", "70", "80", "90" });
            //notionalcreepcoeff = outputValues.CreateDoubleCalcValue("Notional creep coefficient", @"\varphi(t,t_0)", "", 0);
            //charCompStr = outputValues.CreateDoubleCalcValue("Concrete characteristic compressive strength", "f_{ck}", @"N/mm^2", 43);
            //meanCompStr = outputValues.CreateDoubleCalcValue("Concrete mean compressive strength", "f_{cm}", @"N/mm^2", 43);
            //relativeHumidity = inputValues.CreateDoubleCalcValue("Relative humidity", "RH", "", 70);
            //time0 = inputValues.CreateDoubleCalcValue("Time load applied", "t_0", "days", 28);
            //time = inputValues.CreateDoubleCalcValue("Time", "t", "days", 10000000);
            //L = inputValues.CreateDoubleCalcValue("Length", "L", "mm", 500);
            //W = inputValues.CreateDoubleCalcValue("Width", "W", "mm", 500);
            //_Ac = outputValues.CreateDoubleCalcValue("Cross section area", "A_c", "mm^2", 250000);
            //u = outputValues.CreateDoubleCalcValue("Section perimeter", "u", "mm", 2000);
            //creepTimeCoeff = outputValues.CreateDoubleCalcValue("Coefficient for creep with time", @"\beta(t,t_0)", "", 0);
            //creepCoeff = outputValues.CreateDoubleCalcValue("Creep coefficient", @"\varphi_0", "", 0);
        }

        public void Calculate()
        {
            expressions = new List<IFormula>();
            var concProps = ConcProperties.ByGrade(concGrade.ValueAsString());

            charCompStr.Quantity = new Pressure(concProps.fck, UnitsNet.Units.PressureUnit.NewtonPerSquareMillimeter);
            meanCompStr.Quantity = new Pressure(concProps.fcm, UnitsNet.Units.PressureUnit.NewtonPerSquareMillimeter);

            _Ac = W * L;
            CalcLength tempW = W + W; CalcLength tempL = L + L;
            u = tempW + tempL;

            double factorRH = 0;
            double betafcm = 0;
            double betat0 = 0;
            double betaH = 0;
            double h0 = 2 * _Ac.Value / u.Value;
            double alpha1 = Math.Pow((35 / meanCompStr.Value), 0.7);
            double alpha2 = Math.Pow((35 / meanCompStr.Value), 0.2);
            double alpha3 = Math.Pow((35 / meanCompStr.Value), 0.5);
            //expressions.Add(
            //    Formula.FormulaWithNarrative("Calculate alpha values")
            //    .AddExpression(@"\alpha_1=\left[ \frac{35}{f_{cm}} \right]^{0.7}" + Math.Round(alpha1, 2))
            //    .AddExpression(@"\alpha_2=\left[ \frac{35}{f_{cm}} \right]^{0.2}" + Math.Round(alpha2, 2))
            //    .AddExpression(@"\alpha_3=\left[ \frac{35}{f_{cm}} \right]^{0.5}" + Math.Round(alpha3, 2))
            //    .AddRef("B.8c")
            //    );

            if (meanCompStr.Value <= 35)
            {
                factorRH = 1 + (1 - relativeHumidity.Value / 100) / (0.1 * Math.Pow(h0, (double)(1d / 3d)));
                //expressions.Add(
                //    Formula.FormulaWithNarrative("Calculate factor to allow for effect of relative humidity")
                //    .AddRef("B.3a")
                //    .AddExpression(@"f_{cm}\leq35 \Rightarrow")
                //    .AddExpression(@"\phi_{RH}=1+\frac{1-RH/100}{0.1\sqrt[3]{h_0}}")
                //    );
            }
            else
            {
                factorRH = (1 + ((1 - relativeHumidity.Value / 100) / (0.1 * Math.Pow(h0, (double)(1d / 3d)))) * alpha1) * alpha2;
                //expressions.Add(
                //    Formula.FormulaWithNarrative("Calculate factor to allow for effect of relative humidity")
                //    .AddRef("B.3b")
                //    .AddExpression(@"f_{cm}>35 \Rightarrow")
                //    .AddExpression(@"\phi_{RH}=\left[ 1+\frac{1-RH/100}{0.1\sqrt[3]{h_0}}\alpha_1 \right]\alpha_2")
                //    );
            }

            betafcm = 16.8 / Math.Sqrt(meanCompStr.Value);
            //expressions.Add(
            //    Formula.FormulaWithNarrative("")
            //    .AddExpression(@"\beta(f_{cm})=\frac{16.8}{\sqrt{f_{cm}}}=" + Math.Round(betafcm, 2))
            //    .AddRef("B.4")
            //    );

            betat0 = 1 / (0.1 + Math.Pow(time0.Value, 0.20));
            //expressions.Add(
            //    Formula.FormulaWithNarrative("Factor to allow for effect of concrete" +
            //    "strength on the notional creep coefficient")
            //    .AddExpression(@"\beta(f_{cm})")
            //    );

            if (meanCompStr.Value <= 35)
            {
                betaH = Math.Min(1.5 * (1 + Math.Pow((0.012 * relativeHumidity.Value), 18)) * h0 + 250, 1500);
                //expressions.Add(
                //    Formula.FormulaWithNarrative("Calculate coefficient depending on relative humidity and notional member size.")
                //    .AddExpression(meanCompStr.Symbol + @"\leq 35 \Rightarrow")
                //    .AddExpression(@"\beta_H = 1.5\left[ 1 + (0.012 RH)^{18} \right]h_0 + 250 \leq 1500")
                //    .AddRef("B.8a")
                //    );
            }
            else
            {
                betaH = Math.Min(1.5 * (1 + Math.Pow((0.012 * relativeHumidity.Value), 18)) * h0 + 250 * alpha3, 1500 * alpha3);
                //expressions.Add(
                //    Formula.FormulaWithNarrative("Calculate coefficient depending on relative humidity and notional member size.")
                //    .AddExpression(meanCompStr.Symbol + @"> 35 \Rightarrow")
                //    .AddExpression(@"\beta_H = 1.5\left[ 1 + (0.012 RH)^{18} \right]h_0 + 250\alpha_3 \leq 1500\alpha_3")
                //    .AddRef("B.8b")
                //    );
            }

            notionalcreepcoeff.Value = factorRH * betafcm * betat0;

            creepTimeCoeff.Value = Math.Pow((time.Value - time0.Value) / (betaH + time.Value - time0.Value), 0.3);

            creepCoeff.Value = notionalcreepcoeff.Value * creepTimeCoeff.Value;
        }

        public IList<IFormula> GetFormulae()
        {
            return expressions;
        }


        //public override List<MW3DModel> Get3DModels()
        //{
        //    MWMesh mesh = new MWMesh();
        //    mesh.addNode(0, 0, 0, new MWPoint2D(0.5, 0.5));
        //    mesh.addNode(50, 0, 0, new MWPoint2D(0.5, 0.5));
        //    mesh.addNode(50, 50, 0, new MWPoint2D(0.5, 0.5));
        //    mesh.addNode(25, 25, 50, new MWPoint2D(0.5, 0.5));
        //    mesh.setIndices(new List<int[]> { new int[] { 2, 1, 0 }, new int[] { 1, 2, 3 }, new int[] { 3, 0, 1 }, new int[] { 3, 2, 0 } });
        //    mesh.Opacity = 0.2;
        //    mesh.Brush = new MWBrush(0, 0, 255);
        //    return new List<MW3DModel>() { new MW3DModel(mesh) };
        //}
    }
}

