using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcCore;

namespace TestCalcs
{
    [CalcName("Concrete shrinkage")]
    [CalcAlternativeName("TestCalcs.ConcShrinkage")]
    public class ConcShrinkage : CalcCore.CalcBase
    {
        //\varphi
        CalcSelectionList concGrade;
        CalcDouble meanCompStr;
        CalcDouble charCompStr;
        CalcDouble relativeHumidity;
        CalcDouble time;
        CalcDouble timeShrinkStart;
        CalcDouble Ac;
        CalcDouble u;
        CalcDouble L;
        CalcDouble W;
        CalcSelectionList cementType;
        CalcDouble dryingStrain;
        CalcDouble totalShrinkageStrain;

        public double DryingStrain { get => dryingStrain.Value; }
        public double TotalShrinkageStrain { get => totalShrinkageStrain.Value; }

        public override List<Formula> GenerateFormulae()
        {
            return new List<Formula>();
        }

        public ConcShrinkage()
        {
            initialise();
            UpdateCalc();
        }

        public ConcShrinkage(string grade, double rh, double t, double t_shrinkStart, double l, double w, string cementType)
        {
            initialise();
            concGrade.ValueAsString = grade;
            relativeHumidity.Value = rh;
            time.Value = t;
            timeShrinkStart.Value = t_shrinkStart;
            L.Value = l;
            W.Value = w;
            this.cementType.ValueAsString = cementType;
            UpdateCalc();
        }

        private void initialise()
        {
            concGrade = inputValues.CreateCalcSelectionList("Concrete grade", "35", new List<string> { "30", "35", "40", "45", "50", "60", "70", "80", "90" });
            charCompStr = outputValues.CreateDoubleCalcValue("Concrete characteristic compressive strength", "f_{ck}", @"N/mm^2", 43);
            meanCompStr = outputValues.CreateDoubleCalcValue("Concrete mean compressive strength", "f_{cm}", @"N/mm^2", 43);
            relativeHumidity = inputValues.CreateDoubleCalcValue("Relative humidity", "RH", "", 70);
            time = inputValues.CreateDoubleCalcValue("Time", "t", "days", 10000000);
            timeShrinkStart = inputValues.CreateDoubleCalcValue("Shrinkage start", "t_s", "days", 7);
            L = inputValues.CreateDoubleCalcValue("Length", "L", "mm", 500);
            W = inputValues.CreateDoubleCalcValue("Width", "W", "mm", 500);
            Ac = outputValues.CreateDoubleCalcValue("Cross section area", "A_c", "mm^2", 250000);
            u = outputValues.CreateDoubleCalcValue("Section perimeter", "u", "mm", 2000);
            cementType = inputValues.CreateCalcSelectionList("Cement type", "S", new List<string> { "S", "N", "R" });
            dryingStrain = outputValues.CreateDoubleCalcValue("Unrestrained drying strain", @"\epsilon_{cd,0}", "", 0);
            totalShrinkageStrain = outputValues.CreateDoubleCalcValue("Total shrinkage strain", @"\epsilon_{cs}", "", 0);
        }

        public override void UpdateCalc()
        {
            var concProps = ConcProperties.ByGrade(concGrade.ValueAsString);
            
            charCompStr.Value = concProps.fck;
            meanCompStr.Value = concProps.fcm;

            Ac.Value = W.Value * L.Value;
            u.Value = 2d * W.Value + 2d * L.Value;

            double h0 = 2 * Ac.Value / u.Value;
           
            double alphads1;
            double alphads2;
            if (cementType.ValueAsString == "S")
            {
                alphads1 = 3;
                alphads2 = 0.13;
            }
            else if (cementType.ValueAsString == "N")
            {
                alphads1 = 4;
                alphads2 = 0.12;
            }
            else
            {
                alphads1 = 6;
                alphads2 = 0.11;
            }
            double RH0 = 100;
            double fcm0 = 10;

            double betaRH = 1.55 * (1 - Math.Pow((relativeHumidity.Value / RH0), 3));

            dryingStrain.Value = 0.85 * ((220 + 110 * alphads1) * Math.Exp(-alphads2 * (meanCompStr.Value / fcm0))) * Math.Pow(10, -6) * betaRH;

            //3.12
            double autogenousStrainFinal = 2.5 * (charCompStr.Value - 10) * Math.Pow(10, -6);

            //3.13
            double betaast = 1 - Math.Exp(-0.2 * Math.Pow(time.Value, 0.5));

            //3.11
            double autogenousStrainTime = betaast * autogenousStrainFinal;

            //3.10
            double timeDevCoeff = (time.Value - timeShrinkStart.Value) / ((time.Value - timeShrinkStart.Value) + 0.04 * Math.Sqrt(Math.Pow(h0, 3)));

            double kh = getkh(h0);

            //3.9
            double dryingStrainWithTime = timeDevCoeff * kh * dryingStrain.Value;

            //3.8
            totalShrinkageStrain.Value = dryingStrainWithTime + autogenousStrainTime;
        }

        double getkh(double h0)
        {
            if (h0 >= 500)
            {
                return 0.7;
            }
            else if (h0 > 300)
            {
                return 0.7 + ((500 - h0) / (500 - 300)) * 0.05;
            }
            else if (h0 > 200)
            {
                return 0.75 + ((300 - h0) / (300 - 200)) * 0.1;
            }
            else if (h0 > 100)
            {
                return 0.85 + ((200 - h0) / (200 - 100)) * 0.15;
            }
            else return double.NaN;
        }

    }
}
