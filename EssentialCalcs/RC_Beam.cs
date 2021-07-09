using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcCore;

namespace EssentialCalcs
{
    [CalcName("RC Beam")]
    [CalcAlternativeName("TestCalcs.RC_Beam")]
    public class RC_Beam : CalcCore.CalcBase
    {
        // INPUTS
        CalcDouble aggregateAdjustmentFactor;
        CalcSelectionList concGrade;
        CalcDouble ultStrain;
        CalcDouble shortStrain;
        CalcDouble effCompZoneHtFac;
        CalcDouble effStrFac;
        CalcDouble k1;
        CalcDouble k3;
        CalcDouble delta;
        CalcDouble partialFacConc;
        CalcDouble compStrCoeff;
        CalcDouble compStrCoeffw;
        CalcDouble hAgg;
        CalcDouble monolithicSimpleSupportFactor;
        CalcDouble rebarCharYieldStr;
        CalcDouble rebarPartialFactor;
        CalcDouble coverTop;
        CalcDouble coverBtm;
        CalcDouble coverSides;
        CalcDouble firePeriod;
        CalcDouble sidesExposed;
        CalcDouble minWidth;
        CalcDouble secWidth;
        CalcDouble secDepth;
        CalcDouble bendingMom;

        // OUTPUTS
        CalcDouble charCompStr;
        CalcDouble meanCompStr;
        CalcDouble meanAxialTenStr;
        CalcDouble secModElasticicity;
        CalcDouble k2;
        CalcDouble k4;
        CalcDouble desCompStrConc;
        CalcDouble desCompStrConcw;
        CalcDouble rebarDesYieldStr;
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
            // INPUTS
            secWidth = inputValues.CreateDoubleCalcValue("Section width", "b", "mm", 300);
            secDepth = inputValues.CreateDoubleCalcValue("Section depth", "h", "mm", 500);
            bendingMom = inputValues.CreateDoubleCalcValue("Bending moment", "M", "kNm", 100);
            aggregateAdjustmentFactor = inputValues.CreateDoubleCalcValue("Aggregate adjustment factor", "AAF", "", 1);
            concGrade = inputValues.CreateCalcSelectionList("Concrete grade", "C40/50", new List<string>() { @"C30/40", @"C35/42", @"C40/50" });
            ultStrain = inputValues.CreateDoubleCalcValue("Ultimate strain", @"\epsilon_{cu2}", "", 0.0035); 
            shortStrain = inputValues.CreateDoubleCalcValue("Shortening strain", @"\epsilon_{cu3}", "", 0.0035); 
            effCompZoneHtFac = inputValues.CreateDoubleCalcValue("Effective compression zone height factor", @"\lambda", "", 0.8); 
            effStrFac = inputValues.CreateDoubleCalcValue("Effective strength factor", @"\eta", "", 1.00); 
            k1 = inputValues.CreateDoubleCalcValue("Coefficient k1", "k_1", "", 0.4) ;
            k3 = inputValues.CreateDoubleCalcValue("Coefficient k3", "k_3", "", 0.4) ;
            delta = inputValues.CreateDoubleCalcValue("Redistribution factor", @"\delta","", 0.85);
            partialFacConc = inputValues.CreateDoubleCalcValue("Partial factor for concrete", @"\gamma_c", "", 1.5) ;
            compStrCoeff = inputValues.CreateDoubleCalcValue("Compressive strength coefficient", @"\alpha_{cc}", "", 0.85); 
            compStrCoeffw = inputValues.CreateDoubleCalcValue("Compressive strength coefficient", @"\alpha_{cw}", "", 1.0) ;
            hAgg = inputValues.CreateDoubleCalcValue("Maximum aggregate size", "h_{agg}", "mm", 20); 
            monolithicSimpleSupportFactor = inputValues.CreateDoubleCalcValue("Monolithic simple support moment factor", @"\beta_1", "", 0.25); 
            rebarCharYieldStr = inputValues.CreateDoubleCalcValue("Characteristic yield strength for reinforcing steel", "f_yk", "N/mm^2", 500); 
            rebarPartialFactor = inputValues.CreateDoubleCalcValue("Partial factor for reinforcing steel", @"\gamma_s", "", 1.15); 
            coverTop = inputValues.CreateDoubleCalcValue("Top cover", "c_{top}", "mm", 35); 
            coverBtm = inputValues.CreateDoubleCalcValue("Bottom cover", "c_{btm}", "mm", 35); 
            coverSides = inputValues.CreateDoubleCalcValue("Side cover", "c_{sides}", "mm", 35); 
            firePeriod = inputValues.CreateDoubleCalcValue("Fire period", "R", "mins", 60); 
            sidesExposed = inputValues.CreateDoubleCalcValue("Sides exposed", "", "No.", 3); 
            minWidth = inputValues.CreateDoubleCalcValue("Minimum width", "b_{min}", "mm", 120) ;

            // OUTPUTS
            charCompStr = outputValues.CreateDoubleCalcValue("Characteristic compressive cylinder strength", "f_{ck}", "N/mm^2", 35);
            meanCompStr = outputValues.CreateDoubleCalcValue("Mean compressive strength", "f_{cm}", "N/mm^2", 0);
            meanAxialTenStr = outputValues.CreateDoubleCalcValue("Mean axial tensile strength", "f_{ctm}", "N/mm^2", 0.3);
            secModElasticicity = outputValues.CreateDoubleCalcValue("Secant modulus of elasticity of concrete", "E_{cm}", "kN/mm^2", 22);
            k2 = outputValues.CreateDoubleCalcValue("Coefficient k2", "k_2", "", 0);
            k4 = outputValues.CreateDoubleCalcValue("Coefficient k4", "k_4", "", 0);
            desCompStrConc = outputValues.CreateDoubleCalcValue("Design compressive strength", "f_{cd}", "N/mm^2", 0);
            desCompStrConcw = outputValues.CreateDoubleCalcValue("Strength of concrete cracked in shear", "f_{cwd}", "N/mm^2", 0);
            rebarDesYieldStr = outputValues.CreateDoubleCalcValue("Design yield strength for reinforcing steel", "f_{yd}", "N/mm^2", 0);
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

            // Properties
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

            // Lever arm
            effDepth.Value = secDepth.Value - coverBtm.Value - 10 - 16;

            redistRatio.Value = bendingMom.Value * 1000000 / (secWidth.Value * Math.Pow(effDepth.Value,2) * charCompStr.Value);
            //redistRatio2.Value = (2 * effStrFac.Value * compStrCoeff.Value/partialFacConc.Value) * (1 - effCompZoneHtFac.Value * (1 - k1.Value)/(2*k2.Value)) * (effCompZoneHtFac.Value * (1-k1.Value)/(2*k2.Value));
            redistRatio2.Value = effStrFac.Value * compStrCoeff.Value / (partialFacConc.Value * k2.Value) * effCompZoneHtFac.Value * (delta.Value - k1.Value) * (1 - effCompZoneHtFac.Value * (delta.Value - k1.Value) / (2 * k2.Value));
            
            expression.Add(effDepth.Symbol + @" = h - c - \phi_{link} - \phi_{bar}/2 = " + secDepth.Value + " - " + coverBtm.Value + " - 10 - 32/2 = " + effDepth.Value + " mm");
            expression.Add(redistRatio.Symbol + @" = \frac{ " + bendingMom.Symbol + "}{" + secWidth.Symbol + effDepth.Symbol + "^2f_{ck}} = " + Math.Round(redistRatio.Value, 3));
            expression.Add(redistRatio2.Symbol + @" = " + effStrFac.Symbol + @" \frac{" + compStrCoeff.Symbol + "}{" + partialFacConc.Symbol + k2.Symbol + "}" + effCompZoneHtFac.Symbol
               + @"\frac{" + delta.Symbol + "- " + k1.Symbol + "}{" + k2.Symbol + @"}\left(1 -" + effCompZoneHtFac.Symbol + @"\frac{" + delta.Symbol + " - " + k1.Symbol + "}{2" + k2.Symbol + @"}\right) = " + Math.Round(redistRatio2.Value, 3));

            if (redistRatio.Value < redistRatio2.Value)
            {
                
                double val0 = 0.5 * effDepth.Value * (1 + Math.Pow((1 - 2 * redistRatio.Value / (effStrFac.Value * compStrCoeff.Value / partialFacConc.Value)), 0.5));
                double val1 = 0.95 * effDepth.Value;
                leverArm.Value = Math.Min(val0, val1);
                naDepth.Value = 2 * (effDepth.Value - leverArm.Value) / effCompZoneHtFac.Value;
                rebarAsReqd.Value = bendingMom.Value * 1000000 / (rebarDesYieldStr.Value * leverArm.Value);

                expression.Add(redistRatio.Symbol + @" \leq " + redistRatio2.Symbol);
                expression.Add(leverArm.Symbol + @" = min\left[\frac{d}{2}\left(1+\sqrt{1-\frac{2K}{\eta\alpha_{cc}/\gamma_c}}\right),0.95d\right] = min(" + Math.Round(val0) + "," + Math.Round(val1) + ") = " + Math.Round(leverArm.Value) + " mm");
                expression.Add(naDepth.Symbol + @" = \frac{2(" + effDepth.Symbol + "-" + leverArm.Symbol + ")}{" + effCompZoneHtFac.Symbol + "} = " + Math.Round(naDepth.Value) + " mm");
                expression.Add(string.Format(@"{0}=\frac{{{1}}}{{{2}{3}}}={4}{5}", rebarAsReqd.Symbol, bendingMom.Symbol, rebarDesYieldStr.Symbol, leverArm.Symbol, Math.Round(rebarAsReqd.Value, 0), rebarAsReqd.Unit));
            }
            else
            {
                double val0 = 0.5 * effDepth.Value * (1 + Math.Pow((1 - 2 * redistRatio2.Value / (effStrFac.Value * compStrCoeff.Value / partialFacConc.Value)), 0.5));
                double val1 = 0.95 * effDepth.Value;
                leverArm.Value = Math.Min(val0, val1);
                naDepth.Value = 2 * (effDepth.Value - leverArm.Value) / effCompZoneHtFac.Value;

                double Mp = secWidth.Value *1e-3 * Math.Pow(effDepth.Value * 1e-3, 2) * charCompStr.Value *1e3 * (redistRatio.Value - redistRatio2.Value);
                double d2 = coverTop.Value + 10 + 8;
                rebarAsReqd.Value = (redistRatio2.Value * charCompStr.Value *1e3 * secWidth.Value *1e-3 * Math.Pow(effDepth.Value*1e-3,2) / (rebarDesYieldStr.Value * leverArm.Value) 
                    + Mp / (rebarDesYieldStr.Value * (leverArm.Value - d2))) * 1e6;

                
                expression.Add(redistRatio.Symbol + @" > " + redistRatio2.Symbol);
                expression.Add(leverArm.Symbol + @" = min\left[\frac{d}{2}\left(1+\sqrt{1-\frac{2K}{\eta\alpha_{cc}/\gamma_c}}\right),0.95d\right] = min(" + Math.Round(val0) + "," + Math.Round(val1) + ") = " + Math.Round(leverArm.Value) + " mm");
                expression.Add(naDepth.Symbol + @" = \frac{2(" + effDepth.Symbol + "-" + leverArm.Symbol + ")}{" + effCompZoneHtFac.Symbol + "} = " + Math.Round(naDepth.Value) + " mm");
                expression.Add("M' = " + secWidth.Symbol + effDepth.Symbol + "^2" + charCompStr.Symbol + "(" + redistRatio.Symbol +" - "+ redistRatio2.Symbol + ") = "+Math.Round(Mp,1) + "kNm");
                expression.Add("d_2 = " + coverTop.Symbol + @"\phi_{link} + \phi_{bar/2} = " + d2 + "mm");
                expression.Add(rebarAsReqd.Symbol + @"= \frac{" + redistRatio2.Symbol + charCompStr.Symbol + secWidth.Symbol + effDepth.Symbol + "^2}{" + rebarDesYieldStr.Symbol + leverArm.Symbol + "} +"
                    + @"\frac{M'}{" + rebarDesYieldStr.Symbol + "(" + effDepth.Symbol + " - d2)} = "+Math.Round(rebarAsReqd.Value) + "mm^2");
            }
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
