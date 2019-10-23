using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcCore;
using MWGeometry;
using StructuralDrawing2D;

namespace TestCalcs
{
    public class ColumnDesign : CalcBase
    {
        // Column object
        Column MyColumn;

        // Geometry
        CalcDouble Lx;
        CalcDouble Ly;
        CalcDouble Length;
        CalcDouble Angle;

        // Material
        CalcSelectionList ConcreteGrade;
        CalcDouble MaxAggSize;

        // Loads
        CalcDouble MxTop;
        CalcDouble MxBot;
        CalcDouble MyTop;
        CalcDouble MyBot;
        CalcDouble P;

        CalcDouble Mxd;
        CalcDouble Myd;

        // Design
        CalcDouble EffectiveLength;
        CalcDouble CoverToLinks;
        CalcSelectionList BarDiameter;
        CalcSelectionList LinkDiameter;
        CalcSelectionList NRebarX;
        CalcSelectionList NRebarY;
        CalcSelectionList R; // fire resistance in min
        
        bool CapacityCheck = false;
        bool FireCheck = false;
        bool SpacingCheck = false;
        bool MinMaxSteelCheck = false;

        List<Formula> expressions = new List<Formula>();
        List<Concrete> ConcreteGrades = new List<Concrete>();
        List<int> BarDiameters = new List<int>();
        List<int> LinkDiameters = new List<int>();
        List<int> NRebars = new List<int>();
        List<int> FireResistances = new List<int>();

        // Constants for calculations
        const double gs = 1.15;
        const double gc = 1.5;
        const double acc = 0.85;

        public ColumnDesign()
        {
            // Geometry
            Lx = inputValues.CreateDoubleCalcValue("Lx", "L_x", "mm", 350);
            Ly = inputValues.CreateDoubleCalcValue("Ly", "L_y", "mm", 350);
            Length = inputValues.CreateDoubleCalcValue("Length", "L", "mm", 3150);
            Angle = inputValues.CreateDoubleCalcValue("Angle", @"\alpha", @"\deg", 0);

            // Material
            ConcreteGrade = inputValues.CreateCalcSelectionList("ConcreteGrade", "40/50", new List<string> { "32/40", "35/45", "40/50", "50/60" });
            MaxAggSize = inputValues.CreateDoubleCalcValue("MaxAggSize", "Ag", "mm", 20);

            // Loads
            MxTop = inputValues.CreateDoubleCalcValue("MxTop", "M_x^{Top}", "kN/m", 0);
            MxBot = inputValues.CreateDoubleCalcValue("MxBot", "M_x^{Bot}", "kN/m", 0);
            MyTop = inputValues.CreateDoubleCalcValue("MyTop", "M_y^{Top}", "kN/m", 0);
            MyBot = inputValues.CreateDoubleCalcValue("MyBot", "M_y^{Bot}", "kN/m", 0);
            P = inputValues.CreateDoubleCalcValue("AxialLoad", "P", "kN", 5000);

            Mxd = outputValues.CreateDoubleCalcValue("Mxd", "M_{x,Ed}", "kN/m", 0);
            Myd = outputValues.CreateDoubleCalcValue("Myd", "M_{y,Ed}", "kN/m", 0);

            //Design
            EffectiveLength = inputValues.CreateDoubleCalcValue("EffectiveLength", "L_{eff}", "", 0.7);
            CoverToLinks = inputValues.CreateDoubleCalcValue("CoverToLinks", "c", "mm", 40);
            BarDiameter = inputValues.CreateCalcSelectionList("BarDiameter", "16", new List<string> { "10", "12", "16", "20", "25", "32", "40" });
            LinkDiameter = inputValues.CreateCalcSelectionList("LinkDiameter", "10", new List<string> { "10", "12", "16", "20", "25", "32", "40" });
            NRebarX = inputValues.CreateCalcSelectionList("NRebarX", "3", new List<string> { "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            NRebarY = inputValues.CreateCalcSelectionList("NRebarY", "3", new List<string> { "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            R = inputValues.CreateCalcSelectionList("R", "120", new List<string> { "60", "90", "120", "150", "180", "240" });
            
            ConcreteGrades = new List<Concrete>()
            {
                new Concrete("32/40",32,33),
                new Concrete("35/45",35,34),
                new Concrete("40/50",40,35),
                //new Concrete("45/55",45,36),
                new Concrete("50/60",50,37),
            };

            BarDiameters = new List<int> { 10, 12, 16, 20, 25, 32, 40 };
            LinkDiameters = new List<int> { 10, 12, 16, 20, 25, 32, 40 };
            NRebars = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            FireResistances = new List<int> { 60, 90, 120, 150, 180, 240 };
            
            UpdateCalc();
        }

        public override List<Formula> GenerateFormulae()
        {
            var image = generateImage();
            expressions.Insert(0, new Formula
            {
                Narrative = "Geometry:",
                Image = image[0],
            });
            return expressions;
        }

        public override void UpdateCalc()
        { 
            expressions = new List<Formula>();
            MyColumn = new Column()
            {
                LX = Lx.Value,
                LY = Ly.Value,
                Length = Length.Value,
                Angle = Angle.Value,
                ConcreteGrade = ConcreteGrades.First(c => c.Name == ConcreteGrade.ValueAsString),
                MaxAggSize = MaxAggSize.Value,
                MxTop = MxTop.Value,
                MxBot = MxBot.Value,
                MyTop = MyTop.Value,
                MyBot = MyBot.Value,
                P = P.Value,
                EffectiveLength = EffectiveLength.Value,
                CoverToLinks = CoverToLinks.Value,
                BarDiameter = BarDiameters.First(b => b == Convert.ToInt32(BarDiameter.ValueAsString)),
                LinkDiameter = BarDiameters.First(b => b == Convert.ToInt32(LinkDiameter.ValueAsString)),
                NRebarX = NRebars.First(n => n == Convert.ToInt32(NRebarX.ValueAsString)),
                NRebarY = NRebars.First(n => n == Convert.ToInt32(NRebarY.ValueAsString)),
                R = FireResistances.First(f => f == Convert.ToInt32(R.ValueAsString))
            };
            if(Lx.Value > 0 && Ly.Value > 0)
            {
                Get3DModels();
                MinMaxSteelCheck = UpdateMinMaxSteelCheck();
                FireCheck = UpdateFireDesign();
                SpacingCheck = UpdateSecondOrderCheck();
                CapacityCheck = MyColumn.isInsideCapacity();
            }


        }

        public bool UpdateMinMaxSteelCheck()
        {
            Column c = MyColumn;

            double Ac = c.LX * c.LY;
            double Asmin = Math.Max(0.1 * c.P / (c.steelGrade.Fy * 1e-3 / gs), 0.002 * Ac);
            double Asmax = 0.04 * Ac;
            double As = 2 * (c.NRebarX + c.NRebarY - 2) * Math.PI * Math.Pow(c.BarDiameter / 2, 2);
            
            Formula f1 = new Formula();
            f1.Narrative = "Min / Max steel";
            f1.Expression = new List<string>();
            f1.Expression.Add(@"A_{s,min} = max\left(0.1N_{Ed}/f_{yd};0.002A_c\right) = " + Math.Round(Asmin) + "mm^2");
            f1.Expression.Add(@"A_{s,max} = 0.04A_c = " + Math.Round(Asmax) + "mm^2");
            f1.Expression.Add(@"A_s = " + Math.Round(As) + "mm^2");
            
            if (As > Asmax)
            {
                f1.Conclusion = "FAIL --> too much reinforcement";
                f1.Status = CalcStatus.FAIL;
                expressions.Add(f1);
                return false;
            }
            else if (As < Asmin)
            {
                f1.Conclusion = "FAIL --> not enough reinforcement";
                f1.Status = CalcStatus.FAIL;
                expressions.Add(f1);
                return false;
            }

            f1.Conclusion = "PASS";
            f1.Status = CalcStatus.PASS;
            expressions.Add(f1);
            return true;
        }

        public bool UpdateFireDesign()
        {
            bool res = false;
            Column c = MyColumn;
            double Nrd = ((c.NRebarX * c.NRebarY - 4) * Math.PI * Math.Pow(c.BarDiameter / 2, 2) * c.steelGrade.Fy + c.LX * c.LY * c.ConcreteGrade.Fc) / 1E3;
            double mufi = 0.7 * c.P / Nrd;

            // Eurocode Table 5.2.1a
            double afi = 0;
            mufi = (mufi <= 0.35) ? 0.2 : ((mufi <= 0.6) ? 0.5 : 0.7);
            if (c.fireTable.Count == 0) c.SetFireData();
            List<FireData> fdata = c.fireTable.Where(x => x.mu == mufi && x.R == c.R && x.sidesExposed == c.SidesExposed).ToList();
            fdata = fdata.OrderByDescending(x => x.minDimension).ToList();
            for (int i = 0; i < fdata.Count; i++)
            {
                if (c.LX >= fdata[i].minDimension && c.LY >= fdata[i].minDimension)
                {
                    afi = fdata[i].axisDistance;
                    break;
                }
            }
            double cminb = Math.Max(c.LinkDiameter, c.BarDiameter - c.LinkDiameter);
            double cnommin = Math.Max(afi - c.BarDiameter / 2 - c.LinkDiameter, cminb + 10);

            Formula f1 = new Formula();
            f1.Narrative = "Nominal cover for fire and bond requirements";
            f1.Ref = "EN1992-1-2 4.4.1.";
            f1.Expression = new List<string>();
            f1.Expression.Add(@"c_{min,b} = max\left(\phi_v,\phi-\phi_v\right) = " + cminb + "mm");

            f1.Expression.Add(@"\mu_{fi}=0.7\frac{N_{Ed}}{N_{Rd}} \simeq " + Math.Round(mufi, 1));
            if (afi == 0)
            {
                f1.Conclusion = "FAIL --> minimum dimension is " + c.fireTable.Where(x => x.mu == mufi && x.R == c.R).Min(x => x.minDimension) + " mm";
                res = false;
                f1.Status = CalcStatus.FAIL;
            }
            else
            {
                f1.Expression.Add(@"a_{fi} = " + afi + " mm");
                f1.Expression.Add(@"\Delta c_{dev} = 10 mm");
                f1.Expression.Add(@"c_{nom_{min}} = max \left(a_{fi} - \phi/2-\phi_v, c_{min,b} + \Delta c_{dev}\right) = " + cnommin + "mm");
                if (c.CoverToLinks >= cnommin)
                {
                    f1.Conclusion = "PASS";
                    res = true;
                    f1.Status = CalcStatus.PASS;
                }
                else
                {
                    f1.Conclusion = "FAIL";
                    f1.Status = CalcStatus.FAIL;
                }
            }

            expressions.Add(f1);
            

            return res;
        }

        public bool UpdateSecondOrderCheck()
        {
            bool spacing = false;

            Column c = MyColumn;

            List<double> sizes = new List<double>() { c.BarDiameter, c.MaxAggSize + 5, 20 };
            double smin = sizes.Max();
            double sx = (c.LX - 2 * (c.CoverToLinks + c.LinkDiameter) - c.BarDiameter) / (c.NRebarX - 1);
            double sy = (c.LX - 2 * (c.CoverToLinks + c.LinkDiameter) - c.BarDiameter) / (c.NRebarY - 1);
            Formula f0 = new Formula();
            f0.Narrative = "Bar spacing";
            f0.Expression = new List<string>();
            f0.Expression.Add(@"k_1 = 1.0 mm");
            f0.Expression.Add(@"k_2 = 5.0 mm");
            f0.Expression.Add(@"s_{min} = max\left(k_1\phi,\phi_{agg}+k_2,20 mm\right) = " + smin + " mm");
            f0.Expression.Add(@"s_x = " + Math.Round(sx) + " mm");
            f0.Expression.Add(@"s_y = " + Math.Round(sy) + " mm");
            if (sx >= smin && sy >= smin)
            {
                f0.Conclusion = "PASS";
                f0.Status = CalcStatus.PASS;
                spacing = true;
            }
            else
            {
                f0.Conclusion = "FAIL";
                f0.Status = CalcStatus.FAIL;
            }
            expressions.Add(f0);

            double abar = Math.PI * Math.Pow(c.BarDiameter / 2, 2);
            double As = (c.NRebarX * c.NRebarY - (c.NRebarX - 2) * (c.NRebarY - 2)) * Math.PI * Math.Pow(c.BarDiameter / 2.0, 2);
            double[] dxs = new double[c.NRebarY];
            dxs[0] = c.LY - c.CoverToLinks - c.LinkDiameter - c.BarDiameter / 2;

            Formula f0x = new Formula();
            f0x.Narrative = "Effective LY for bending about x axis";
            f0x.Expression = new List<string>();
            f0x.Expression.Add(@"A_{bar} = \pi\phi^2/4 = " + Math.Round(abar) + " mm^2");
            f0x.Expression.Add(@"s_y = \left(h-2(c_{nom}+\phi_v)-\phi\right)/\left(N_y-1\right) = " + Math.Round(sy) + " mm");
            f0x.Expression.Add(@"d_{x1} = h - c_{nom}-\phi_v-\phi/2 = " + Math.Round(dxs[0]) + " mm");
            for (int i = 1; i < c.NRebarY; i++)
            {
                dxs[i] = dxs[i - 1] - sy;
                f0x.Expression.Add(@"d_{x" + (i + 1).ToString() + "} = d_{x" + i + "} - s_x = " + Math.Round(dxs[i]) + "mm");
            }
            string s = @"I_{sx} = 2 A_{bar}N_y(d_{x1}-h/2)^2";
            double Isx = 2 * abar * c.NRebarX * Math.Pow((dxs[0] - c.LY / 2), 2);
            for (int i = 1; i < c.NRebarY / 2; i++)
            {
                Isx += 2 * abar * 2 * Math.Pow((dxs[i] - c.LY / 2), 2);
                s += "4 A_{bar}N_y(d_{x" + (i + 1).ToString() + "}-h/2)^2";
            }
            f0x.Expression.Add(s + " = " + Math.Round(Isx / 1E4) + "cm^4");
            double isx = Math.Sqrt(Isx / As);
            f0x.Expression.Add(@"i_{sx} = \sqrt{I_{sy}/A_s} = " + Math.Round(isx) + " mm");
            double dx = c.LY / 2 + isx;
            f0x.Expression.Add(@"d_x = h/2 + i_{sx} = " + Math.Round(dx) + " mm");
            expressions.Add(f0x);

            double[] dys = new double[c.NRebarX];
            dys[0] = c.LX - c.CoverToLinks - c.LinkDiameter - c.BarDiameter / 2;

            Formula f0y = new Formula();
            f0y.Narrative = "Effective LY for bending about x axis";
            f0y.Expression = new List<string>();
            f0y.Expression.Add(@"A_{bar} = \pi\phi^2/4 = " + Math.Round(abar) + " mm^2");
            f0y.Expression.Add(@"s_x = \left(b-2(c_{nom}+\phi_v)-\phi\right)/\left(N_x-1\right) = " + Math.Round(sx) + " mm");
            f0y.Expression.Add(@"d_{y1} = h - c_{nom}-\phi_v-\phi/2 = " + Math.Round(dys[0]) + " mm");
            for (int i = 1; i < c.NRebarX; i++)
            {
                dys[i] = dys[i - 1] - sx;
                f0y.Expression.Add(@"d_{y" + (i + 1).ToString() + "} = d_{y" + i + "} - s_x = " + Math.Round(dys[i]) + "mm");
            }
            s = @"I_{sy} = 2 A_{bar}N_x(d_{y1}-b/2)^2";
            double Isy = 2 * abar * c.NRebarY * Math.Pow((dys[0] - c.LX / 2), 2);
            for (int i = 1; i < c.NRebarX / 2; i++)
            {
                Isy += 2 * abar * 2 * Math.Pow((dys[i] - c.LX / 2), 2);
                s += "4 A_{bar}N_x(d_{y" + (i + 1).ToString() + "}-b/2)^2";
            }
            f0y.Expression.Add(s + " = " + Math.Round(Isy / 1E4) + "cm^4");
            double isy = Math.Sqrt(Isy / As);
            f0y.Expression.Add(@"i_{sy} = \sqrt{I_{sy}/A_s} = " + Math.Round(isy) + " mm");
            double dy = c.LX / 2 + isy;
            f0y.Expression.Add(@"d_y = b/2 + i_{sy} = " + Math.Round(dy) + " mm");
            expressions.Add(f0y);

            double l0 = c.EffectiveLength * c.Length;
            Formula f1 = new Formula();
            f1.Narrative = "Column effective length";
            f1.Expression = new List<string>();
            f1.Expression.Add(@"l_{0} = f \times l = " + l0 + " mm");
            expressions.Add(f1);

            double ix = c.LY / Math.Sqrt(12);
            double lambdax = l0 / ix;
            Formula f2 = new Formula();
            f2.Narrative = "Column slenderness about x axis";
            f2.Ref = "5.8.3.2";
            f2.Expression = new List<string>();
            f2.Expression.Add(@"i_x = h/\sqrt{12} = " + Math.Round(ix, 1) + " mm");
            f2.Expression.Add(@"\lambda_x = l_0/i_x = " + Math.Round(lambdax, 1));
            expressions.Add(f2);

            double iy = c.LX / Math.Sqrt(12);
            double lambday = l0 / iy;
            Formula f3 = new Formula();
            f3.Narrative = "Column slenderness about y axis";
            f3.Ref = "5.8.3.2";
            f3.Expression = new List<string>();
            f3.Expression.Add(@"i_y = b/\sqrt{12} = " + Math.Round(iy, 1) + " mm");
            f3.Expression.Add(@"\lambda_y = l_0/i_y = " + Math.Round(lambday, 1));
            expressions.Add(f3);

            double ei = l0 / 400;
            double M01x = Math.Min(Math.Abs(c.MxTop), Math.Abs(c.MxBot)) + ei * c.P / 1E3;
            double M02x = Math.Max(Math.Abs(c.MxTop), Math.Abs(c.MxBot)) + ei * c.P / 1E3;
            Formula f4 = new Formula();
            f4.Narrative = "Moments about x axis including imperfections";
            f4.Expression = new List<string>();
            f4.Expression.Add(@"e_{i} = l_0/400 = " + Math.Round(ei, 1) + " mm");
            f4.Expression.Add(@"M_{01x} = min\left(\left|M_x^{top}\right|,\left|M_x^{bot}\right|\right) - e_iN_{Ed} = " + Math.Round(M01x, 1) + " kN.m");
            f4.Expression.Add(@"M_{02x} = max\left(\left|M_x^{top}\right|,\left|M_x^{bot}\right|\right) + e_iN_{Ed} = " + Math.Round(M02x, 1) + " kN.m");
            expressions.Add(f4);

            double omega = As * c.steelGrade.Fy / gs /
                (c.LX * c.LY * acc * c.ConcreteGrade.Fc / gc);
            double B = Math.Sqrt(1 + 2 * omega);
            double rmx = M01x / M02x;
            double C = 1.7 - rmx;
            double n = c.P / (c.LX * c.LY * acc * c.ConcreteGrade.Fc / gc) * 1E3;
            double lambdaxlim = 20 * 0.7 * B * C / Math.Sqrt(n);
            bool secondorderx = false;
            Formula f5 = new Formula();
            f5.Narrative = "Slenderness limit about x axis";
            f5.Expression = new List<string>();
            f5.Expression.Add(@"A = 0.7");
            f5.Expression.Add(@"\omega = A_s f_{yd} / \left( A_c f_{cd} \right) = " + Math.Round(omega, 3));
            f5.Expression.Add(@"B = \sqrt{\left(1+2\omega\right)} = " + Math.Round(B, 2));
            f5.Expression.Add(@"r_{mx} = " + Math.Round(rmx, 3));
            f5.Expression.Add(@"C = 1.7 - r_mx = " + Math.Round(C, 3));
            f5.Expression.Add(@"n = N_{Ed} / \left( A_cf_{cd} \right) = " + Math.Round(n, 3));
            f5.Expression.Add(@"\lambda_{limx} = 20ABC/\sqrt{n} = " + Math.Round(lambdaxlim, 1));
            if (lambdax < lambdaxlim)
            {
                f5.Expression.Add(@"\lambda_x < \lambda_{xlim}");
                f5.Conclusion = "Second order effects may be ignored";
            }
            else
            {
                f5.Expression.Add(@"\lambda_x >= \lambda_{xlim}");
                f5.Conclusion = "Second order effects must be considered";
                secondorderx = true;
            }
            expressions.Add(f5);

            double M01y = Math.Min(Math.Abs(c.MyTop), Math.Abs(c.MyBot)) + ei * c.P / 1E3;
            double M02y = Math.Max(Math.Abs(c.MyTop), Math.Abs(c.MyBot)) + ei * c.P / 1E3;
            Formula f6 = new Formula();
            f6.Narrative = "Moments about y axis including imperfections";
            f6.Expression = new List<string>();
            f6.Expression.Add(@"e_{i} = l_0/400 = " + Math.Round(ei, 1) + " mm");
            f6.Expression.Add(@"M_{01y} = min\left(\left|M_y^{top}\right|,\left|M_y^{bot}\right|\right) - e_iN_{Ed} = " + Math.Round(M01y, 1) + " kN.m");
            f6.Expression.Add(@"M_{02y} = max\left(\left|M_y^{top}\right|,\left|M_y^{bot}\right|\right) + e_iN_{Ed} = " + Math.Round(M02y, 1) + " kN.m");
            expressions.Add(f6);

            double rmy = M01y / M02y;
            C = 1.7 - rmy;
            double lambdaylim = 20 * 0.7 * B * C / Math.Sqrt(n);
            bool secondordery = false;
            Formula f7 = new Formula();
            f7.Narrative = "Slenderness limit about y axis";
            f7.Expression = new List<string>();
            f7.Expression.Add(@"A = 0.7");
            f7.Expression.Add(@"\omega = A_s f_{yd} / \left( A_c f_{cd} \right) = " + Math.Round(omega, 3));
            f7.Expression.Add(@"B = \sqrt{\left(1+2\omega\right)} = " + Math.Round(B, 2));
            f7.Expression.Add(@"r_{my} = " + Math.Round(rmy, 3));
            f7.Expression.Add(@"C = 1.7 - r_{my} = " + Math.Round(C, 3));
            f7.Expression.Add(@"n = N_{Ed} / \left( A_cf_{cd} \right) = " + Math.Round(n, 3));
            f7.Expression.Add(@"\lambda_{limx} = 20ABC/\sqrt{n} = " + Math.Round(lambdaylim, 1));
            if (lambday < lambdaylim)
            {
                f7.Expression.Add(@"\lambda_y < \lambda_{ylim}");
                f7.Conclusion = "Second order effects may be ignored";
            }
            else
            {
                f7.Expression.Add(@"\lambda_y >= \lambda_{ylim}");
                f7.Conclusion = "Second order effects must be considered";
                secondordery = true;
            }
            expressions.Add(f7);

            Formula f8 = new Formula();
            if (!secondorderx)
            {
                double Medx = Math.Max(M02x, c.P * Math.Max(c.LY * 1E-3 / 30, 20 * 1E-3));
                c.Mxd = Math.Round(Medx, 1);
                f8.Narrative = "Design moment about x for a stocky column";
                f8.Expression = new List<string>();
                f8.Expression.Add(@"M_{Edx} = max \left( M_{02x}, N_{Ed}\times max \left(h/30,20 mm\right)\right) = " + Math.Round(Medx) + "kN.m");
            }
            else
            {
                double u = 2 * (c.LX + c.LY);
                double nu = 1 + omega;
                double Kr = Math.Min(1, (nu - n) / (nu - 0.4));
                double d = c.LY - c.CoverToLinks - c.LinkDiameter - c.BarDiameter / 2;
                double eyd = c.steelGrade.Fy / gs / c.steelGrade.E / 1E3;
                double r0 = 0.45 * dx / eyd;
                double h0 = 2 * c.LY * c.LX / u;
                double alpha1 = Math.Pow(35 / (c.ConcreteGrade.Fc + 8), 0.7);
                double alpha2 = Math.Pow(35 / (c.ConcreteGrade.Fc + 8), 0.2);
                double phiRH = (1 + (0.5 / (0.1 * Math.Pow(h0, 1.0 / 3))) * alpha1) * alpha2;
                double bfcm = 16.8 / Math.Sqrt(c.ConcreteGrade.Fc + 8);
                double bt0 = 1 / (0.1 + Math.Pow(7, 0.2));
                double phi0 = phiRH * bfcm * bt0;
                double phiInf = phi0;
                double phiefy = phiInf * 0.8;
                double betay = 0.35 + c.ConcreteGrade.Fc / 200 - lambdax / 150;
                double kphiy = Math.Max(1, 1 + betay * phiefy);
                double r = r0 / (Kr * kphiy);
                double e2x = Math.Pow(l0, 2) / (r * 10);
                double m2x = c.P * e2x * 1E-3;
                double M0e = 0.6 * M02x + 0.4 * M01x;
                List<double> Ms = new List<double>() { M02x, M0e + m2x, M01x + 0.5 * m2x, Math.Max(c.LY * 1E-3 / 30, 20 * 1E-3) * c.P };
                double Medx = Ms.Max();
                c.Mxd = Math.Round(Medx, 1);

                f8.Narrative = "Design moment about x for a slender column";
                f8.Ref = "5.8.8";
                f8.Expression = new List<string>();
                f8.Expression.Add(@"RH = 50\%");
                f8.Expression.Add(@"u = " + u + " mm");
                f8.Expression.Add(@"t_{0} = 7 days");
                f8.Expression.Add(@"n_u = 1 + \omega = " + Math.Round(nu, 3));
                f8.Expression.Add(@"n_{bal} = 0.4");
                f8.Expression.Add(@"K_r = min\left(1.0,(n_u-n)/(n_u-n_{bal})\right) = " + Math.Round(Kr, 3));
                f8.Expression.Add(@"\varepsilon_{yd} = f_{yd}/E_s = " + Math.Round(eyd, 5));
                f8.Expression.Add(@"1/r_0 = \varepsilon_{yd}/\left(0.45d\right) = " + Math.Round(1 / r0, 7) + @"mm^{-1}");
                f8.Expression.Add(@"h_0 = 2 A_c/u = " + Math.Round(h0));
                f8.Expression.Add(@"\alpha_1 = (35 / f_{cm})^{0.7} = " + Math.Round(alpha1, 3));
                f8.Expression.Add(@"\alpha_2 = (35 / f_{cm})^{0.2} = " + Math.Round(alpha2, 3));
                f8.Expression.Add(@"\phi_{RH}=\left[1+\left((1-RH/100\%)/(0.1\times(h_0)^{1/3})\right)\times \alpha1\right]\times \alpha2 = " + Math.Round(phiRH, 3));
                f8.Expression.Add(@"\beta_{fcm}=16.8/\sqrt{f_{cm}} = " + Math.Round(bfcm, 3));
                f8.Expression.Add(@"\beta_{t_0} = 1/(0.1 + t_0^{0.2} = " + Math.Round(bt0, 3));
                f8.Expression.Add(@"\phi_0 = \phi_{RH}\beta_{fcm}\beta{t0} = " + Math.Round(phi0, 3));
                f8.Expression.Add(@"\beta_{c\infty} = 1.00");
                f8.Expression.Add(@"\phi_{\infty} = \phi_0\beta_{c\infty} = " + Math.Round(phiInf, 3));
                f8.Expression.Add(@"r_{Mx} = 0.80");
                f8.Expression.Add(@"\phi_{efy}=\phi_{\infty}r_{My} = " + Math.Round(phiefy, 3));
                f8.Expression.Add(@"\beta_y = 0.35 + f_{ck}/200 - \lambda_x/150 = " + Math.Round(betay, 3));
                f8.Expression.Add(@"K_{\phi x} = max(1, 1 + \beta_y\phi_{efy}) = " + Math.Round(kphiy, 3));
                f8.Expression.Add(@"1/r = K_rK_{\phi x}/r_0 = " + Math.Round(1 / r, 7) + @"mm^{-1}");
                f8.Expression.Add("c = 10");
                f8.Expression.Add("e_{2x} = l_0^2/(rc) = " + Math.Round(e2x) + " mm");
                f8.Expression.Add(@"M_{2x} = N_{Ed} \times e_{2x} = " + Math.Round(m2x, 1) + "kN.m");
                f8.Expression.Add(@"M_{0e} = 0.6M_{02x} + 0.4M_{01x} = " + Math.Round(M0e, 1) + "kN.m");
                f8.Expression.Add(@"M_{Edx} = max\left(M_{02}, M_{0e}+M_{2x},M_{01}+0.5M_{2x},e_0N_{Ed}\right) = " + Math.Round(Medx, 1) + " kN.m");
            }
            expressions.Add(f8);

            Formula f9 = new Formula();
            if (!secondordery)
            {
                double Medy = Math.Max(M02y, c.P * Math.Max(c.LX * 1E-3 / 30, 20 * 1E-3));
                c.Myd = Math.Round(Medy, 1);
                f9.Narrative = "Design moment about y for a stocky column";
                f9.Expression = new List<string>();
                f9.Expression.Add(@"M_{Edy} = max \left( M_{02y}, N_{Ed}\times max \left(h/30,20 mm\right)\right) = " + Math.Round(Medy) + "kN.m");
            }
            else
            {
                double u = 2 * (c.LX + c.LY);
                double nu = 1 + omega;
                double Kr = Math.Min(1, (nu - n) / (nu - 0.4));
                double d = c.LY - c.CoverToLinks - c.LinkDiameter - c.BarDiameter / 2;
                double eyd = c.steelGrade.Fy / gs / c.steelGrade.E / 1E3;
                double r0 = 0.45 * dy / eyd;
                double h0 = 2 * c.LY * c.LX / u;
                double alpha1 = Math.Pow(35 / (c.ConcreteGrade.Fc + 8), 0.7);
                double alpha2 = Math.Pow(35 / (c.ConcreteGrade.Fc + 8), 0.2);
                double phiRH = (1 + (0.5 / (0.1 * Math.Pow(h0, 1.0 / 3))) * alpha1) * alpha2;
                double bfcm = 16.8 / Math.Sqrt(c.ConcreteGrade.Fc + 8);
                double bt0 = 1 / (0.1 + Math.Pow(7, 0.2));
                double phi0 = phiRH * bfcm * bt0;
                double phiInf = phi0;
                double phiefy = phiInf * 0.8;
                double betay = 0.35 + c.ConcreteGrade.Fc / 200 - lambday / 150;
                double kphiy = Math.Max(1, 1 + betay * phiefy);
                double r = r0 / (Kr * kphiy);
                double e2y = Math.Pow(l0, 2) / (r * 10);
                double m2y = c.P * e2y * 1E-3;
                double M0e = 0.6 * M02y + 0.4 * M01y;
                List<double> Ms = new List<double>() { M02y, M0e + m2y, M01y + 0.5 * m2y, Math.Max(c.LX * 1E-3 / 30, 20 * 1E-3) * c.P };
                double Medy = Ms.Max();
                c.Myd = Math.Round(Medy, 1);

                f9.Narrative = "Design moment about y for a slender column";
                f9.Ref = "5.8.8";
                f9.Expression = new List<string>();
                f9.Expression.Add(@"RH = 50\%");
                f9.Expression.Add(@"u = " + u + " mm");
                f9.Expression.Add(@"t_{0} = 7 days");
                f9.Expression.Add(@"n_u = 1 + \omega = " + Math.Round(nu, 3));
                f9.Expression.Add(@"n_{bal} = 0.4");
                f9.Expression.Add(@"K_r = min\left(1.0,(n_u-n)/(n_u-n_{bal})\right) = " + Math.Round(Kr, 3));
                f9.Expression.Add(@"\varepsilon_{yd} = f_{yd}/E_s = " + Math.Round(eyd, 5));
                f9.Expression.Add(@"1/r_0 = \varepsilon_{yd}/\left(0.45d\right) = " + Math.Round(1 / r0, 7) + @"mm^{-1}");
                f9.Expression.Add(@"h_0 = 2 A_c/u = " + Math.Round(h0));
                f9.Expression.Add(@"\alpha_1 = (35 / f_{cm})^{0.7} = " + Math.Round(alpha1, 3));
                f9.Expression.Add(@"\alpha_2 = (35 / f_{cm})^{0.2} = " + Math.Round(alpha2, 3));
                f9.Expression.Add(@"\phi_{RH}=\left[1+\left((1-RH/100\%)/(0.1\times(h_0)^{1/3})\right)\times \alpha1\right]\times \alpha2 = " + Math.Round(phiRH, 3));
                f9.Expression.Add(@"\beta_{fcm}=16.8/\sqrt{f_{cm}} = " + Math.Round(bfcm, 3));
                f9.Expression.Add(@"\beta_{t_0} = 1/(0.1 + t_0^{0.2} = " + Math.Round(bt0, 3));
                f9.Expression.Add(@"\phi_0 = \phi_{RH}\beta_{fcm}\beta{t0} = " + Math.Round(phi0, 3));
                f9.Expression.Add(@"\beta_{c\infty} = 1.00");
                f9.Expression.Add(@"\phi_{\infty} = \phi_0\beta_{c\infty} = " + Math.Round(phiInf, 3));
                f9.Expression.Add(@"r_{Mx} = 0.80");
                f9.Expression.Add(@"\phi_{efy}=\phi_{\infty}r_{My} = " + Math.Round(phiefy, 2));
                f9.Expression.Add(@"\beta_y = 0.35 + f_{ck}/200 - \lambda_y/150 = " + Math.Round(betay, 3));
                f9.Expression.Add(@"K_{\phi x} = max(1, 1 + \beta_y\phi_{efy}) = " + Math.Round(kphiy, 3));
                f9.Expression.Add(@"1/r = K_rK_{\phi x}/r_0 = " + Math.Round(1 / r, 7) + @"mm^{-1}");
                f9.Expression.Add("c = 10");
                f9.Expression.Add("e_{2y} = l_0^2/(rc) = " + Math.Round(e2y) + " mm");
                f9.Expression.Add(@"M_{2y} = N_{Ed}\times e_{2y} = " + Math.Round(m2y, 1) + "kN.m");
                f9.Expression.Add(@"M_{0e} = 0.6M_{02y} + 0.4M_{01y} = " + Math.Round(M0e, 1) + "kN.m");
                f9.Expression.Add(@"M_{Edy} = max\left(M_{02}, M_{0e}+M_{2y},M_{01y}+0.5M_{2y},e_0N_{Ed}\right) = " + Math.Round(Medy, 1) + " kN.m");
            }
            expressions.Add(f9);

            return spacing;
        }

        private List<SkiaSharp.SKBitmap> generateImage()
        {
            double sf = 1;
            Column c = MyColumn;

            DisplayDataSet dataset = DisplayDataSet.GetDisplayDataSet();
            dataset.AddFormattingInstruction("ColumnOutline", 
                                             new SkiaSharp.SKPaint { StrokeWidth = 2, Color = SkiaSharp.SKColors.Gray }, 
                                             new SkiaSharp.SKPaint { StrokeWidth = 6, Color = SkiaSharp.SKColors.Transparent });
            dataset.AddFormattingInstruction("Rebar", 
                                             new SkiaSharp.SKPaint { StrokeWidth = 2, Color = SkiaSharp.SKColors.Brown }, 
                                             new SkiaSharp.SKPaint { StrokeWidth = 15, Color = SkiaSharp.SKColors.Transparent });
            StructuralDrawing2D.StructuralDrawing2D drawing = new StructuralDrawing2D.StructuralDrawing2D(dataset);
            
            // axis
            var pathX = new SkiaSharp.SKPath();
            pathX.MoveTo((float)(-c.LX * sf), 0);
            pathX.LineTo((float)(c.LX * sf), 0);
            drawing.AddElement(DisplayFormatPreset.Gridline, pathX, false);

            var pathY = new SkiaSharp.SKPath();
            pathY.MoveTo(0, (float)(-c.LY * sf));
            pathY.LineTo(0, (float)(c.LY * sf));
            drawing.AddElement(DisplayFormatPreset.Gridline, pathY, false);

            // column shape
            var path = new SkiaSharp.SKPath();
            var rect = new SkiaSharp.SKRect((float)(-c.LX / 2 * sf), (float)(c.LY / 2 * sf), (float)(c.LX / 2 * sf), (float)(-c.LY / 2 * sf));
            path.AddRect(rect);
            drawing.AddElement("ColumnOutline", path, true);

            drawing.AddText(new DrawingText("X", drawing.GetBounds().Right + 50, 5, 25));
            drawing.AddText(new DrawingText("X", drawing.GetBounds().Left - 50, 5, 25));
            drawing.AddText(new DrawingText("Y", 0, drawing.GetBounds().Top - 50, 25));
            drawing.AddText(new DrawingText("Y", 0, drawing.GetBounds().Bottom + 50, 25));

            // rebars
            double dx = (c.LX - 2 * (c.CoverToLinks + c.LinkDiameter) - c.BarDiameter) / (c.NRebarX - 1);
            double dy = (c.LY - 2 * (c.CoverToLinks + c.LinkDiameter) - c.BarDiameter) / (c.NRebarY - 1);

            for(int i = 0; i < c.NRebarX; i++)
            {
                for(int j = 0; j < c.NRebarY; j++)
                {
                    if(i == 0 || i == c.NRebarX-1 || j == 0 || j == c.NRebarY-1)
                    {
                        var pathR = new SkiaSharp.SKPath();

                        double x = c.CoverToLinks + c.LinkDiameter + c.BarDiameter / 2 + i * dx - c.LX / 2;
                        double y = c.CoverToLinks + c.LinkDiameter + c.BarDiameter / 2 + j * dy - c.LY / 2;
                        pathR.AddCircle((float)(x * sf), (float)(y * sf), (float)(c.BarDiameter / 2 * sf));

                        drawing.AddElement("Rebar", pathR, true);
                    }
                }
            }

            
            var bitmap = drawing.GenerateBitmapImage(600, 600, 100);
            return new List<SkiaSharp.SKBitmap> { bitmap };
        }

        public override List<MW3DModel> Get3DModels()
        {
            MyColumn.GetInteractionDiagram();

            List<MW3DModel> Models = new List<MW3DModel>();
            MWMesh myMesh = new MWMesh();

            for(int i = 0; i < MyColumn.diagramVertices.Count; i++)
            {
                MWPoint3D pt = MyColumn.diagramVertices[i];
                myMesh.addNode(pt.X, pt.Y, pt.Z, MWPoint2D.Point2DByCoordinates(0, 0));
            }

            List<int[]> indicesList = new List<int[]>();
            for(int i = 0; i < MyColumn.diagramFaces.Count; i++)
            {
                var f = MyColumn.diagramFaces[i];
                int[] indices = new int[]
                {
                    MyColumn.diagramVertices.IndexOf(f.Points[0]),
                    MyColumn.diagramVertices.IndexOf(f.Points[1]),
                    MyColumn.diagramVertices.IndexOf(f.Points[2])
                };
                indicesList.Add(indices);
            }
            myMesh.setIndices(indicesList);

            myMesh.Brush = new MWBrush(200, 50, 50);
            myMesh.Opacity = 0.5;

            MW3DModel myID = new MW3DModel(myMesh);

            Models.Add(myID);

            return Models;
        }
    }
}
