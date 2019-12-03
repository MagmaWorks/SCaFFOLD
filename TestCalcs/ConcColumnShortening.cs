using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcCore;

namespace TestCalcs
{
    [CalcName("RC Column Shortening")]
    [CalcAlternativeName("TestCalcs.ConcColumnShortening")]
    public class ConcColumnShortening : CalcBase
    {
        List<Formula> expressions;
        CalcDouble notionalcreepcoeff;
        CalcDouble creepCoeff;
        CalcSelectionList concGrade;
        CalcDouble meanCompStr;
        CalcDouble charCompStr;
        CalcDouble relativeHumidity1;
        CalcDouble relativeHumiditySwitchTime;
        CalcDouble relativeHumidity2;
        //CalcDouble time0;
        CalcDouble time;
        CalcDouble timeShrinkStart;
        CalcDouble Ac;
        CalcDouble u;
        CalcDouble L;
        CalcDouble W;
        CalcDouble H;
        CalcDouble F;
        CalcDouble creepTimeCoeff;
        CalcSelectionList cementType;
        CalcDouble dryingStrain;
        CalcDouble totalShrinkageStrain;
        CalcDouble totalCreepMovement;
        CalcDouble shrinkageMovement;
        CalcDouble totalMovement;

        CalcListOfDoubleArrays _loads;
        CalcDouble _numberOfSSteps;

        public double TotalMovement
        {
            get
            {
                return totalMovement.Value;
            }
        }

        public double Time
        {
            get
            {
                return time.Value;
            }
            set
            {
                time.Value = value;
                UpdateCalc();
            }
        }

        public ConcColumnShortening()
        {
            initialise();
            UpdateCalc();
        }

        private void initialise()
        {
            _loads = inputValues.CreateCalcListOfDoubleArrays("Loads (kN) and times (d)", new List<double[]> { new double[] { 100, 28 }, new double[] { 500, 90 }, new double[] { 2000, 180 } });
            _numberOfSSteps = outputValues.CreateDoubleCalcValue("Steps", "", "", 0);
            concGrade = inputValues.CreateCalcSelectionList("Concrete grade", "35", new List<string> { "30", "35", "40", "45", "50", "60", "70", "80", "90" });
            notionalcreepcoeff = outputValues.CreateDoubleCalcValue("Notional creep coefficient", @"\varphi(t,t_0)", "", 0);
            charCompStr = outputValues.CreateDoubleCalcValue("Concrete characteristic compressive strength", "f_{ck}", @"N/mm^2", 43);
            meanCompStr = outputValues.CreateDoubleCalcValue("Concrete mean compressive strength", "f_{cm}", @"N/mm^2", 43);
            relativeHumidity1 = inputValues.CreateDoubleCalcValue("Relative humidity 1", "RH", "", 70);
            relativeHumidity2 = inputValues.CreateDoubleCalcValue("Relative humidity 2", "RH", "", 50);
            relativeHumiditySwitchTime = inputValues.CreateDoubleCalcValue("Relative humidity switch time", "t_{humiditychange}", "d", 80);
            //time0 = inputValues.CreateDoubleCalcValue("Time load applied", "t_0", "days", 28);
            time = inputValues.CreateDoubleCalcValue("Time", "t", "days", 10000000);
            timeShrinkStart = inputValues.CreateDoubleCalcValue("Shrinkage start", "t_s", "days", 7);
            L = inputValues.CreateDoubleCalcValue("Length", "L", "mm", 500);
            W = inputValues.CreateDoubleCalcValue("Width", "W", "mm", 500);
            H = inputValues.CreateDoubleCalcValue("Height", "H", "mm", 3500);
            Ac = outputValues.CreateDoubleCalcValue("Cross section area", "A_c", "mm^2", 250000);
            u = outputValues.CreateDoubleCalcValue("Section perimeter", "u", "mm", 2000);
            creepTimeCoeff = outputValues.CreateDoubleCalcValue("Coefficient for creep with time", @"\beta(t,t_0)", "", 0);
            creepCoeff = outputValues.CreateDoubleCalcValue("Creep coefficient", @"\varphi_0", "", 0);
            cementType = inputValues.CreateCalcSelectionList("Cement type", "S", new List<string> { "S", "N", "R" });
            dryingStrain = outputValues.CreateDoubleCalcValue("Unrestrained drying strain", @"\epsilon_{cd,0}", "", 0);
            totalShrinkageStrain = outputValues.CreateDoubleCalcValue("Total shrinkage strain", @"\epsilon_{cs}", "", 0);
            totalCreepMovement = outputValues.CreateDoubleCalcValue("Total creep movement", "", "mm", 0);
            shrinkageMovement = outputValues.CreateDoubleCalcValue("Shrinkage movement", "", "mm", 0);
            totalMovement = outputValues.CreateDoubleCalcValue("Total movement", "", "mm", 0); UpdateCalc();
        }

        public ConcColumnShortening(List<double[]> loads, string concGrade, double RH, double t, double ts, double L, double W ,double H)
        {
            initialise();
            _loads.Value = loads;
            this.concGrade.ValueAsString = concGrade;
            this.relativeHumidity1.Value = RH;
            this.relativeHumidity2.Value = RH;
            //this.time0.Value = t0;
            this.time.Value = t;
            this.timeShrinkStart.Value = ts;
            this.L.Value = L;
            this.W.Value = W;
            this.H.Value = H;
            UpdateCalc();
        }

        public override List<Formula> GenerateFormulae()
        {
            return expressions;
        }

        public override void UpdateCalc()
        {
            formulae = null;
            expressions = new List<Formula>();
            var concProps = ConcProperties.ByGrade(concGrade.ValueAsString);
            // CALC SHRINKAGE FOR FIRST AND SECOND RH VALUES
            var shrinkageExpression = Formula.FormulaWithNarrative("Calculate shrinkage");
            if (time.Value <= relativeHumiditySwitchTime.Value)
            {
                var shrinkageCalc1 = new ConcShrinkage(concGrade.ValueAsString, relativeHumidity1.Value, time.Value, timeShrinkStart.Value, L.Value, W.Value, cementType.ValueAsString);
                totalShrinkageStrain.Value = shrinkageCalc1.TotalShrinkageStrain;
                shrinkageExpression.AddFirstExpression(totalShrinkageStrain.Symbol + "=" + Math.Round(totalShrinkageStrain.Value,2));
            }
            else
            {
                var shrinkageCalc1 = new ConcShrinkage(concGrade.ValueAsString, relativeHumidity1.Value, relativeHumiditySwitchTime.Value, timeShrinkStart.Value, L.Value, W.Value, cementType.ValueAsString);
                var shrinkageCalc2 = new ConcShrinkage(concGrade.ValueAsString, relativeHumidity2.Value, relativeHumiditySwitchTime.Value, timeShrinkStart.Value, L.Value, W.Value, cementType.ValueAsString);
                var shrinkageCalc3 = new ConcShrinkage(concGrade.ValueAsString, relativeHumidity2.Value, time.Value, timeShrinkStart.Value, L.Value, W.Value, cementType.ValueAsString);
                totalShrinkageStrain.Value = shrinkageCalc1.TotalShrinkageStrain + (shrinkageCalc3.TotalShrinkageStrain - shrinkageCalc2.TotalShrinkageStrain);
                shrinkageExpression.Narrative += Environment.NewLine + "Calculate shrinkage up to change in humidity using RH1 (1) and RH2 (2), and " +
                    "calculate total shrinkage using RH2 (3).";
                shrinkageExpression.AddFirstExpression(@"\epsilon_{cs,1} = " + string.Format("{0:#.##E+0}", shrinkageCalc1.TotalShrinkageStrain));
                shrinkageExpression.AddExpression(@"\epsilon_{cs,2} = " + string.Format("{0:#.##E+0}", shrinkageCalc2.TotalShrinkageStrain));
                shrinkageExpression.AddExpression(@"\epsilon_{cs,3} = " + string.Format("{0:#.##E+0}", shrinkageCalc3.TotalShrinkageStrain));
                shrinkageExpression.AddExpression(totalShrinkageStrain.Symbol + @"= \epsilon_{cs,1} + (\epsilon_{cs,3} - \epsilon_{cs,2}) = " + string.Format("{0:#.##E+0}", totalShrinkageStrain.Value));
            }

            Ac.Value = L.Value * W.Value;
            u.Value = 2 * L.Value + 2 * W.Value;

            totalCreepMovement.Value = 0;
            var creepExpression = Formula.FormulaWithNarrative("Calculate creep from each load" + Environment.NewLine);
            double Et = 1.08 * concProps.Ecm;

            foreach (var loadEvent in _loads.Value)
            {
                double load = loadEvent[0];
                double loadTime = loadEvent[1];

                if (loadTime < time.Value)
                {
                    if (loadTime > relativeHumiditySwitchTime.Value)
                    {
                        // load only affected by RH2
                        var creepCalc = new ConcCreep(concGrade.ValueAsString, relativeHumidity2.Value, time.Value, loadTime, L.Value, W.Value);
                        double effectiveE = Et / (1 + creepCalc.CreepCoefficient);
                        double stress = load * 1000 / Ac.Value;
                        double creepStrain = stress / (effectiveE * 1000);
                        double creepMovement = H.Value * creepStrain;
                        this.totalCreepMovement.Value += creepMovement;
                        creepExpression.Narrative += "Elastic movement plus creep due to " + load + "kN applied at " + loadTime + " days: " + creepMovement + "mm" + Environment.NewLine;
                    }
                    else if (time.Value < relativeHumiditySwitchTime.Value)
                    {
                        //load only affected by RH1
                        var creepCalc = new ConcCreep(concGrade.ValueAsString, relativeHumidity1.Value, time.Value, loadTime, L.Value, W.Value);
                        double effectiveE = Et / (1 + creepCalc.CreepCoefficient);
                        double stress = load * 1000 / Ac.Value;
                        double creepStrain = stress / (effectiveE * 1000);
                        double creepMovement = H.Value * creepStrain;
                        this.totalCreepMovement.Value += creepMovement;
                        creepExpression.Narrative += "Elastic movement plus creep due to " + load + "kN applied at " + loadTime + " days: " + creepMovement + "mm" + Environment.NewLine;

                    }
                    else 
                    {
                        var creepCalc1 = new ConcCreep(concGrade.ValueAsString, relativeHumidity1.Value, relativeHumiditySwitchTime.Value, loadTime, L.Value, W.Value);
                        var creepCalc2_1 = new ConcCreep(concGrade.ValueAsString, relativeHumidity2.Value, relativeHumiditySwitchTime.Value, loadTime, L.Value, W.Value);
                        var creepCalc2_2 = new ConcCreep(concGrade.ValueAsString, relativeHumidity2.Value, time.Value, loadTime, L.Value, W.Value);
                        double effCreepCoeff = creepCalc1.CreepCoefficient + (creepCalc2_2.CreepCoefficient - creepCalc2_1.CreepCoefficient);
                        double effectiveE = Et / (1 + effCreepCoeff);
                        double stress = load * 1000 / Ac.Value;
                        double creepStrain = stress / (effectiveE * 1000);
                        double creepMovement = H.Value * creepStrain;
                        this.totalCreepMovement.Value += creepMovement;
                        creepExpression.Narrative += "Creep coefficients :" + creepCalc1.CreepCoefficient + ", " + creepCalc2_1.CreepCoefficient + ", " + creepCalc2_2.CreepCoefficient + ". " + Environment.NewLine;
                        creepExpression.Narrative += "Elastic movement plus creep due to " + load + "kN applied at " + loadTime + " days: " + creepMovement + "mm" + Environment.NewLine;
                    }
                }
            }

            expressions.Add(shrinkageExpression);
            expressions.Add(creepExpression);

            charCompStr.Value = concProps.fck;
            meanCompStr.Value = concProps.fcm;
            shrinkageMovement.Value = H.Value * totalShrinkageStrain.Value;
            totalMovement.Value = this.totalCreepMovement.Value + shrinkageMovement.Value;
        }
    }
}
