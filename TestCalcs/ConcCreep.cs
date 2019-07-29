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

        public double NotionalCreepCoefficient { get => notionalcreepcoeff.Value; }
        public double CreepCoefficient { get => creepCoeff.Value; }
        public double Ac { get => _Ac.Value; }
        public double U { get => u.Value; }
        public double CreepTimeCoeff { get => creepTimeCoeff.Value; }

        public override List<Formula> GenerateFormulae()
        {
            return new List<Formula>();
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

            if (meanCompStr.Value <= 35)
            {
                factorRH = 1 + (1 - relativeHumidity.Value / 100) / (0.1 * Math.Pow(h0, (double)(1d / 3d)));
            }
            else
            {
                factorRH = (1 + ((1 - relativeHumidity.Value / 100) / (0.1 * Math.Pow(h0, (double)(1d / 3d)))) * alpha1) * alpha2;
            }

            betafcm = 16.8 / Math.Sqrt(meanCompStr.Value);

            betat0 = 1 / (0.1 + Math.Pow(time0.Value, 0.20));

            if (meanCompStr.Value <= 35)
            {
                betaH = Math.Min(1.5 * (1 + Math.Pow((0.0012 * factorRH), 18)) * h0 + 250, 1500);
            }
            else
            {
                betaH = Math.Min(1.5 * (1 + Math.Pow((0.0012 * factorRH), 18)) * h0 + 250 * alpha3, 1500 * alpha3);
            }

            notionalcreepcoeff.Value = factorRH * betafcm * betat0;

            creepTimeCoeff.Value = Math.Pow((time.Value - time0.Value) / (betaH + time.Value - time0.Value), 0.3);

            creepCoeff.Value = notionalcreepcoeff.Value * creepTimeCoeff.Value;
        }
    }
}
