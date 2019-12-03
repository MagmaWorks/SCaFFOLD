using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcCore;

namespace TestCalcs
{
    [CalcName("Concrete creep and shrinkage")]
    [CalcAlternativeName("TestCalcs.ConcCreepAndShrinkage")]
    public class ConcCreepAndShrinkage : CalcCore.CalcBase
    {
        //\varphi
        CalcDouble notionalcreepcoeff;
        CalcDouble creepCoeff;
        CalcSelectionList concGrade;
        CalcDouble meanCompStr;
        CalcDouble charCompStr;
        CalcDouble relativeHumidity;
        CalcDouble time0;
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
        CalcDouble creepMovement;
        CalcDouble shrinkageMovement;
        CalcDouble totalMovement;

        public override List<Formula> GenerateFormulae()
        {
            return new List<Formula>();
        }

        public ConcCreepAndShrinkage()
        {
            concGrade = inputValues.CreateCalcSelectionList("Concrete grade", "35", new List<string> { "30", "35", "40", "45", "50", "60", "70", "80", "90" });
            notionalcreepcoeff = outputValues.CreateDoubleCalcValue("Notional creep coefficient", @"\varphi(t,t_0)", "", 0);
            charCompStr = outputValues.CreateDoubleCalcValue("Concrete characteristic compressive strength", "f_{ck}", @"N/mm^2", 43);
            meanCompStr = outputValues.CreateDoubleCalcValue("Concrete mean compressive strength", "f_{cm}", @"N/mm^2", 43);
            relativeHumidity = inputValues.CreateDoubleCalcValue("Relative humidity", "RH", "", 70);
            time0 = inputValues.CreateDoubleCalcValue("Time load applied", "t_0", "days", 28);
            time = inputValues.CreateDoubleCalcValue("Time", "t", "days", 10000000);
            timeShrinkStart = inputValues.CreateDoubleCalcValue("Shrinkage start", "t_s", "days", 7);
            L = inputValues.CreateDoubleCalcValue("Length", "L", "mm", 500);
            W = inputValues.CreateDoubleCalcValue("Width", "W", "mm", 500);
            H = inputValues.CreateDoubleCalcValue("Height", "H", "mm", 3500);
            F = inputValues.CreateDoubleCalcValue("Force", "F", "kN", 1000);
            Ac = outputValues.CreateDoubleCalcValue("Cross section area", "A_c", "mm^2", 250000);
            u = outputValues.CreateDoubleCalcValue("Section perimeter", "u", "mm", 2000);
            creepTimeCoeff = outputValues.CreateDoubleCalcValue("Coefficient for creep with time", @"\beta(t,t_0)", "", 0);
            creepCoeff = outputValues.CreateDoubleCalcValue("Creep coefficient", @"\varphi_0", "", 0);
            cementType = inputValues.CreateCalcSelectionList("Cement type", "S", new List<string> { "S", "N", "R" });
            dryingStrain = outputValues.CreateDoubleCalcValue("Unrestrained drying strain", @"\epsilon_{cd,0}", "", 0);
            totalShrinkageStrain = outputValues.CreateDoubleCalcValue("Total shrinkage strain", @"\epsilon_{cs}", "", 0);
            creepMovement = outputValues.CreateDoubleCalcValue("Creep movement", "", "mm", 0);
            shrinkageMovement = outputValues.CreateDoubleCalcValue("Shrinkage movement", "", "mm", 0);
            totalMovement = outputValues.CreateDoubleCalcValue("Total movement", "", "mm", 0);
        }

        public override void UpdateCalc()
        {
            var concProps = ConcProperties.ByGrade(concGrade.ValueAsString);
            var creepCalc = new ConcCreep(concGrade.ValueAsString, relativeHumidity.Value, time.Value, time0.Value, L.Value, W.Value);
            var shrinkageCalc = new ConcShrinkage(concGrade.ValueAsString, relativeHumidity.Value, time.Value, timeShrinkStart.Value, L.Value, W.Value, cementType.ValueAsString);
            notionalcreepcoeff.Value = creepCalc.NotionalCreepCoefficient;
            charCompStr.Value = concProps.fck;
            meanCompStr.Value = concProps.fcm;
            Ac.Value = creepCalc.Ac;
            u.Value = creepCalc.U;
            creepTimeCoeff.Value = creepCalc.CreepTimeCoeff;
            creepCoeff.Value = creepCalc.CreepCoefficient;
            dryingStrain.Value = shrinkageCalc.DryingStrain;
            totalShrinkageStrain.Value = shrinkageCalc.TotalShrinkageStrain;

            double effectiveE = 1.08 * concProps.Ecm / (1 + creepCoeff.Value);
            double stress = F.Value * 1000 / Ac.Value;
            double creepStrain = stress / (effectiveE * 1000);
            creepMovement.Value = H.Value * creepStrain; 
            shrinkageMovement.Value = H.Value * totalShrinkageStrain.Value;

            totalMovement.Value = creepMovement.Value + shrinkageMovement.Value;
        }
    }
}
