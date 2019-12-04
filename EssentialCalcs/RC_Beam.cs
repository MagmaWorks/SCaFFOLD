using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcCore;

namespace EssentialCalcs
{
    public class RC_Beam : CalcCore.CalcBase
    {
        CalcDouble aggregateAdjustmentFactor;
        CalcSelectionList concGrade;
        CalcDouble charCompStr;
        CalcDouble meanCompStr;
        CalcDouble meanAxialTenStr;
        CalcDouble secModElasticicity;
        CalcDouble ultStrain;
        CalcDouble shortStrain;
        CalcDouble effCompZoneHtFac;
        CalcDouble effStrFac;
        CalcDouble k1;
        CalcDouble k2;
        CalcDouble k3;
        CalcDouble k4;
        CalcDouble partialFacConc;
        CalcDouble compStrCoeff;
        CalcDouble desCompStrConc;
        CalcDouble compStrCoeffw;
        CalcDouble desCompStrConcw;
        CalcDouble hAgg;
        CalcDouble monolithicSimpleSupportFactor;
        CalcDouble rebarCharYieldStr;
        CalcDouble rebarPartialFactor;
        CalcDouble rebarDesYieldStr;
        CalcDouble coverTop;
        CalcDouble coverBtm;
        CalcDouble coverSides;
        CalcDouble firePeriod;
        CalcDouble sidesExposed;
        CalcDouble minWidth;
        CalcDouble secWidth;
        CalcDouble secDepth;
        CalcDouble bendingMom;
        CalcDouble effDepth;
        CalcDouble redistRatio;
        CalcDouble redistRatio2;
        CalcDouble leverArm;
        CalcDouble naDepth;
        CalcDouble rebarAsReqd;
        CalcDouble rebarAsProv;
        CalcDouble rebarMinArea;
        CalcDouble rebarMaxArea;

        List<Formula> expressions;

        public RC_Beam()
        {
            InstanceName = "RC Beam ref A3";
            _typeName = "Rectangular RC Beam Calculation";

            initialise();
        }

        void initialise()
        {
            secWidth = inputValues.CreateDoubleCalcValue("Section width", "b", "mm", 300);
            secDepth = inputValues.CreateDoubleCalcValue("Section depth", "h", "mm", 500);
            bendingMom = inputValues.CreateDoubleCalcValue("Bending moment", "M", "kNm", 100);
            aggregateAdjustmentFactor = inputValues.CreateDoubleCalcValue("Aggregate adjustment factor", "AAF", "", 1);
            charCompStr = outputValues.CreateDoubleCalcValue("Characteristic compressive cylinder strength", "f_{ck}", "N/mm^2", 35);
            concGrade = inputValues.CreateCalcSelectionList("Concrete grade", "C40/50", new List<string>() { @"C30/40", @"C35/42", @"C40/50" });
            meanCompStr = outputValues.CreateDoubleCalcValue("Mean compressive strength", "f_{cm}", "N/mm^2", 0);
            meanAxialTenStr = outputValues.CreateDoubleCalcValue("Mean axial tensile strength", "f_{ctm}", "N/mm^2", 0.3);
            secModElasticicity = outputValues.CreateDoubleCalcValue("Secant modulus of elasticity of concrete", "E_{cm}", "kN/mm^2", 22); ;
            ultStrain = inputValues.CreateDoubleCalcValue("Ultimate strain", @"\epsilon_{cu2}", "", 0.0035); 
            shortStrain = inputValues.CreateDoubleCalcValue("Shortening strain", @"\epsilon_{cu3}", "", 0.0035); 
            effCompZoneHtFac = inputValues.CreateDoubleCalcValue("Effective compression zone height factor", @"\lambda", "", 0.8); 
            effStrFac = inputValues.CreateDoubleCalcValue("Effective strength factor", @"\eta", "", 1.00); 
            k1 = inputValues.CreateDoubleCalcValue("Coefficient", "k_1", "", 0.4) ;
            k2 = outputValues.CreateDoubleCalcValue("Coefficient", "k_2", "", 0) ;
            k3 = inputValues.CreateDoubleCalcValue("Coefficient", "k_3", "", 0.4) ;
            k4 = outputValues.CreateDoubleCalcValue("Coefficient", "k_4", "", 0) ;
            partialFacConc = inputValues.CreateDoubleCalcValue("Partial factor for concrete", @"\gamma_c", "", 1.5) ;
            compStrCoeff = inputValues.CreateDoubleCalcValue("Compressive strength coefficient", "a_{cc}", "", 0.85); 
            desCompStrConc = outputValues.CreateDoubleCalcValue("Design compressive strength", "f_{cd}", "N/mm^2", 0) ;
            compStrCoeffw = inputValues.CreateDoubleCalcValue("Compressive strength coefficient", "a_{cw}", "", 1.0) ;
            desCompStrConcw = outputValues.CreateDoubleCalcValue("Strength of concrete cracked in shear", "f_{cwd}", "N/mm^2", 0) ;
            hAgg = inputValues.CreateDoubleCalcValue("Maximum aggregate size", "h_{agg}", "mm", 20); 
            monolithicSimpleSupportFactor = inputValues.CreateDoubleCalcValue("Monolithic simple support moment factor", @"\beta_1", "", 0.25); 
            rebarCharYieldStr = inputValues.CreateDoubleCalcValue("Characteristic yield strength for reinforcing steel", "f_yk", "N/mm^2", 500); 
            rebarPartialFactor = inputValues.CreateDoubleCalcValue("Partial factor for reinforcing steel", @"\gamma_s", "", 1.15); 
            rebarDesYieldStr = outputValues.CreateDoubleCalcValue("Design yield strength for reinforcing steel", "f_{yd}", "N/mm^2", 0) ;
            coverTop = inputValues.CreateDoubleCalcValue("Top cover", "c_{top}", "mm", 35); 
            coverBtm = inputValues.CreateDoubleCalcValue("Bottom cover", "c_{btm}", "mm", 35); 
            coverSides = inputValues.CreateDoubleCalcValue("Side cover", "c_{sides}", "mm", 35); 
            firePeriod = inputValues.CreateDoubleCalcValue("Fire period", "R", "mins", 60); 
            sidesExposed = inputValues.CreateDoubleCalcValue("Sides exposed", "", "No.", 3); 
            minWidth = inputValues.CreateDoubleCalcValue("Minimum width", "b_{min}", "mm", 120) ;
            effDepth = outputValues.CreateDoubleCalcValue("Effective depth", "d", "mm", 0); 
            redistRatio = outputValues.CreateDoubleCalcValue("Redistribution ratio", "K", "", 0);
            redistRatio2 = outputValues.CreateDoubleCalcValue("Redistribution ratio limit", "K'", "", 0);
            leverArm = outputValues.CreateDoubleCalcValue("Lever arm", "z", "mm", 0); 
            naDepth = outputValues.CreateDoubleCalcValue("Neutral axis depth", "x", "mm", 0); 
            rebarAsReqd = outputValues.CreateDoubleCalcValue("Tension reinforcement required", "A_{s,reqd}", "mm^2", 0); 
            rebarAsProv = outputValues.CreateDoubleCalcValue("Tension reinforcement provided", "A_{s,prov}", "mm^2", 0); 
            rebarMinArea = outputValues.CreateDoubleCalcValue("Minimum tension reinforcement", "A_{s,min}", "mm^2", 0); 
            rebarMaxArea = outputValues.CreateDoubleCalcValue("Maximum tension reinforcement", "A_{s,max}", "mm^2", 0); 

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
            expressions.Add(new Formula()
            {
                Narrative = "Beam calculations to BS EN 1992-1-1 2004. Currently in beta so check thoroughly!",
            });
            List<string> expression = new List<string>();
            charCompStr.Value = getConcreteStrength(concGrade.ValueAsString);
            meanCompStr.Value = charCompStr.Value + 8;
            expression.Add(string.Format("{0}={1}+8={2}{3}", meanCompStr.Symbol, charCompStr.Symbol, meanCompStr.Value, meanCompStr.Unit));
            meanAxialTenStr.Value = 0.3 * Math.Pow(charCompStr.Value, 2f / 3f);
            expression.Add(string.Format(@"{0}=0.3\times {1}^{{2/3}}={2}{3}",
                meanAxialTenStr.Symbol,
                charCompStr.Symbol,
                Math.Round(meanAxialTenStr.Value,2),
                meanAxialTenStr.Unit));
            secModElasticicity.Value = 22 * Math.Pow(meanCompStr.Value / 10f, 0.3) * aggregateAdjustmentFactor.Value;
            expression.Add(string.Format(@"{0}=22\times \left(\frac{{{1}}}{{10}}\right)^{{0.3}}={2}{3}",
                secModElasticicity.Symbol,
                meanCompStr.Symbol,
                Math.Round(secModElasticicity.Value,3),
                secModElasticicity.Unit));
            k2.Value = 0.6 + 0.0014 / ultStrain.Value;
            k4.Value = 0.6 + 0.0014 / ultStrain.Value;
            desCompStrConc.Value = compStrCoeff.Value * charCompStr.Value / partialFacConc.Value;
            desCompStrConcw.Value = compStrCoeffw.Value * charCompStr.Value / partialFacConc.Value;
            rebarDesYieldStr.Value = rebarCharYieldStr.Value / rebarPartialFactor.Value;

            expressions.Add(new Formula() { Expression = expression, Ref = "Property calcs", Narrative="" });
            expression = new List<string>();

            effDepth.Value = secDepth.Value - coverBtm.Value - 16;
            redistRatio.Value = bendingMom.Value * 1000000 / (secWidth.Value * Math.Pow(effDepth.Value,2) * charCompStr.Value);
            redistRatio2.Value = (2 * effStrFac.Value * compStrCoeff.Value/partialFacConc.Value) * (1 - effCompZoneHtFac.Value * (1 - k1.Value)/(2*k2.Value)) * (effCompZoneHtFac.Value * (1-k1.Value)/(2*k2.Value));
            leverArm.Value = Math.Min(0.5*effDepth.Value*(1+Math.Pow((1 - 2 * redistRatio.Value / (effStrFac.Value * compStrCoeff.Value / partialFacConc.Value)),0.5)), 0.95 * effDepth.Value);
            naDepth.Value = 2 * (effDepth.Value - leverArm.Value) / effCompZoneHtFac.Value;
            rebarAsReqd.Value = bendingMom.Value * 1000000 / (rebarDesYieldStr.Value * leverArm.Value);
            expression.Add(string.Format(@"{0}=\frac{{{1}}}{{({2}{3})}}={4}{5}", rebarAsReqd.Symbol, bendingMom.Symbol, rebarDesYieldStr.Symbol, leverArm.Symbol, Math.Round(rebarAsReqd.Value,0), rebarAsReqd.Unit));
            var bottomBars = calcBarSizeAndDia(rebarAsReqd.Value);
            rebarAsProv.Value = bottomBars.Item1 * Math.PI * Math.Pow(bottomBars.Item2 / 2, 2);
            if (double.IsNaN(rebarAsProv.Value))
                rebarAsProv.Status = CalcStatus.FAIL;
            else
                rebarAsProv.Status = CalcStatus.NONE;
            expression.Add(string.Format("{0}={1}{2}", rebarAsProv.Symbol, Math.Round(rebarAsProv.Value,0), rebarAsProv.Unit));
            expressions.Add(new Formula()
            {
                Expression = expression,
                Ref = "cl. 6.1",
                Narrative = "Calculates the tension reinforcement required to resist bending.",
                Conclusion = string.Format("{0}No. H{1} bars provided", bottomBars.Item1, bottomBars.Item2),
                Status = rebarAsProv.Status
            });

            // Check As_prov exceeds As_min
            expression = new List<string>();
            rebarMinArea.Value = Math.Max(0.26 * meanAxialTenStr.Value / rebarCharYieldStr.Value, 0.0013) * secWidth.Value * effDepth.Value;
            expression.Add(string.Format(@"{0}=max(0.26\frac{{{1}}}{{{2}}}, 0.0013)\times {3}\times {4}={5}{6}",
                rebarMinArea.Symbol,
                meanAxialTenStr.Symbol,
                rebarCharYieldStr.Symbol,
                secWidth.Symbol,
                effDepth.Symbol,
                Math.Round(rebarMinArea.Value,0),
                rebarMinArea.Unit));
            if (rebarAsProv.Value >= rebarMinArea.Value)
            {
                expression.Add(string.Format(@"{0}>={1}", rebarAsProv.Symbol, rebarMinArea.Symbol));
                expressions.Add(new Formula()
                {
                    Expression = expression,
                    Ref = "equ. 9.1N",
                    Narrative = "Check that provided reinforcement exceeds minimum",
                    Conclusion = "OK",
                    Status = CalcStatus.PASS
                });
            }
            else
            {
                expression.Add(string.Format(@"{0}<{1}", rebarAsProv.Symbol, rebarMinArea.Symbol));
                expressions.Add(new Formula()
                {
                    Expression = expression,
                    Ref = "equ. 9.1N",
                    Narrative = "Check that provided reinforcement exceeds minimum",
                    Conclusion = "Too little reinforcing steel",
                    Status = CalcStatus.FAIL
                });
            }

            // Check As_prov is less than As_max
            expression = new List<string>();
            rebarMaxArea.Value = 0.04 * secWidth.Value * secDepth.Value;
            expression.Add(string.Format(@"{0}=0.04\times {1}\times {2}={3}{4}",
                rebarMaxArea.Symbol,
                secWidth.Symbol,
                secDepth.Symbol,
                Math.Round(rebarMaxArea.Value,0),
                rebarMaxArea.Unit));
            if (rebarAsProv.Value <= rebarMaxArea.Value)
            {
                expression.Add(string.Format(@"{0}<={1}", rebarAsProv.Symbol, rebarMaxArea.Symbol));
                expressions.Add(new Formula()
                {
                    Expression = expression,
                    Ref = "cl. 9.2.1(3)",
                    Narrative = "Check that provided reinforcement is less than maximum",
                    Conclusion = "OK",
                    Status = CalcStatus.PASS
                });
            }
            else
            {
                expression.Add(string.Format(@"{0}>{1}", rebarAsProv.Symbol, rebarMaxArea.Symbol));
                expressions.Add(new Formula()
                {
                    Expression = expression,
                    Ref = "cl. 9.2.1.1(3)",
                    Narrative = "Check that provided reinforcement is less than maximum",
                    Conclusion = "Too much reinforcing steel",
                    Status = CalcStatus.FAIL
                });
            }
        }

        Tuple<double,double> calcBarSizeAndDia(double area)
        {
            int maxBars = 5;
            List<int> barSizes = new List<int> { 10, 12, 16, 20, 25, 32, 40 };
            foreach (var item in barSizes)
            {
                for (int i = 2; i < maxBars; i++)
                {
                    if (i * Math.PI * Math.Pow(item/2,2) > area)
                    {
                        return new Tuple<double, double>(i, item);
                    }
                }
            }
            return new Tuple<double, double>(double.NaN, double.NaN);
        }

        private double getConcreteStrength(string grade)
        {
            var concGradeToStrength = new Dictionary<string, double> { { @"C30/40", 30 }, { @"C35/42", 35 }, { @"C40/50", 40 } };
            return concGradeToStrength[grade];
        }
    }
}
