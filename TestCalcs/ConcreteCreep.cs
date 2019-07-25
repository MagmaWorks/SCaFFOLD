using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcCore;

namespace TestCalcs
{
    public class ConcreteCreep : CalcCore.CalcBase
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

        public ConcreteCreep()
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
            double fcm = 0;
            double fck = 0;
            double Ecm = 0;
            switch (concGrade.ValueAsString)
            {
                case "30":
                    fck = 30;
                    fcm = 38;
                    Ecm = 33;
                    break;
                case "35":
                    fck = 35;
                    fcm = 43;
                    Ecm = 34;
                    break;
                case "40":
                    fck = 40;
                    fcm = 48;
                    Ecm = 35;
                    break;
                case "45":
                    fck = 45;
                    fcm = 53;
                    Ecm = 36;
                    break;
                case "50":
                    fck = 50;
                    fcm = 58;
                    Ecm = 37;
                    break;
                case "55":
                    fck = 55;
                    fcm = 63;
                    Ecm = 38;
                    break;
                case "60":
                    fck = 60;
                    fcm = 68;
                    Ecm = 39;
                    break;
                case "70":
                    fck = 70;
                    fcm = 78;
                    Ecm = 41;
                    break;
                case "80":
                    fck = 80;
                    fcm = 88;
                    Ecm = 42;
                    break;
                case "90":
                    fck = 90;
                    fcm = 98;
                    Ecm = 44;
                    break;
                default:
                    break;
            }
            charCompStr.Value = fck;
            meanCompStr.Value = fcm;

            Ac.Value = W.Value * L.Value;
            u.Value = 2d * W.Value + 2d * L.Value;

            double factorRH = 0;
            double betafcm = 0;
            double betat0 = 0;
            double betaH = 0;
            double h0 = 2 * Ac.Value / u.Value;
            double alpha1 = Math.Pow((35/meanCompStr.Value),0.7);
            double alpha2 = Math.Pow((35 / meanCompStr.Value), 0.2);
            double alpha3 = Math.Pow((35 / meanCompStr.Value), 0.5);

            if (meanCompStr.Value <= 35)
            {
                factorRH = 1 + (1 - relativeHumidity.Value / 100) / (0.1 * Math.Pow(h0, (double) (1d / 3d)));
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

            creepTimeCoeff.Value = Math.Pow((time.Value - time0.Value)/(betaH + time.Value - time0.Value) ,0.3);

            creepCoeff.Value = notionalcreepcoeff.Value * creepTimeCoeff.Value;


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

            double betaRH = 1.55 * (1 - Math.Pow((relativeHumidity.Value/RH0), 3));

            dryingStrain.Value = 0.85 * ((220 + 110 * alphads1) * Math.Exp(-alphads2*(meanCompStr.Value/fcm0))) * Math.Pow(10,-6) * betaRH;

            //3.12
            double autogenousStrainFinal = 2.5 * (charCompStr.Value - 10) * Math.Pow(10,-6);

            //3.13
            double betaast = 1 - Math.Exp(-0.2 * Math.Pow(time.Value, 0.5));

            //3.11
            double autogenousStrainTime = betaast * autogenousStrainFinal;

            //3.10
            double timeDevCoeff = (time.Value - timeShrinkStart.Value) / ((time.Value - timeShrinkStart.Value)+ 0.04 * Math.Sqrt(Math.Pow(h0,3)));

            double kh = getkh(h0);

            //3.9
            double dryingStrainWithTime = timeDevCoeff * kh * dryingStrain.Value;

            //3.8
            totalShrinkageStrain.Value = dryingStrainWithTime + autogenousStrainTime;

            double effectiveE = Ecm / (1 + creepCoeff.Value);
            double stress = F.Value * 1000 / Ac.Value;
            double creepStrain = stress / (effectiveE * 1000);
            creepMovement.Value = H.Value * creepStrain;

            shrinkageMovement.Value = H.Value * totalShrinkageStrain.Value;

            totalMovement.Value = creepMovement.Value + shrinkageMovement.Value;
        }

        double getkh (double h0)
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
