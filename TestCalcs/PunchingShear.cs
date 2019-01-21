using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CalcCore;
//using netDxf.Entities;
//using netDxf;
using netDxf.Tables;

namespace TestCalcs
{
    public class PunchingShear : CalcCore.CalcBase
    {
        CalcSelectionList _colType;
        CalcSelectionList _linkArrangement;
        CalcDouble _fck;
        CalcSelectionList _loadCombType;
        CalcDouble _concPartFactor;
        CalcDouble _columnAdim;
        CalcDouble _columnBdim;
        CalcCore.CalcDouble _punchingLoad;
        CalcDouble _mz;
        CalcDouble _my;
        double udl = 0;
        CalcDouble _h;
        CalcDouble _offsety;
        CalcDouble _offsetz;
        double dy;
        double dz;
        double d_average;
        double ui;
        CalcCore.CalcDouble _beta;
        CalcDouble _stressvEdi;
        double stressvEd1;
        double reinforcementRatioy;
        double reinforcementRatioz;
        double vRdc;
        double vRdMax;
        double u1;
        CalcDouble _Asw;
        CalcDouble _rebarY;
        CalcDouble _rebarZ;
        CalcDouble _rho;
        CalcSelectionList _srMode;
        CalcDouble _srTarget;
        CalcSelectionList _stMode;
        CalcDouble _stTarget;
        double sr;
        double st;
        double fywd;
        double fywdef;
        CalcDouble _uoutef;
        CalcDouble _distToFirstLinkPerim;
        CalcDouble _perimSpacing;
        CalcDouble _linksInFirstPerim;
        CalcDouble _numberOfPerimeters;
        CalcDouble _legsTotal;
        CalcDouble _legDia;
        CalcSelectionList _dxfOpt;
        CalcFolderPath _dxfFolder;
        CalcDouble _holePosX;
        CalcDouble _holePosY;
        CalcDouble _holeSizeX;
        CalcDouble _holeSizeY;
        CalcDouble _hole2PosX;
        CalcDouble _hole2PosY;
        CalcDouble _hole2SizeX;
        CalcDouble _hole2SizeY;

        List<Formula> expressions = new List<Formula>();
        List<Tuple<Line, Line>> _holeEdges;
        List<PolyLine> controlPerimeters;
        PolyLine controlPerimeterNoHoles;
        List<PolyLine> outerPerimeters;
        PolyLine columnOutline;
        PolyLine fullColumnOutline;
        List<PolyLine> columnOutlines;
        List<PolyLine> holeOutlines;
        List<List<Vector2>> allHoleCorners;
        PolyLine slabEdge;
        List<PolyLine> shearLinkStartPerimeters;
        List<PolyLine> shearLinkEndPerimeters;
        List<Line> shearSpurs;
        List<List<Vector2>> shearLinks;
        List<List<PolyLine>> allPerimeters;
        List<List<PolyLine>> perimetersToReinforce;
        List<PolyLine> u1reducedNoHoles;
        List<PolyLine> u1reduced;


        public PunchingShear()
        {
            _colType = inputValues.CreateCalcSelectionList("Column condition", "INTERNAL", new List<string> { "INTERNAL", "EDGE", "CORNER" });
            _linkArrangement = inputValues.CreateCalcSelectionList("Shear link arrangement", "GRID", new List<string> { "SPURS_AUTO", "GRID", "CRUCIFORM" });
            _srMode = inputValues.CreateCalcSelectionList("Radial spacing", "AUTO", new List<string> { "AUTO", "TARGET" });
            _srTarget = inputValues.CreateDoubleCalcValue("Target radial spacing", "s_r", "mm", 0);
            _stMode = inputValues.CreateCalcSelectionList("Tangential spacing", "AUTO", new List<string> { "AUTO", "TARGET" });
            _stTarget = inputValues.CreateDoubleCalcValue("Target tangential spacing", "s_t", "mm", 0);
            _beta = outputValues.CreateDoubleCalcValue("Beta value", @"\beta", "", 2);
            _columnAdim = inputValues.CreateDoubleCalcValue("Column A dimension", "A", "mm", 350);
            _columnBdim = inputValues.CreateDoubleCalcValue("Column B dimension", "B", "mm", 350);
            _h = inputValues.CreateDoubleCalcValue("Slab depth", "h", "mm", 225);
            _offsety = inputValues.CreateDoubleCalcValue("Offset to effective depth y dir", "d_{y,offset}", "mm", 45);
            _offsetz = inputValues.CreateDoubleCalcValue("Offset to effective depth z dir", "d_{z,offset}", "mm", 65);
            _rebarY = inputValues.CreateDoubleCalcValue("Tension reinforcement y dir", "A_{s,y}", @"mm^2/m", 1695);
            _rebarZ = inputValues.CreateDoubleCalcValue("Tension reinforcement z dir", "A_{s,z}", @"mm^2/m", 1695);
            _my = inputValues.CreateDoubleCalcValue("Moment about y axis", "M_{Ed,y}", "kNm", 33);
            _mz = inputValues.CreateDoubleCalcValue("Moment about z axis", "M_{Ed, z}", "kNm", 90);
            _fck = inputValues.CreateDoubleCalcValue("Concrete strength", @"f_{ck}", @"N/{mm^2}", 40);
             _loadCombType = inputValues.CreateCalcSelectionList("Design situation", "PERMANENT", new List<string> {"PERMANENT", "ACCIDENTAL" });
            _concPartFactor = outputValues.CreateDoubleCalcValue("Partial factor for concrete", @"\gamma_c", "", 1.5);
            _rho = outputValues.CreateDoubleCalcValue("Reinforcement ratio", @"\rho_l", "", 0);
            _punchingLoad = inputValues.CreateDoubleCalcValue("Punching shear load", "V_{Ed}", "kN", 457);
            _stressvEdi = outputValues.CreateDoubleCalcValue("Shear stress at column face", @"v_{Ed,0}", @"N/{mm^2} ", 0);
            _Asw = outputValues.CreateDoubleCalcValue("Punching shear leg area required per perimeter", "A_{sw}", "mm^2", 0);
            _uoutef = outputValues.CreateDoubleCalcValue("Outer perimeter required", "u_{out,ef,req}", "mm", 0);
            _linksInFirstPerim = outputValues.CreateDoubleCalcValue("Number of spurs", "", "No.", 0);
            _numberOfPerimeters = outputValues.CreateDoubleCalcValue("Legs per spur", "", "No.", 0);
            _perimSpacing = outputValues.CreateDoubleCalcValue("Spacing of link perimeters", "", "mm", 0);
            _distToFirstLinkPerim = outputValues.CreateDoubleCalcValue("Distance to first link perimeter", "", "mm", 0);
            _legsTotal = outputValues.CreateDoubleCalcValue("Total legs", "", "No.", 0);
            _legDia = outputValues.CreateDoubleCalcValue("Leg diameter", "", "mm", 0);
            _dxfOpt = inputValues.CreateCalcSelectionList("DXF output option", "NONE", new List<string> { "NONE", "SPECIFIED_FOLDER" });
            _dxfFolder = inputValues.CreateCalcFolderPath("DXF output folder", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            _holePosX = inputValues.CreateDoubleCalcValue("Hole 1 X position", "", "mm", 0);
            _holePosY = inputValues.CreateDoubleCalcValue("Hole 1 Y position", "", "mm", 220);
            _holeSizeX = inputValues.CreateDoubleCalcValue("Hole 1 X size", "", "mm", 0);
            _holeSizeY = inputValues.CreateDoubleCalcValue("Hole 1 Y size", "", "mm", 200);
            _hole2PosX = inputValues.CreateDoubleCalcValue("Hole 2 X position", "", "mm", 500);
            _hole2PosY = inputValues.CreateDoubleCalcValue("Hole 2 Y position", "", "mm", -300);
            _hole2SizeX = inputValues.CreateDoubleCalcValue("Hole 2 X size", "", "mm", 00);
            _hole2SizeY = inputValues.CreateDoubleCalcValue("Hole 2 Y size", "", "mm", 150);
            UpdateCalc();
        }


        public override List<Formula> GenerateFormulae()
        {
            return expressions;
        }

        private void resetFields()
        {
            controlPerimeters = new List<PolyLine>();
            controlPerimeterNoHoles = null;
            outerPerimeters = new List<PolyLine>();
            columnOutline = null;
            fullColumnOutline = null;
            columnOutlines = new List<PolyLine>();
            holeOutlines = null;
            allHoleCorners = null;
            slabEdge = null;
            shearLinkStartPerimeters = new List<PolyLine>();
            shearLinkEndPerimeters = new List<PolyLine>();
            shearSpurs = new List<Line>();
            shearLinks = new List<List<Vector2>>();
            allPerimeters = new List<List<PolyLine>>();
            perimetersToReinforce = new List<List<PolyLine>>();
            _holeEdges = null;
            u1reducedNoHoles = null;
            u1reduced = null;
        }

        public override void UpdateCalc()
        {
            formulae = null;
            resetFields();
            expressions = new List<Formula>();

            if (_stMode.ValueAsString == "AUTO")
            {
                st = 1.5 * d_average;
            }
            else
            {
                st = Math.Min(_stTarget.Value, 1.5 * d_average);
            }

            // calculate effective depths in each direction
            dy = _h.Value - _offsety.Value;
            dz = _h.Value - _offsetz.Value;
            d_average = (dy + dz) / 2;
            if (_loadCombType.ValueAsString == "PERMANENT") _concPartFactor.Value = 1.5;
            else _concPartFactor.Value = 1.2;
            expressions.Add(new Formula
            {
                Ref = "cl.2.4.2.4(1)",
                Narrative = "Partial factor for concrete",
                Expression = new List<string>
                {
                    _concPartFactor.Symbol + @"=" + _concPartFactor.Value
                }
            });

            expressions.Add(new Formula()
            {
                Narrative = "Calculate effective depths",
                Expression = new List<string>
                {
                    string.Format(@"{0} = {1} - {2} = {3}{4}", "d_y", _h.Symbol, _offsety.Symbol, dy, "mm"),
                    string.Format(@"{0} = {1} - {2} = {3}{4}", "d_z", _h.Symbol, _offsetz.Symbol, dz, "mm"),
                    string.Format(@"{0} = \frac{{{1} + {2}}}{{2}} = {3}{4}", @"d_{eff}", "d_y", "d_z", d_average, "mm")
                },
                Ref = "cl. 6.4.2(1)"
            });

            generateHoles();
            generateColumnOutlines();
            generateSlabEdge();
            controlPerimeterNoHoles = generatePerimeter(_columnAdim.Value, _columnBdim.Value, 2 * d_average);
            controlPerimeters = generatePerimeterWithHoles(2 * d_average);
            u1reducedNoHoles = new List<PolyLine> { generateReducedControlPerimeter(_columnAdim.Value, _columnBdim.Value) };
            u1reduced = generateReducedControlPerimeterWithHoles();
            ui = columnOutlines.Sum(a => a.Length);
            u1 = controlPerimeters.Sum(a => a.Length);

            expressions.Add(new Formula
            {
                Narrative = "Determine the effective column face and first control perimeters taking into account the effect of holes.",
                Expression = new List<string>
                {
                    string.Format("u_{{0,no holes}} = {0}mm", Math.Round(columnOutline.Length, 2)),
                    string.Format(@"u_0 = {0} mm", Math.Round(ui, 2)),
                    Environment.NewLine,
                    "u_{1,no holes} = " + Math.Round(controlPerimeterNoHoles.Length,2) + "mm",
                    "u_1 = " + Math.Round(u1,2) + "mm",
                },
            });

            // calc Beta
            Formula betaFormula = new Formula() { Narrative = "Calculate Beta factor." + Environment.NewLine, Ref = "cl 6.4.3" };
            switch (_colType.ValueAsString)
            {
                case "INTERNAL":
                    double ey = _my.Value * 1E6 / (_punchingLoad.Value * 1E3);
                    double ez = _mz.Value * 1E6 / (_punchingLoad.Value * 1E3);
                    double by = (controlPerimeterNoHoles.Segments.Max(a => a.Start.X) - controlPerimeterNoHoles.Segments.Min(a => a.Start.X));
                    double bz = (controlPerimeterNoHoles.Segments.Max(a => a.Start.Y) - controlPerimeterNoHoles.Segments.Min(a => a.Start.Y));
                    double term1 = Math.Pow(ey / bz, 2);
                    double term2 = Math.Pow(ez / by, 2);
                    _beta.Value = 1 + 1.8 * Math.Sqrt(term1 + term2);
                    betaFormula.Narrative += "Calculated based on a rectangular internal column with loading eccentric to both axes. Control perimeter dimensions as Fig 6.13.";
                    betaFormula.Expression.Add(_beta.Symbol + @"=1 + 1.8\sqrt{\left(\frac{e_y}{b_z}\right)^2+\left(\frac{e_z}{b_y}\right)^2} =" + Math.Round(_beta.Value, 3));
                    betaFormula.Expression.Add(@"e_y =\frac{" + _my.Symbol + @"}{" + _punchingLoad.Symbol + "}=" + Math.Round(ey, 1) + "mm");
                    betaFormula.Expression.Add(@"e_z =\frac{" + _mz.Symbol + @"}{" + _punchingLoad.Symbol + "}=" + Math.Round(ez, 1) + "mm");
                    betaFormula.Expression.Add(@"b_y =" + Math.Round(by, 1) + "mm");
                    betaFormula.Expression.Add(@"b_z =" + Math.Round(bz, 1) + "mm");
                    Uri uri = new Uri("pack://application:,,,/TestCalcs;component/resources/ControlPerimeters_Fig_6_13.png");
                    betaFormula.Image = new BitmapImage(uri);
                    break;
                case "EDGE":
                    double epar = _my.Value * 1E6 / (_punchingLoad.Value * 1E3);
                    double c1 = _columnAdim.Value;
                    double c2 = _columnBdim.Value;
                    double k = calck(c1 / c2);
                    double w1 = Math.Pow(c2, 2) / 4 + c1 * c2 + 4 * c1 * d_average + 8 * Math.Pow(d_average, 2) + Math.PI * d_average * c2;
                    var u1 = controlPerimeterNoHoles.Length;
                    var u1red = u1reducedNoHoles.Sum(a => a.Length);
                    _beta.Value = (u1 / u1red) + k * (u1 / w1) * epar;
                    betaFormula.Narrative += "Calculated on the basis of eccentricities about both axes, but moment about the axis parallel to slab edge is towards the interior of hte slab.";
                    betaFormula.Expression.Add(_beta.Symbol + @"=\frac{u_1}{u_{1^*}}+k\frac{u_1}{W_1}e_{par}=" + Math.Round(_beta.Value, 3));
                    betaFormula.Expression.Add(@"u_1=" + Math.Round(u1, 2) + "mm");
                    betaFormula.Expression.Add(@"u_{1^*}=" + Math.Round(u1red, 2) + "mm");
                    betaFormula.Expression.Add(@"k=" + Math.Round(k, 2));
                    betaFormula.Expression.Add(@"e_{par} =\frac{" + _my.Symbol + @"}{" + _punchingLoad.Symbol + "}=" + Math.Round(epar, 1) + "mm");
                    betaFormula.Expression.Add(@"W_1=\frac{c_2^2}{4}+c_1c_2+4c_1d+8d^2+\pi dc_2=" + Math.Round(w1, 2));
                    Uri uri2 = new Uri("pack://application:,,,/TestCalcs;component/resources/PunchingShear_Fig_6_20.png");
                    betaFormula.Image = new BitmapImage(uri2);
                    break;
                case "CORNER":
                    u1 = controlPerimeterNoHoles.Length;
                    u1red = u1reducedNoHoles.Sum(a => a.Length);
                    _beta.Value = u1 / u1red;
                    betaFormula.Expression.Add(_beta.Symbol + @"=\frac{u_1}{u_{1^*}}=" + Math.Round(_beta.Value, 3));
                    betaFormula.Expression.Add(@"u_1=" + Math.Round(u1, 2) + "mm");
                    betaFormula.Expression.Add(@"u_{1^*}=" + Math.Round(u1red, 2) + "mm");
                    uri2 = new Uri("pack://application:,,,/TestCalcs;component/resources/PunchingShear_Fig_6_20.png");
                    betaFormula.Image = new BitmapImage(uri2);
                    break;
                default:
                    break;
            }
            expressions.Add(betaFormula);

            // calculate shear stress at face
            _stressvEdi.Value = _beta.Value * _punchingLoad.Value * 1000 / (ui * d_average);
            var colFaceStressFormula = new Formula()
            {
                Narrative = "Check shear stress at column face.",
                Expression = new List<string>
                {
                    string.Format(@"u_0 = {0} mm", Math.Round(ui, 2)),
                    _beta.Symbol + " = " + Math.Round(_beta.Value,3) + _beta.Unit,
                    String.Format(@"{0} = {1}\frac{{{2}}}{{u_0 d_{{eff}}}} = {3}{4}",
                        _stressvEdi.Symbol,
                        _beta.Symbol,
                        _punchingLoad.Symbol,
                        Math.Round(_stressvEdi.Value, 2),
                        @"N/{mm^2}"),
                },
                Ref = "cl.6.4.5(3)"
            };

            vRdMax = shearStressResistance(_fck.Value);

            if (_stressvEdi.Value > vRdMax)
            {
                _stressvEdi.Status = CalcStatus.FAIL;
                _uoutef.Value = double.NaN;
                _Asw.Value = double.NaN;
                colFaceStressFormula.Status = CalcStatus.FAIL;
                colFaceStressFormula.Conclusion = "Too high";
                expressions.Add(colFaceStressFormula);
                var image2 = generateImage();
                expressions.Insert(0, new Formula
                {
                    Narrative = "Diagram:" + Environment.NewLine +
                    "Column shown in green, control and outer perimeters in red.",
                    Image = image2,
                    Conclusion = "Face stress too high",
                    Status = CalcStatus.FAIL
                });
                return;
            }
            else
            {
                colFaceStressFormula.Status = CalcStatus.PASS;
                colFaceStressFormula.Conclusion = "OK";
                _stressvEdi.Status = CalcStatus.PASS;
                colFaceStressFormula.Expression.Add(@"v_{Rd,max}=" + Math.Round(vRdMax, 2) + @"N/mm^2");
                colFaceStressFormula.Expression.Add(string.Format(@"{0} \leq {1}", _stressvEdi.Symbol, @"v_{Rd,max}"));
                expressions.Add(colFaceStressFormula);
            }

            reinforcementRatioy = _rebarY.Value / (1000 * dy);
            reinforcementRatioz = _rebarZ.Value / (1000 * dz);

            _rho.Value = Math.Pow(reinforcementRatioy * reinforcementRatioz, 0.5);

            vRdc = shearResistanceNoRein(_rho.Value, d_average, _fck.Value, _concPartFactor.Value); expressions.Add(new Formula
            {
                Expression = new List<string>
                    {
                    @"v_{Rd,c}=\max(C_{Rd,c}k(100\rho_lf_{ck})^{1/3}+k_1\sigma_{cp},v_{min}+k_1\sigma_{cp})",
                    @"v_{Rd,c}="+Math.Round(vRdc,2)+@"N/mm^2",
                    @"\text{where}",
                    @"C_{Rd,c}=\frac{0.18}{" + _concPartFactor.Symbol + @"}=" + Math.Round(0.18/_concPartFactor.Value,2),
                    @"k=1+\sqrt{\frac{200}{d}}\leq2",
                    _rho.Symbol + @"=\sqrt{\rho_{ly} \rho_{lz}}\leq0.02",
                    _rho.Symbol + "=" + Math.Round(_rho.Value,5) + _rho.Unit
                    },
                Narrative = "Calculate punching shear resistance",
                Ref = "cl. 6.4.4(1)"
            });

            stressvEd1 = _beta.Value * _punchingLoad.Value * 1000 / (u1 * d_average);
            expressions.Add(new Formula
            {
                Narrative = "Calculate shear stress at first control perimeter",
                Expression = new List<string>
                {
                    "u_1 = " + Math.Round(u1,2) + "mm",
                    @"v_{Ed1} = \frac{" + _beta.Symbol + " " + _punchingLoad.Symbol + @"}{u_1 d_{average}} = " + Math.Round(stressvEd1,2) + @"N/{mm^2}"
                },
            });

            if (stressvEd1 <= vRdc)
            {
                _uoutef.Value = 0;
                _Asw.Value = 0;
                _numberOfPerimeters.Value = 0;
                _linksInFirstPerim.Value = 0;
                _legsTotal.Value = 0;
                expressions.Add(new Formula
                {
                    Expression = new List<string>
                    {
                        string.Format(@"{0} \leq {1}", @"v_{Ed,1}", @"v_{Rd,c}"),
                    },
                    Conclusion = "No shear links required",
                    Status = CalcStatus.PASS,
                    Narrative = "Check if shear links are required"
                });
                var image2 = generateImage();
                expressions.Insert(0, new Formula
                {
                    Narrative = "Diagram:" + Environment.NewLine +
                    "Column shown in green, control and outer perimeters in red.",
                    Image = image2,
                    Conclusion = "No links required",
                    Status = CalcStatus.PASS
                });
                return;
            }
            else if (stressvEd1 > 2 * vRdc)
            {
                _uoutef.Value = double.NaN;
                _Asw.Value = double.NaN;
                _numberOfPerimeters.Value = double.NaN;
                _linksInFirstPerim.Value = double.NaN;
                _legsTotal.Value = double.NaN;

                expressions.Add(new Formula
                {
                    Narrative = "Check if shear stress at control perimter is less than limiting value",
                    Expression = new List<string>
                    {
                        string.Format(@"{0} > 2 \times {1}", @"v_{Ed,1}", @"v_{Rd,c}"),
                    },
                    Conclusion = "Redesign slab",
                    Status = CalcStatus.FAIL
                });
                var image2 = generateImage();
                expressions.Insert(0, new Formula
                {
                    Narrative = "Diagram:" + Environment.NewLine +
                    "Column shown in green, control and outer perimeters in red.",
                    Image = image2,
                    Conclusion = "Redesign slab",
                    Status = CalcStatus.FAIL
                });
                return;
            }
            else
            {
                expressions.Add(new Formula
                {
                    Expression = new List<string>
                    {
                        string.Format(@"{0} > {1}", @"v_{Ed,1}", @"v_{Rd,c}"),
                        string.Format(@"{0} \leq 2 \times {1}", @"v_{Ed,1}", @"v_{Rd,c}"),
                    },
                    Conclusion = "Links required",
                    Narrative = "Check if shear links are required and stress is less than limit"
                });
            }

            switch (_srMode.ValueAsString)
            {
                case "AUTO":
                    sr = 0.75 * d_average;
                    break;
                case "TARGET":
                    sr = _srTarget.Value;
                    break;
                default:
                    break;
            }
            if (sr <= 0.75 * d_average)
            {
                expressions.Add(new Formula
                {
                    Narrative = "Maximum radial spacing",
                    Expression = new List<string>
                            {
                                @"s_r = " + Math.Round(sr,0) + "mm",
                                @"s_r \leq 0.75 \times d",
                                @"s_r \leq 0.75 \times " + Math.Round(d_average,0) + "mm"
                            },
                    Status = CalcStatus.PASS
                });
            }
            else
            {
                expressions.Add(new Formula
                {
                    Narrative = "Maximum radial spacing",
                    Expression = new List<string>
                            {
                                @"s_r = " + Math.Round(sr,0) + "mm",
                                @"s_r > 0.75 \times d",
                                @"s_r > 0.75 \times " + Math.Round(d_average,0) + "mm"
                            },
                    Status = CalcStatus.FAIL
                });
                expressions.Insert(0, new Formula
                {
                    Narrative = "Diagram:" + Environment.NewLine +
                    "Column shown in green, control and outer perimeters in red.",
                    Image = generateImage(),
                    Conclusion = "Radial spacing too high",
                    Status = CalcStatus.FAIL
                });
                return;
            }

            fywd = 338; // need to calc this in future
            fywdef = Math.Min(250 + 0.25 * d_average, fywd);
            _Asw.Value = (stressvEd1 - 0.75 * vRdc) * sr * u1 / (1.5 * fywdef);
            expressions.Add(new Formula
            {
                Narrative = "Area of shear reinforcement required, assuming shear links are perpendicular to slab",
                Expression = new List<string>
                {
                    @"f_{ywdef} = \min{(250 + 0.25 \times d_{average},f_{ywd})} = \min{(" + (250 + 0.25 * d_average) + @"," + fywd+@")} = " + fywdef + @"N/mm^2",
                    _Asw.Symbol + @" = \frac{(v_{Ed1} - 0.75)v_{Rdc}s_ru_1}{1.5 \times f_{ywdef}} =" + Math.Round(_Asw.Value,0) + _Asw.Unit,
                    @"\frac{" + _Asw.Symbol + @"}{s_r}=" + Math.Round(_Asw.Value / sr,3)
                },
                Ref = "cl.6.4.5(1)"
            });

            _uoutef.Value = _beta.Value * 1000 * _punchingLoad.Value / (vRdc * d_average);

            _distToFirstLinkPerim.Value = 0.5 * d_average;
            _perimSpacing.Value = sr;
            allPerimeters = new List<List<PolyLine>>();
            allPerimeters.Add(generatePerimeterWithHoles(_distToFirstLinkPerim.Value));
            shearLinkStartPerimeters = allPerimeters[0];
            for (int i = 1; i < 25; i++) // arbitrary max number of perimeters to check to. Change to while loop.
            {
                var perim = generatePerimeterWithHoles(_distToFirstLinkPerim.Value + i * _perimSpacing.Value);
                allPerimeters.Add(perim);
                if (perim.Sum(a => a.Length) > _uoutef.Value) break;
            }

            outerPerimeters = allPerimeters.Last();
            int numberofperimeters = Math.Max(allPerimeters.Count - 2, 1);
            perimetersToReinforce = allPerimeters.GetRange(0, numberofperimeters);
            shearLinkEndPerimeters = perimetersToReinforce.Last();

            double perimeterAtLastLinks = shearLinkEndPerimeters.Sum(a => a.Length);

            //spurs
            shearSpurs = new List<Line>();
            shearLinks = new List<List<Vector2>>();
            var shearLinkStartSpurs = new List<PolyLine>();
            for (int i = 0; i < perimetersToReinforce.Count; i++)
            {
                shearLinkStartSpurs.Add(generatePerimeter(_columnAdim.Value, _columnBdim.Value, 0.5 * d_average + i * (sr)));
            }

            if (_linkArrangement.ValueAsString == "SPURS_AUTO")
            {
                perimetersToReinforce = new List<List<PolyLine>> { perimetersToReinforce[0] };
                var spurPoints = new List<List<Tuple<Vector2, Vector2>>> { spurStartPoints() };
                double distFromCol = 0.5 * d_average;
                if (_colType.ValueAsString == "INTERNAL") spurPoints = new List<List<Tuple<Vector2, Vector2>>> { spurStartPoints2() };

                for (int i = 1; i < 25; i++)
                {
                    distFromCol += sr;
                    int spursOnPrevPerim = spurPoints[i - 1].Count;
                    List<Tuple<Vector2, Vector2>> newPerim = new List<Tuple<Vector2, Vector2>>();
                    for (int k = 0; k < spursOnPrevPerim; k++)
                    {
                        var vector = spurPoints[i - 1][k].Item2;
                        newPerim.Add(new Tuple<Vector2, Vector2>(spurPoints[i - 1][k].Item1 + vector * (float)sr, vector));
                    }
                    var segs = new List<GeometryBase>();
                    var testOuter = new List<GeometryBase>();
                    var prevPoint = newPerim.Last();
                    int start = 0;
                    if (_colType.ValueAsString != "INTERNAL")
                    {
                        prevPoint = newPerim.First();
                        start = 1;
                    }

                    for (int j = start; j < newPerim.Count; j++)
                    {
                        var item = newPerim[j];
                        if (
                           ((distFromCol < 2 * d_average) && ((item.Item1 - prevPoint.Item1).Length() > 1.5 * d_average))
                           ||
                           ((distFromCol >= 2 * d_average) && ((item.Item1 - prevPoint.Item1).Length() > 2 * d_average))
                            )
                        {
                            var interPoint = (prevPoint.Item1 + item.Item1) / 2;
                            var interVec = Vector2.Normalize((prevPoint.Item2 + item.Item2));
                            newPerim.Insert(j, new Tuple<Vector2, Vector2>(interPoint, interVec));
                            segs.Add(new Line(prevPoint.Item1, interPoint));
                            segs.Add(new Line(interPoint, item.Item1));
                        }
                        else
                        {
                            segs.Add(new Line(prevPoint.Item1, item.Item1));
                        }
                        prevPoint = item;
                    }
                    var newPerimLine = new PolyLine(segs);
                    // extend to slab edge to max 0.5 * st
                    if (_colType.ValueAsString == "EDGE" || _colType.ValueAsString == "CORNER")
                    {
                        var v1 = Vector2.Normalize(segs.First().Start - segs.First().End);
                        var ray1 = new Line(segs.First().Start, segs.First().Start + v1 * 10000);
                        var v2 = Vector2.Normalize(segs.Last().End - segs.Last().Start);
                        var ray2 = new Line(segs.Last().End, segs.Last().End + v2 * 10000);

                        var inter1 = slabEdge.intersection(ray1);
                        var inter2 = slabEdge.intersection(ray2);
                        if (inter1[0].TypeOfIntersection == IntersectionType.WITHIN)
                        {
                            var pt1 = inter1[0].Point;
                            if ((pt1 - segs.First().Start).Length() < 0.5f * (float)st) segs.Insert(0, new Line(pt1, segs.First().Start));
                            else segs.Insert(0, new Line(segs.First().Start + v1 * 0.5f * (float)st, segs.First().Start));
                        }
                        if (inter2[0].TypeOfIntersection == IntersectionType.WITHIN)
                        {
                            var pt2 = inter2[0].Point;
                            if ((pt2 - segs.Last().End).Length() < 0.5f * (float)st) segs.Add(new Line(segs.Last().End, pt2));
                            else segs.Add(new Line(segs.Last().End, segs.Last().End + v2 * 0.5f * (float)st));
                        }
                    }
                    var newPerimWithHoles = generatePerimeterWithHoles(newPerimLine);
                    perimetersToReinforce.Add(newPerimWithHoles);
                    spurPoints.Add(newPerim);
                    if (newPerimWithHoles.Sum(a => a.Length) > _uoutef.Value)
                    {
                        break;
                    }
                }
                outerPerimeters = perimetersToReinforce.Last();
                int numberOfPerimsToRemove = (int)Math.Ceiling(1.5 * d_average / sr);
                numberOfPerimsToRemove = Math.Min(numberOfPerimsToRemove, spurPoints.Count - 2);
                if (numberOfPerimsToRemove >= 2)
                {
                    spurPoints.RemoveRange(spurPoints.Count - numberOfPerimsToRemove, numberOfPerimsToRemove);
                    perimetersToReinforce.RemoveRange(perimetersToReinforce.Count - numberOfPerimsToRemove, numberOfPerimsToRemove);
                }

                shearLinkEndPerimeters = perimetersToReinforce.Last();
                // remove links in holes
                shearLinks = new List<List<Vector2>>();
                foreach (var item in spurPoints)
                {
                    List<Vector2> filteredList = new List<Vector2>();
                    foreach (var link in item)
                    {
                        bool include = true;
                        foreach (var hole in _holeEdges)
                        {
                            var angle1 = Math.Atan2(hole.Item1.End.Y, hole.Item1.End.X);
                            if (angle1 < 0) angle1 += Math.PI * 2;
                            var angle2 = Math.Atan2(hole.Item2.End.Y, hole.Item2.End.X);
                            if (angle2 < 0) angle2 += Math.PI * 2;
                            var angle3 = Math.Atan2(link.Item1.Y, link.Item1.X);
                            if (angle3 < 0) angle3 += Math.PI * 2;
                            if (Math.Abs(angle1 - angle2) < Math.PI)
                            {
                                if (angle3 > Math.Min(angle1, angle2) && angle3 < Math.Max(angle1, angle2))
                                {
                                    include = false;
                                }
                            }
                            else
                            {
                                if (angle3 < Math.Min(angle1, angle2) || angle3 > Math.Max(angle1, angle2))
                                {
                                    include = false;
                                }
                            }
                        }
                        if (include) filteredList.Add(link.Item1);
                    }
                    shearLinks.Add(filteredList);
                }
            }

            if (_linkArrangement.ValueAsString == "GRID")
            {
                perimetersToReinforce = new List<List<PolyLine>>();
                var initialLines = generateColumnFaces();
                var links = new List<List<Tuple<Vector2, Vector2>>>();
                var perimeterLines = new List<List<Line>>();
                double offsetFromColumn = 0;
                //generateFirstLinkPerimeter
                var newLines = new List<Line>();
                var newLinks = new List<Tuple<Vector2, Vector2>>();
                foreach (var line in initialLines.Segments)
                {
                    Vector2 dir = line.End - line.Start;
                    Vector2 perp = Vector2.Normalize(new Vector2(dir.Y, -dir.X));

                    int numberOfSpursOnEdge = Math.Max(2, (int)Math.Ceiling(line.Length / (st)) + 1);
                    double armOffset = 50;
                    if (line.Length < 200) armOffset = 25;
                    var stepVec = Vector2.Normalize(dir) * (float)((line.Length - 2*armOffset) / (double)(numberOfSpursOnEdge - 1));
                    var startPoint = line.PointAtParameter(armOffset / line.Length);
                    for (int i = 0; i < numberOfSpursOnEdge; i++)
                    {
                        newLinks.Add(new Tuple<Vector2, Vector2>(startPoint + stepVec * i + 0.5f * (float)d_average * perp, perp));
                    }
                    newLines.Add(new Line(line.Start + 0.5f * (float)d_average * perp, line.End + 0.5f * (float)d_average * perp));
                }
                offsetFromColumn += 0.5 * d_average;
                perimetersToReinforce.Add(generatePerimeterWithHoles(offsetFromColumn));
                perimeterLines.Add(newLines);
                links.Add(newLinks);
                var prevLinks = newLinks;

                //generateMorePerimeters
                newLines = new List<Line>();
                for (int i = 1; i < 25; i++)//arbitrary max number of perim
                {
                    newLinks = new List<Tuple<Vector2, Vector2>>();
                    newLines = new List<Line>();
                    offsetFromColumn += sr;
                    foreach (var link in prevLinks)
                    {
                        newLinks.Add(new Tuple<Vector2, Vector2>(link.Item1 + link.Item2 * (float)sr, link.Item2));
                    }
                    //generate perimeterlines
                    foreach (var line in perimeterLines[i - 1])
                    {
                        Vector2 dir = line.End - line.Start;
                        Vector2 perp = Vector2.Normalize(new Vector2(dir.Y, -dir.X));
                        newLines.Add(new Line(line.Start + (float)sr * perp, line.End + (float)sr * perp));
                    }

                    // generate perimeter
                    var perim = generatePerimeterWithHoles(offsetFromColumn);
                    perimetersToReinforce.Add(perim);
                    double outerPerimOffset = offsetFromColumn + 1.5 * d_average;
                    outerPerimeters = generatePerimeterWithHoles(outerPerimOffset);
                    if (outerPerimeters.Sum(a => a.Length) > _uoutef.Value)
                    {
                        links.Add(newLinks);
                        break;
                    }
                    var newCornerLinks = new List<Tuple<Vector2, Vector2>>();
                    //generate new links from previous corners
                    int startLine = 1; int prevLine = 0;
                    if (initialLines.IsClosed == true)
                    {
                        startLine = 0;
                        prevLine = initialLines.Segments.Count - 1;
                    }
                    for (int j = startLine; j < perimeterLines[i - 1].Count; j++)
                    {
                        var line1 = perimeterLines[i - 1][prevLine];
                        var line2 = perimeterLines[i - 1][j];
                        Vector2 dir1 = line1.End - line1.Start;
                        Vector2 perp1 = Vector2.Normalize(new Vector2(dir1.Y, -dir1.X));
                        Vector2 dir2 = line2.End - line2.Start;
                        Vector2 perp2 = Vector2.Normalize(new Vector2(dir2.Y, -dir2.X));
                        var cornerPoint = line1.intersection(line2);
                        newLinks.Add(new Tuple<Vector2, Vector2>(cornerPoint[0].Point + perp1 * (float)sr, perp1));
                        newLinks.Add(new Tuple<Vector2, Vector2>(cornerPoint[0].Point + perp2 * (float)sr, perp2));
                        newCornerLinks.Add(new Tuple<Vector2, Vector2>(cornerPoint[0].Point + perp1 * (float)sr + perp2 * (float)sr, new Vector2(0f, 0f)));
                        prevLine = j;
                    }
                    perimeterLines.Add(newLines);
                    prevLinks = newLinks;
                    var linksToAdd = newLinks.Select(a => a).ToList();
                    linksToAdd.AddRange(newCornerLinks.Select(a => a).ToList());
                    links.Add(linksToAdd);
                }

                shearLinkEndPerimeters = perimetersToReinforce.Last();
                // remove links in holes
                shearLinks = new List<List<Vector2>>();
                foreach (var item in links)
                {
                    List<Vector2> filteredList = new List<Vector2>();
                    foreach (var link in item)
                    {
                        bool include = true;
                        foreach (var hole in _holeEdges)
                        {
                            var angle1 = Math.Atan2(hole.Item1.End.Y, hole.Item1.End.X);
                            if (angle1 < 0) angle1 += Math.PI * 2;
                            var angle2 = Math.Atan2(hole.Item2.End.Y, hole.Item2.End.X);
                            if (angle2 < 0) angle2 += Math.PI * 2;
                            var angle3 = Math.Atan2(link.Item1.Y, link.Item1.X);
                            if (angle3 < 0) angle3 += Math.PI * 2;
                            if (Math.Abs(angle1 - angle2) < Math.PI)
                            {
                                if (angle3 > Math.Min(angle1, angle2) && angle3 < Math.Max(angle1, angle2))
                                {
                                    include = false;
                                }
                            }
                            else
                            {
                                if (angle3 < Math.Min(angle1, angle2) || (angle3 > Math.Max(angle1, angle2)))
                                {
                                    include = false;
                                }
                            }
                        }
                        if (include) filteredList.Add(link.Item1);
                    }
                    shearLinks.Add(filteredList);
                }
            }

            if (_linkArrangement.ValueAsString == "CRUCIFORM")
            {
                var cruciformGroups = new List<List<Tuple<Vector2, Vector2>>>();
                perimetersToReinforce = new List<List<PolyLine>>();
                var initialLines = generateColumnFaces();
                var links = new List<List<Tuple<Vector2, Vector2>>>();
                var perimeterLines = new List<List<Line>>();
                double offsetFromColumn = 0;
                //generateFirstLinkPerimeter
                var newLines = new List<Line>();
                foreach (var line in initialLines.Segments)
                {
                    var newLinks = new List<Tuple<Vector2, Vector2>>();
                    Vector2 dir = line.End - line.Start;
                    Vector2 perp = Vector2.Normalize(new Vector2(dir.Y, -dir.X));
                    int numberOfSpursOnEdge = Math.Max(2, (int)Math.Ceiling(line.Length / (st)) + 1);
                    double armOffset = 50;
                    if (line.Length < 200) armOffset = 25;
                    var stepVec = Vector2.Normalize(dir) * (float)((line.Length - 2 * armOffset) / (double)(numberOfSpursOnEdge - 1));
                    var startPoint = line.PointAtParameter(armOffset / line.Length);
                    //var stepVec = Vector2.Normalize(dir) * (float)(line.Length / (double)(numberOfSpursOnEdge - 1));
                    for (int i = 0; i < numberOfSpursOnEdge; i++)
                    {
                        newLinks.Add(new Tuple<Vector2, Vector2>(startPoint + stepVec * i + 0.5f * (float)d_average * perp, perp));
                    }
                    cruciformGroups.Add(newLinks);
                    newLines.Add(new Line(line.Start + 0.5f * (float)d_average * perp, line.End + 0.5f * (float)d_average * perp));
                }
                offsetFromColumn += 0.5 * d_average;
                perimetersToReinforce.Add(generatePerimeterWithHoles(offsetFromColumn));
                links.Add(cruciformGroups.SelectMany(i => i).ToList());

                for (int i = 0; i < 25; i++)
                {
                    var prevCruciformGroups = cruciformGroups;
                    cruciformGroups = new List<List<Tuple<Vector2, Vector2>>>();
                    foreach (var edgeLinks in prevCruciformGroups)
                    {
                        var newLinks = new List<Tuple<Vector2, Vector2>>();
                        foreach (var link in edgeLinks)
                        {
                            newLinks.Add(new Tuple<Vector2, Vector2>(link.Item1 + (float)sr * link.Item2, link.Item2));
                        }
                        cruciformGroups.Add(newLinks);
                    }

                    links.Add(cruciformGroups.SelectMany(a => a).ToList());
                    perimetersToReinforce.Add(generatePerimeterWithHoles(offsetFromColumn));
                    if (Math.Sqrt(2 * Math.Pow(offsetFromColumn, 2)) > 2 * d_average) break;
                    offsetFromColumn += sr;
                }

                bool closed = false;
                if (_colType.ValueAsString == "INTERNAL") closed = true;

                var outerPerim = new List<PolyLine>();
                for (int j = 0; j < cruciformGroups.Count; j++)
                {
                    var outPerimSegment = new List<GeometryBase>();
                    var edgeLinks = cruciformGroups[j];
                    var pt1 = edgeLinks.First().Item1 + edgeLinks.First().Item2 * 1.5f * (float)d_average;
                    var pt2 = edgeLinks.Last().Item1 + edgeLinks.Last().Item2 * 1.5f * (float)d_average;
                    var angle = Math.Atan2(edgeLinks.Last().Item2.Y, edgeLinks.Last().Item2.X);
                    if (angle < 0) angle += 2d * Math.PI;
                    if (j != 0 || closed)
                    {
                        var angle2 = angle;
                        if (angle - Math.PI * 0.75 < 0)
                        {
                            angle2 += 2 * Math.PI;
                        }
                        var arc = new Arc { Centre = edgeLinks.First().Item1, Radius = 1.5 * d_average, StartAngle = angle2 - Math.PI / 4, EndAngle = angle2 };
                        var startVec = Vector2.Normalize(new Vector2((float)Math.Cos(angle2 - Math.PI * 0.75), (float)Math.Sin(angle2 - Math.PI * 0.75)));
                        var pt3 = arc.Start + startVec * (float)d_average;
                        outPerimSegment.Add(new Line(pt3, arc.Start));
                        outPerimSegment.Add(arc);
                    }
                    outPerimSegment.Add(new Line(pt1, pt2));
                    if (j < cruciformGroups.Count - 1 || closed)
                    {
                        var arc = new Arc { Centre = edgeLinks.Last().Item1, Radius = 1.5 * d_average, StartAngle = angle, EndAngle = angle + Math.PI / 4 };
                        var endVec = Vector2.Normalize(new Vector2((float)Math.Cos(angle + Math.PI * 0.75), (float)Math.Sin(angle + Math.PI * 0.75)));
                        var pt4 = arc.End + endVec * (float)d_average;
                        outPerimSegment.Add(arc);
                        outPerimSegment.Add(new Line(arc.End, pt4));
                    }
                    outerPerim.Add(new PolyLine(outPerimSegment));
                }

                outerPerimeters = new List<PolyLine>();
                foreach (var perim in outerPerim)
                {
                    outerPerimeters.AddRange(generatePerimeterWithHoles(perim));
                }

                // remove links in holes
                shearLinks = new List<List<Vector2>>();
                foreach (var item in links)
                {
                    List<Vector2> filteredList = new List<Vector2>();
                    foreach (var link in item)
                    {
                        bool include = true;
                        foreach (var hole in _holeEdges)
                        {
                            var angle1 = Math.Atan2(hole.Item1.End.Y, hole.Item1.End.X);
                            if (angle1 < 0) angle1 += Math.PI * 2;
                            var angle2 = Math.Atan2(hole.Item2.End.Y, hole.Item2.End.X);
                            if (angle2 < 0) angle2 += Math.PI * 2;
                            var angle3 = Math.Atan2(link.Item1.Y, link.Item1.X);
                            if (angle3 < 0) angle3 += Math.PI * 2;
                            if (Math.Abs(angle1 - angle2) < Math.PI)
                            {
                                if (angle3 > Math.Min(angle1, angle2) && angle3 < Math.Max(angle1, angle2))
                                {
                                    include = false;
                                }
                            }
                            else
                            {
                                if (angle3 < Math.Min(angle1, angle2) || (angle3 > Math.Max(angle1, angle2)))
                                {
                                    include = false;
                                }
                            }
                        }
                        if (include) filteredList.Add(link.Item1);
                    }
                    shearLinks.Add(filteredList);
                }
            }

            _linksInFirstPerim.Value = shearLinks[0].Count;
            _numberOfPerimeters.Value = perimetersToReinforce.Count;
            _legsTotal.Value = shearLinks.Sum(a => a.Count);
            _legDia.Value = calcBarSizeAndDia(_Asw.Value / _linksInFirstPerim.Value, new List<int> { 8, 10, 12, 16 });

            var outerPerimExp = new Formula
            {
                Narrative = "Outer perimeter",
                Expression = new List<string>
                {
                    _uoutef.Symbol + "=" + Math.Round(_uoutef.Value,0) + _uoutef.Unit,
                    @"u_{out,ef,prov} = " + Math.Round(outerPerimeters.Sum(a => a.Length),0) + @"mm"
                },
                Ref = "cl.6.4.5(4)"
            };

            if (_uoutef.Value > outerPerimeters.Sum(a => a.Length))
            {
                outerPerimExp.Conclusion = "Outer perimeter too short";
                outerPerimExp.Status = CalcStatus.FAIL;
                expressions.Add(outerPerimExp);

                expressions.Insert(0, new Formula
                {
                    Narrative = "Diagram:" + Environment.NewLine + "Column shown in green, control and outer perimeters in red.",
                    Image = generateImage(),
                    Conclusion = "Outer perimeter too short",
                    Status = CalcStatus.FAIL
                });
                return;
            }
            else
            {
                expressions.Add(outerPerimExp);
            }


            var detailingFormula = new Formula
            {
                Narrative = "Detailing dimensions:",
                Expression = new List<string>
                {
                    @"\text{Distance to first perimeter} =" + Math.Round(_distToFirstLinkPerim.Value,0) + _distToFirstLinkPerim.Unit,
                    @"\text{Perimeter spacing} =" + Math.Round(_perimSpacing.Value,0) + _perimSpacing.Unit
                }
            };
            if (_linkArrangement.ValueAsString == "GRID")
            {
                Uri uri = new Uri("pack://application:,,,/TestCalcs;component/resources/PunchingShear_ConcreteCentreLayout.png");
                detailingFormula.Image = new BitmapImage(uri);
                detailingFormula.Ref = "Concrete Centre Figure 9";
            }
            else if (_linkArrangement.ValueAsString == "SPURS_AUTO")
            {
                Uri uri = new Uri("pack://application:,,,/TestCalcs;component/resources/PunchingShear_Fig_6_22.png");
                detailingFormula.Image = new BitmapImage(uri);
                detailingFormula.Ref = "Figure 6.22, diagram A";
            }
            else if (_linkArrangement.ValueAsString == "CRUCIFORM")
            {
                Uri uri = new Uri("pack://application:,,,/TestCalcs;component/resources/PunchingShear_Fig_6_22.png");
                detailingFormula.Image = new BitmapImage(uri);
                detailingFormula.Ref = "Figure 6.22, diagram B";
            }

            expressions.Add(detailingFormula);

            var image = generateImage();
            expressions.Insert(0, new Formula
            {
                Narrative = "Diagram:" + Environment.NewLine +
                "Column shown in green, control and outer perimeters in red.",
                Image = image,
                Conclusion = "OK",
                Status = CalcStatus.PASS
            });

            if(_dxfOpt.ValueAsString == "SPECIFIED_FOLDER")
            {
                generateDXFoutput();
            }

        }

        private void generateDXFoutput()
        {
            var dxf = new netDxf.DxfDocument();
            var ColumnLayer = new netDxf.Tables.Layer("Column")
            {
                Color = netDxf.AciColor.Green,
                Lineweight = netDxf.Lineweight.W50
            };
            var slabEdgeLayer = new netDxf.Tables.Layer("SlabEdge")
            {
                Color = netDxf.AciColor.Yellow,
                Lineweight = netDxf.Lineweight.W35
            };
            var linksLayer = new netDxf.Tables.Layer("Links")
            {
                Color = netDxf.AciColor.Green,
                Lineweight = netDxf.Lineweight.W35
            };
            var controlPerimeterLayer = new netDxf.Tables.Layer("ControlPerimeter")
            {
                Color = netDxf.AciColor.Red,
                Lineweight = netDxf.Lineweight.W18
            };
            var outerPerimeterLayer = new netDxf.Tables.Layer("OuterPerimeter")
            {
                Color = netDxf.AciColor.Red,
                Lineweight = netDxf.Lineweight.W18
            };

            dxf.Layers.Add(ColumnLayer);
            dxf.Layers.Add(slabEdgeLayer);
            dxf.Layers.Add(linksLayer);
            dxf.Layers.Add(controlPerimeterLayer);
            dxf.Layers.Add(outerPerimeterLayer);

            foreach (var item in fullColumnOutline.Segments)
            {
                dxf.AddEntity(getDXFEntityFromGeometry(item, ColumnLayer));                
            }
            foreach (var item in slabEdge.Segments)
            {
                dxf.AddEntity(getDXFEntityFromGeometry(item, slabEdgeLayer));
            }
            foreach (var item in shearLinks.SelectMany(a => a).ToList())
            {
                dxf.AddEntity(new netDxf.Entities.Circle(new netDxf.Vector2(item.X, item.Y), 10) { Layer = linksLayer });
            }
            foreach (var item in controlPerimeters.SelectMany(a => a.Segments))
            {
                dxf.AddEntity(getDXFEntityFromGeometry(item, controlPerimeterLayer));
            }
            foreach (var item in outerPerimeters.SelectMany(a => a.Segments))
            {
                dxf.AddEntity(getDXFEntityFromGeometry(item, outerPerimeterLayer));
            }

            var filePath = _dxfFolder.ValueAsString + @"\" + this.InstanceName + @".dxf";
            dxf.Save(filePath);
        }

        private netDxf.Entities.EntityObject getDXFEntityFromGeometry(GeometryBase item, Layer layer)
        {
            if (item.GetType() == typeof(Line))
            {
                var dxfLine = new netDxf.Entities.Line(
                    new netDxf.Vector2(item.Start.X, item.Start.Y),
                    new netDxf.Vector2(item.End.X, item.End.Y))
                { Layer = layer };
                return dxfLine;
            }
            else if (item.GetType() == typeof(Arc))
            {
                var arc = item as Arc;
                var dxfLine = new netDxf.Entities.Arc(
                    new netDxf.Vector2(arc.Centre.X, arc.Centre.Y),
                    arc.Radius,
                    arc.StartAngle * (180 / Math.PI),
                    arc.EndAngle * (180 / Math.PI))
                { Layer = layer };
                return dxfLine;
            }
            return null;
        }

        private List<Tuple<Vector2, Vector2>> spurStartPoints2()
        {
            var returnList = new List<Tuple<Vector2, Vector2>>();

            var firstPerim = generatePerimeter(_columnAdim.Value, _columnBdim.Value, 0.5 * d_average);
            var controlPerim = generatePerimeter(_columnAdim.Value, _columnBdim.Value, 2 * d_average);
            int spurSegments = (int)Math.Ceiling(controlPerim.Length / (st));
            if (firstPerim.IsClosed)
            {
                spurSegments++;
            }
            for (int i = 0; i < spurSegments; i++)
            {
                double frac = (1d / (double)spurSegments)*(i);
                var pt1 = firstPerim.PointAtParameter((frac));
                var pt2 = controlPerim.PointAtParameter((frac));
                var vec = Vector2.Normalize(pt2 - pt1);
                returnList.Add(new Tuple<Vector2, Vector2>(pt1, vec));
            }
            return returnList;
        }

        private double calck(double c1overc2)
        {
            if (c1overc2 <= 0.5) return 0.45;
            else if (c1overc2 < 1) return (c1overc2 - 0.5) * (0.15 / 0.5) + 0.5;
            else if (c1overc2 < 3) return (c1overc2 - 1) * (0.2 / 2) + 1;
            else return 0.8;
        }

        private void generateHoles()
        {
            // SET UP GEOMETRY
            // holes
            var holeExpression = new Formula()
            {
                Narrative = "Effective width of opening with respect to centre of loading",
                Ref = "Figure 6.14",
            };
            allHoleCorners = new List<List<Vector2>>();
            if (_holeSizeX.Value != 0 && _holeSizeY.Value != 0)
            {
                allHoleCorners.Add(new List<Vector2> {
            new Vector2((float)_holePosX.Value, (float)(_holePosY.Value + _holeSizeY.Value)),
            new Vector2((float)(_holePosX.Value + _holeSizeX.Value), (float)(_holePosY.Value + _holeSizeY.Value)),
            new Vector2((float)(_holePosX.Value + _holeSizeX.Value), (float)(_holePosY.Value)),
            new Vector2((float)_holePosX.Value, (float)(_holePosY.Value))
            });
            }
            if (_hole2SizeX.Value != 0 && _hole2SizeY.Value != 0)
            {
                allHoleCorners.Add(new List<Vector2> {
            new Vector2((float)_hole2PosX.Value, (float)(_hole2PosY.Value + _hole2SizeY.Value)),
            new Vector2((float)(_hole2PosX.Value + _hole2SizeX.Value), (float)(_hole2PosY.Value + _hole2SizeY.Value)),
            new Vector2((float)(_hole2PosX.Value + _hole2SizeX.Value), (float)(_hole2PosY.Value)),
            new Vector2((float)_hole2PosX.Value, (float)(_hole2PosY.Value)),
            });
            }

            _holeEdges = new List<Tuple<Line, Line>>();
            for (int i = 0; i < allHoleCorners.Count; i++)
            {
                var holeCorners = allHoleCorners[i];
                double minAngle = Math.Atan2(holeCorners[0].Y, holeCorners[0].X);
                double maxAngle = Math.Atan2(holeCorners[0].Y, holeCorners[0].X);
                foreach (var corner in holeCorners)
                {
                    double angle = Math.Atan2(corner.Y, corner.X);
                    minAngle = Math.Min(minAngle, angle);
                    maxAngle = Math.Max(maxAngle, angle);
                }
                if ((minAngle * maxAngle < 0) && (maxAngle - minAngle > Math.PI))
                {
                    var newMax = minAngle + 2 * Math.PI;
                    minAngle = maxAngle;
                    maxAngle = newMax;
                }
                // translate coordinate system so that width and lengh of hole can be compared using radius as x-axis
                //figure 6.14
                var minx = 0d; var maxx = 0d; var miny = 0d; var maxy = 0d;
                var midAngle = (maxAngle + minAngle) / 2;
                var s = Math.Sin(-midAngle);
                var c = Math.Cos(-midAngle);
                var corn = holeCorners[0];
                var newx = corn.X * c - corn.Y * s;
                var newy = corn.X * s - corn.Y * c;
                minx = newx;
                maxx = newx;
                miny = newy;
                maxy = newy;
                foreach (var corner in holeCorners)
                {
                    s = Math.Sin(-midAngle);
                    c = Math.Cos(-midAngle);
                    newx = corner.X * c - corner.Y * s;
                    newy = corner.X * s - corner.Y * c;
                    minx = Math.Min(minx, newx);
                    maxx = Math.Max(maxx, newx);
                    miny = Math.Min(miny, newy);
                    maxy = Math.Max(maxy, newy);
                }
                maxy = minx * Math.Sin((maxAngle - minAngle) / 2);
                miny = -minx * Math.Sin((maxAngle - minAngle) / 2);
                var rangex = maxx - minx;
                var rangey = maxy - miny;
                if (rangex > rangey)
                {
                    var newrangey = Math.Sqrt(rangey * rangex);
                    holeExpression.Expression.Add(@"width_" + i + "=" + Math.Round(newrangey, 0) + "mm");
                    minAngle = Math.Atan2(-newrangey / 2, minx) + midAngle;
                    maxAngle = Math.Atan2(newrangey / 2, minx) + midAngle;
                    if ((minAngle * maxAngle < 0) && (maxAngle - minAngle > Math.PI))
                    {
                        var newMax = minAngle + 2 * Math.PI;
                        minAngle = maxAngle;
                        maxAngle = newMax;
                    }
                }
                else
                {
                    holeExpression.Expression.Add(@"width_" + i + "=" + Math.Round(rangey, 0) + "mm");

                }
                var holeEdge1 = new Line(new Vector2(0, 0), new Vector2((float)(10000 * Math.Cos(minAngle)), (float)(10000 * Math.Sin(minAngle))));
                var holeEdge2 = new Line(new Vector2(0, 0), new Vector2((float)(10000 * Math.Cos(maxAngle)), (float)(10000 * Math.Sin(maxAngle))));
                _holeEdges.Add(new Tuple<Line, Line>(holeEdge1, holeEdge2));
            }
            if (allHoleCorners.Count > 0)
            {
                Uri uri = new Uri("pack://application:,,,/TestCalcs;component/resources/PunchingShear_Fig_6_14.png");
                holeExpression.Image = new BitmapImage(uri);
                expressions.Add(holeExpression);
            }

            holeOutlines = new List<PolyLine>();
            foreach (var holeCorners in allHoleCorners)
            {
                var holeSegments = new List<GeometryBase>();

                for (int i = 1; i < holeCorners.Count; i++)
                {
                    holeSegments.Add(new Line(holeCorners[i - 1], holeCorners[i]));
                }
                holeOutlines.Add(new PolyLine(holeSegments) { IsClosed = true });
            }
        }

        private List<int> getSpursList()
        {
            List<int> basicNumbers = new List<int> { 8, 12, 16, 24};
            switch (_linkArrangement.ValueAsString)
            {
                case "SPURS_AUTO":
                    basicNumbers = new List<int> { 8, 12, 16, 24 };
                    break;
                case "SPURS_45_DEGREES":
                    basicNumbers = new List<int> { 8 };
                    break;
                case "SPURS_30_DEGREES":
                    basicNumbers = new List<int> { 12 };
                    break;
                default:
                    break;
            }
            switch (_colType.ValueAsString)
            {
                case "INTERNAL":
                    return basicNumbers;
                case "EDGE":
                    return basicNumbers.Select(a => (a / 2) + 1).ToList();
                case "CORNER":
                    return basicNumbers.Select(a => (a / 4) + 1).ToList();
                default:
                    break;
            }
            return basicNumbers;
        }

        private PolyLine generateColumnFaces()
        {
            var x = (float)_columnAdim.Value / 2f; var y = (float)_columnBdim.Value / 2f;
            var returnList = new PolyLine(new List<GeometryBase>()
            {
                new Line(new Vector2(-x,y), new Vector2(-x,-y)),
                new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                new Line(new Vector2(x,-y), new Vector2(x,y)),
                new Line(new Vector2(x,y), new Vector2(-x,y))
            })
            { IsClosed = true };

            switch (_colType.ValueAsString)
            {
                case "INTERNAL":
                    break;
                case "EDGE":
                    returnList = new PolyLine(new List<GeometryBase>()
                    {
                        new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                        new Line(new Vector2(x,-y), new Vector2(x,y)),
                        new Line(new Vector2(x,y), new Vector2(-x,y)),
                    });
                    break;
                case "CORNER":

                    returnList = new PolyLine(new List<GeometryBase>()
                    {
                        new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                        new Line(new Vector2(x,-y), new Vector2(x,y)),
                    });
                    break;
                default:
                    break;
            }
            return returnList;

        }

        private void generateColumnOutlines()
        {
            var x = (float)_columnAdim.Value / 2f; var y = (float)_columnBdim.Value / 2f;
            fullColumnOutline = new PolyLine(new List<GeometryBase>
            {
                new Line(new Vector2(-x,y), new Vector2(-x,-y)),
                new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                new Line(new Vector2(x,-y), new Vector2(x,y)),
            })
            { IsClosed = true };
            columnOutline = new PolyLine(new List<GeometryBase>
            {
                new Line(new Vector2(-x,y), new Vector2(-x,-y)),
                new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                new Line(new Vector2(x,-y), new Vector2(x,y)),
            })
            { IsClosed = true };

            switch (_colType.ValueAsString)
            {
                case "INTERNAL":
                    break;
                case "EDGE":
                    if (1.5 * d_average > _columnAdim.Value)
                    {
                        columnOutline = new PolyLine(new List<GeometryBase>
                        {
                            new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                            new Line(new Vector2(x,-y), new Vector2(x,y)),
                            new Line(new Vector2(x,y), new Vector2(-x,y)),
                        });
                    }
                    else
                    {
                        columnOutline = new PolyLine(new List<GeometryBase>
                        {
                            new Line(new Vector2(x - 1.5f*(float)d_average,-y), new Vector2(x,-y)),
                            new Line(new Vector2(x,-y),new Vector2(x,y)),
                            new Line(new Vector2(x,y),new Vector2(x - 1.5f*(float)d_average,y)),
                        });
                    }
                    break;
                case "CORNER":
                    if (3 * d_average > _columnAdim.Value + _columnBdim.Value)
                    {
                        columnOutline = new PolyLine(new List<GeometryBase>
                        {
                            new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                            new Line(new Vector2(x,-y), new Vector2(x,y)),
                        });
                    }
                    else
                    {
                        columnOutline = new PolyLine(new List<GeometryBase>
                        {
                            new Line(new Vector2(x-(float)Math.Min(1.5*d_average, _columnAdim.Value),-y), new Vector2(x,-y)),
                            new Line(new Vector2(x,-y), new Vector2(x,(float)Math.Min(1.5*d_average, _columnBdim.Value) -y)),
                        });
                    }
                    break;
                default:
                    break;
            }
            columnOutlines = generatePerimeterWithHoles(columnOutline);
        }

        private void generateSlabEdge()
        {
            var x = (float)_columnAdim.Value / 2f;
            var y = (float)_columnBdim.Value / 2f;
            slabEdge = new PolyLine(new List<GeometryBase> { new Line(new Vector2(-x, y), new Vector2(x, y)) });
            switch (_colType.ValueAsString)
            {
                case "INTERNAL":
                    break;
                case "EDGE":
                    slabEdge = new PolyLine(new List<GeometryBase>
                    {
                        new Line(new Vector2(-x, -10000), new Vector2(-x, 10000))
                    });
                    break;
                case "CORNER":
                    slabEdge = new PolyLine(new List<GeometryBase>
                    {
                        new Line(new Vector2(-x, -10000), new Vector2(-x, y)),
                        new Line(new Vector2(-x, y), new Vector2(10000, y))
                    });
                    break;
                default:
                    break;
            }
        }

        static double calcBarSizeAndDia(double area, List<int> barSizes)
        {
            foreach (var item in barSizes)
            {
                if (Math.PI * Math.Pow(item / 2, 2) > area)
                {
                    return item;
                }
            }
            return double.NaN;
        }

        public static double shearResistanceNoRein(double rhoL, double d_eff, double charStrength, double concPartFact)
        {
            double k = Math.Min(1 + Math.Sqrt(200 / d_eff),2);

            double cRdc = 0.18 / concPartFact;

            double resistance1 = cRdc * k * Math.Pow((100d * rhoL * charStrength), 1f / 3f);
           
            double resistance = Math.Max(resistance1,0.035 * Math.Pow(k, 1.5) * Math.Pow(charStrength, 0.5));
            Console.WriteLine("rho" + rhoL + ";str" + charStrength + ";k" + k + ";vmin" + resistance+";vRdc"+resistance1);
            return resistance;
        }

        public static double shearStressResistance(double charStrength)
        {
            List<Tuple<double, double>> charStrengthshearResistance = new List<Tuple<double, double>>
            {
                new Tuple<double, double>(20,3.68),
                new Tuple<double, double>(25,4.5),
                new Tuple<double, double>(28,4.98),
                new Tuple<double, double>(30,5.28),
                new Tuple<double, double>(32,5.58),
                new Tuple<double, double>(35,6.02),
                new Tuple<double, double>(40,6.72),
                new Tuple<double, double>(45,7.38),
                new Tuple<double, double>(50,8)
            };

            double returnDouble = 0;

            foreach (var item in charStrengthshearResistance)
            {
                if (charStrength >= item.Item1)
                {
                    returnDouble = item.Item2;
                }
            }
            return returnDouble;
        }

        private PolyLine generatePerimeter(double width, double depth, double offset)
        {
            float offsetF = (float)offset;
            float x = (float)width / 2f;
            float y = (float)depth / 2f;
            var perimeter = new PolyLine(new List<GeometryBase>
            {
                new Line(new Vector2(0, y + offsetF), new Vector2(-x, y+offsetF)),
                new Arc(){Centre = new Vector2(-x,y), Radius=offsetF, StartAngle=Math.PI/2, EndAngle=Math.PI},
                new Line(new Vector2(-x-offsetF, y), new Vector2(-x-offsetF, -y)),
                new Arc(){Centre = new Vector2(-x,-y), Radius=offsetF, StartAngle=Math.PI, EndAngle=1.5*Math.PI},
                new Line(new Vector2(-x, - y - offsetF), new Vector2(x, - y - offsetF)),
                new Arc(){Centre = new Vector2(x,-y), Radius=offsetF, StartAngle=1.5*Math.PI, EndAngle=2*Math.PI},
                new Line(new Vector2(x + offsetF, - y), new Vector2(x +offsetF, y)),
                new Arc(){Centre = new Vector2(x,y), Radius=offsetF, StartAngle=0, EndAngle=Math.PI/2},
                new Line(new Vector2(x, y + offsetF), new Vector2(0, y + offsetF)),
            });

            List<IntersectionResult> inter2;
            List<IntersectionResult> inter1;
            PolyLine perimeter2 = perimeter;

            switch (_colType.ValueAsString)
            {
                case "INTERNAL":
                    break;
                case "EDGE":
                    inter2 = perimeter.intersection(new Line(new Vector2(-x, -10000), new Vector2(-x, 0)));
                    inter1 = perimeter.intersection(new Line(new Vector2(-x, 0), new Vector2(-x, 10000)));
                    perimeter2 = perimeter.Cut(inter2[0].Parameter, inter1[0].Parameter);
                    break;
                case "CORNER":
                    inter2 = perimeter.intersection(new Line(new Vector2(-x, -10000), new Vector2(-x, y)));
                    inter1 = perimeter.intersection(new Line(new Vector2(x, y), new Vector2(10000, y)));
                    perimeter2 = perimeter.Cut(inter2[0].Parameter, inter1[0].Parameter);
                    break;
                default:
                    break;
            }

            return perimeter2;
        }

        private PolyLine generateReducedControlPerimeter(double width, double depth)
        {
            float offsetF = 2f * (float)d_average;
            float x = (float)width / 2f;
            float y = (float)depth / 2f;
            var perimeter = new PolyLine(new List<GeometryBase>
            {
                new Line(new Vector2(0, y + offsetF), new Vector2(-x, y+offsetF)),
                new Arc(){Centre = new Vector2(-x,y), Radius=offsetF, StartAngle=Math.PI/2, EndAngle=Math.PI},
                new Line(new Vector2(-x-offsetF, y), new Vector2(-x-offsetF, -y)),
                new Arc(){Centre = new Vector2(-x,-y), Radius=offsetF, StartAngle=Math.PI, EndAngle=1.5*Math.PI},
                new Line(new Vector2(-x, - y - offsetF), new Vector2(x, - y - offsetF)),
                new Arc(){Centre = new Vector2(x,-y), Radius=offsetF, StartAngle=1.5*Math.PI, EndAngle=2*Math.PI},
                new Line(new Vector2(x + offsetF, - y), new Vector2(x +offsetF, y)),
                new Arc(){Centre = new Vector2(x,y), Radius=offsetF, StartAngle=0, EndAngle=Math.PI/2},
                new Line(new Vector2(x, y + offsetF), new Vector2(0, y + offsetF)),
            });

            List<IntersectionResult> inter2;
            List<IntersectionResult> inter1;
            PolyLine perimeter2 = perimeter;

            switch (_colType.ValueAsString)
            {
                case "INTERNAL":
                    break;
                case "EDGE":
                    float offx = (float)Math.Min(1.5 * d_average, 0.5 * _columnAdim.Value);
                    inter2 = perimeter.intersection(new Line(new Vector2(x - offx, -10000), new Vector2(x - offx, 0)));
                    inter1 = perimeter.intersection(new Line(new Vector2(x - offx, 0), new Vector2(x - offx, 10000)));
                    perimeter2 = perimeter.Cut(inter2[0].Parameter, inter1[0].Parameter);
                    break;
                case "CORNER":
                    float offx2 = (float)Math.Min(1.5 * d_average, 0.5 * _columnAdim.Value);
                    float offy2 = (float)Math.Min(1.5 * d_average, 0.5 * _columnBdim.Value);
                    inter2 = perimeter.intersection(new Line(new Vector2(x - offx2, -10000), new Vector2(x - offx2, y)));
                    inter1 = perimeter.intersection(new Line(new Vector2(x, -y + offy2), new Vector2(10000, -y + offy2)));
                    perimeter2 = perimeter.Cut(inter2[0].Parameter, inter1[0].Parameter);
                    break;
                default:
                    break;
            }
            return perimeter2;
        }

        private List<Tuple<Vector2,Vector2>> spurStartPoints()
        {
            var returnList = new List<Tuple<Vector2, Vector2>>();
            var innerPoints = new List<Vector2>();
            var x = (float)_columnAdim.Value / 2;
            var y = (float)_columnBdim.Value / 2;
            double startAngle = 0;
            int angleDivisions = 1;
            bool includeLastCorner = false;
            switch (_linkArrangement.ValueAsString)
            {
                case "SPURS_AUTO":
                    angleDivisions = 2;
                    break;
                case "GRID":
                    angleDivisions = 2;
                    break;
                default:
                    break;
            }

            double newx = 0; double newy = 0;
            if (_columnAdim.Value > 3 * d_average)
            {
                newx = x - Math.Min(0.5 * d_average, y);
            }
            else
            {
                newx = 0;
            }
            if (_columnBdim.Value > 3 * d_average)
            {
                newy = y - Math.Min(0.5 * d_average, x);
            }
            else
            {
                newy = 0;
            }

            switch (_colType.ValueAsString)
            {
                case "INTERNAL":
                    innerPoints.Add(new Vector2(-(float)newx, (float)newy));
                    innerPoints.Add(new Vector2(-(float)newx, -(float)newy));
                    innerPoints.Add(new Vector2((float)newx, -(float)newy));
                    innerPoints.Add(new Vector2((float)newx, (float)newy));
                    innerPoints.Add(new Vector2(-(float)newx, (float)newy));
                    startAngle = 1 * Math.PI;
                    includeLastCorner = true;
                    break;
                case "EDGE":
                    innerPoints.Add(new Vector2(-(float)newx, -(float)newy));
                    innerPoints.Add(new Vector2((float)newx, -(float)newy));
                    innerPoints.Add(new Vector2((float)newx, (float)newy));
                    innerPoints.Add(new Vector2(-(float)newx, (float)newy));
                    startAngle = 1.5 * Math.PI;
                    break;
                case "CORNER":
                    innerPoints.Add(new Vector2(-(float)newx, -(float)newy));
                    innerPoints.Add(new Vector2((float)newx, -(float)newy));
                    innerPoints.Add(new Vector2((float)newx, (float)newy));
                    startAngle = 1.5 * Math.PI;
                    break;
                default:
                    break;
            }

            var angle = startAngle;
            for (int j = 1; j < innerPoints.Count; j++)
            {
                var point = innerPoints[j];

                Line line = new Line(innerPoints[j - 1], point);
                var dist = line.Length;
                if (dist > 0.5 * d_average) // really a check that dist>0
                {
                    int divs = (int)Math.Ceiling(dist / (st));
                    double paramFraction = 1d / divs;
                    for (int k = 0; k < divs; k++)
                    {
                        var point2 = line.PointAtParameter(k * paramFraction);
                        var ray = new Line(point2, point2 + new Vector2(10000f * (float)Math.Cos(angle), 10000f * (float)Math.Sin(angle)));
                        var vec = Vector2.Normalize(ray.End - ray.Start);
                        var inter = fullColumnOutline.intersection(ray);
                        var pt = inter[0].Point + vec * 0.5f * (float)d_average;
                        returnList.Add(new Tuple<Vector2, Vector2>(pt, Vector2.Normalize(vec)));
                    }
                }

                int cornerDivs = angleDivisions;
                if (j >= innerPoints.Count - 1 && !includeLastCorner)
                {
                    cornerDivs = 1;
                }
                for (int i = 0; i < cornerDivs; i++)
                {
                    var ray = new Line(point, point + new Vector2(10000f * (float)Math.Cos(angle), 10000f * (float)Math.Sin(angle)));
                    var vec = Vector2.Normalize(ray.End - ray.Start);
                    var inter = fullColumnOutline.intersection(ray);
                    var pt = inter[0].Point + vec * 0.5f * (float)d_average;
                    returnList.Add(new Tuple<Vector2, Vector2>(pt, Vector2.Normalize(vec)));
                    angle += Math.PI / (2d * angleDivisions);
                }

            }
            return returnList;
        }

        private List<PolyLine> generatePerimeterWithHoles(PolyLine perimeter)
        {
            var perimeters = new List<PolyLine>();
            var perimeter2 = perimeter;
            var listOfHoleIntersections = new List<Tuple<IntersectionResult, IntersectionResult>>();
            foreach (var hole in _holeEdges)
            {
                var inter1 = perimeter2.intersection(hole.Item1);
                var inter2 = perimeter2.intersection(hole.Item2);
                listOfHoleIntersections.Add(new Tuple<IntersectionResult, IntersectionResult>(inter1[0], inter2[0]));
            }
            int holeCounter = 0;
            var startStops = new List<Tuple<double, bool>>();

            foreach (var item in listOfHoleIntersections)
            {
                if (item.Item1.TypeOfIntersection == IntersectionType.WITHIN && item.Item2.TypeOfIntersection == IntersectionType.WITHIN)
                {
                    if (item.Item1.Parameter < item.Item2.Parameter)
                    {
                        startStops.Add(new Tuple<double, bool>(item.Item1.Parameter, true));
                        startStops.Add(new Tuple<double, bool>(item.Item2.Parameter, false));
                    }
                    else
                    {
                        startStops.Add(new Tuple<double, bool>(item.Item1.Parameter, true));
                        startStops.Add(new Tuple<double, bool>(item.Item2.Parameter, false));
                        holeCounter++;
                    }

                }
                else if (item.Item1.TypeOfIntersection == IntersectionType.WITHIN)
                {
                    startStops.Add(new Tuple<double, bool>(item.Item1.Parameter, true));
                }
                else if (item.Item2.TypeOfIntersection == IntersectionType.WITHIN)
                {
                    startStops.Add(new Tuple<double, bool>(item.Item2.Parameter, false));
                    holeCounter++;
                }
            }

            var orderedStartStops = startStops.OrderBy(a => a.Item1).ToList();
            var listOfCuts = new List<double>();
            double prevParam = 0;
            foreach (var item in orderedStartStops)
            {
                if (holeCounter < 1 && item.Item2 == true)
                {
                    listOfCuts.Add(prevParam);
                    listOfCuts.Add(item.Item1);
                    prevParam = item.Item1;
                    holeCounter++;
                }
                else if (item.Item2 == true)
                {
                    holeCounter++;
                }
                else if (item.Item2 == false)
                {
                    holeCounter--;
                    prevParam = item.Item1;
                }
            }
            if (holeCounter < 1)
            {
                listOfCuts.Add(prevParam);
                listOfCuts.Add(1);
            }

            for (int i = 0; i < listOfCuts.Count / 2; i++)
            {
                perimeters.Add(perimeter2.Cut(listOfCuts[i * 2], listOfCuts[1 + i * 2]));
            }
            return perimeters;
        }


        private List<PolyLine> generatePerimeterWithHoles(double offset)
        {
            var perimeter2 = generatePerimeter(_columnAdim.Value, _columnBdim.Value, offset);
            return generatePerimeterWithHoles(perimeter2);
        }

        private List<PolyLine> generateReducedControlPerimeterWithHoles()
        {
            var perimeter = generateReducedControlPerimeter(_columnAdim.Value, _columnBdim.Value);
            return generatePerimeterWithHoles(perimeter);
        }

        private PathGeometry generateGeometry(PolyLine polyline)
        {
            var segmentsPath = new PathSegmentCollection();
            foreach (var item in polyline.Segments)
            {
                if (item.GetType() == typeof(Arc))
                {
                    var arc = item as Arc;
                    segmentsPath.Add(new ArcSegment(new Point(arc.End.X, arc.End.Y), new Size(arc.Radius, arc.Radius), arc.EndAngle - arc.StartAngle, false, SweepDirection.Clockwise, true));
                }
                if (item.GetType() == typeof(Line))
                {
                    var line = item as Line;
                    segmentsPath.Add(new LineSegment(new Point(line.End.X, line.End.Y), true));
                }
            }
            var pathFig = new PathFigure(new Point(polyline.Start.X, polyline.Start.Y), segmentsPath, false);
            return new PathGeometry(new List<PathFigure> { pathFig });
        }

        private GeometryGroup generateVoidCross(List<PolyLine> polylines)
        {
            var returnGeometry = new GeometryGroup();
            foreach (var polyline in polylines)
            {
                var seg = polyline.Segments;
                returnGeometry.Children.Add(new LineGeometry(new Point(seg[0].Start.X, seg[0].Start.Y), new Point(seg[2].Start.X, seg[2].Start.Y)));
                returnGeometry.Children.Add(new LineGeometry(new Point(seg[1].Start.X, seg[1].Start.Y), new Point(seg[3].Start.X, seg[3].Start.Y)));
            }
            return returnGeometry;
        }

        private PathGeometry generateGeometry(List<PolyLine> polylines)
        {
            var figPaths = new List<PathFigure>();
            foreach (var polyline in polylines)
            {
                var segmentsPath = new PathSegmentCollection();
                foreach (var item in polyline.Segments)
                {
                    if (item.GetType() == typeof(Arc))
                    {
                        var arc = item as Arc;
                        segmentsPath.Add(new ArcSegment(new Point(arc.End.X, arc.End.Y), new Size(arc.Radius, arc.Radius), arc.EndAngle - arc.StartAngle, false, SweepDirection.Clockwise, true));
                    }
                    if (item.GetType() == typeof(Line))
                    {
                        var line = item as Line;
                        segmentsPath.Add(new LineSegment(new Point(line.End.X, line.End.Y), true));
                    }
                }
                var pathFig = new PathFigure(new Point(polyline.Start.X, polyline.Start.Y), segmentsPath, false);
                figPaths.Add(pathFig);
            }
            return new PathGeometry(figPaths);
        }

        private BitmapSource generateImage()
        {
            var testImage = new RenderTargetBitmap(1000, 1000, 96, 96, PixelFormats.Pbgra32);
            var visual = new DrawingVisual();
            using (var r = visual.RenderOpen())
            {
                var controlPerimeterGeometry = generateGeometry(controlPerimeters);
                var reducedControlPerimeterGeometry = generateGeometry(u1reduced);
                var outerPerimeterGeometry = generateGeometry(outerPerimeters);
                var columnGeometry = generateGeometry(fullColumnOutline);
                var columnLoadedFaceGeometry = generateGeometry(columnOutlines);
                var holeGeometry = generateGeometry(holeOutlines);
                var voidGeometry = generateVoidCross(holeOutlines);
                var spursGeometry = new GeometryGroup();
                var linksGeometry = new GeometryGroup();
                var holeEdgeLinesGeometry = new GeometryGroup();
                var slabGeometry = generateGeometry(slabEdge);
                var allPerimetersFlattened = new List<PolyLine> ();
                foreach (var item in perimetersToReinforce)
                {
                    foreach (var section in item)
                    {
                        allPerimetersFlattened.Add(section);
                    }
                }
                var innerLinksPerimeterGeometry = generateGeometry(allPerimetersFlattened);
                var outerLinksPerimeterGeometry = generateGeometry(shearLinkEndPerimeters);

                foreach (var hole in _holeEdges)
                {
                    var line = hole.Item1;
                    holeEdgeLinesGeometry.Children.Add(new LineGeometry(new Point(line.Start.X, line.Start.Y), new Point(line.End.X, line.End.Y)));
                    line = hole.Item2;
                    holeEdgeLinesGeometry.Children.Add(new LineGeometry(new Point(line.Start.X, line.Start.Y), new Point(line.End.X, line.End.Y)));
                }

                foreach (var spur in shearSpurs)
                {
                    spursGeometry.Children.Add(new LineGeometry(new Point(spur.Start.X, spur.Start.Y), new Point(spur.End.X, spur.End.Y)));
                }
                foreach (var spur in shearLinks)
                {
                    foreach (var link in spur)
                    {
                        linksGeometry.Children.Add(new EllipseGeometry(new Point(link.X, link.Y), 15, 15));
                    }
                }

                GeometryGroup allGeometry = new GeometryGroup();
                allGeometry.Children = new GeometryCollection(new List<Geometry> { controlPerimeterGeometry, outerPerimeterGeometry, columnGeometry, holeGeometry, outerLinksPerimeterGeometry, innerLinksPerimeterGeometry });
                var newBounds = allGeometry.Bounds;
                double maxX = Math.Max(Math.Abs(newBounds.X), Math.Abs(newBounds.BottomRight.X));
                double maxY = Math.Max(Math.Abs(newBounds.Y), Math.Abs(newBounds.BottomRight.Y));

                double scale = 900 / (Math.Max(2*maxX, 2*maxY));
                var scaleTransform = new ScaleTransform(scale, -scale);
                var transX = 50+scale * -newBounds.X;
                var transY = 50+scale * -newBounds.Y;
                var transTransform = new TranslateTransform(transX, transY);
                var transGroup = new TransformGroup();
                transGroup.Children.Add(scaleTransform);
                transGroup.Children.Add(transTransform);

                columnLoadedFaceGeometry.Transform = transGroup;
                controlPerimeterGeometry.Transform = transGroup;
                reducedControlPerimeterGeometry.Transform = transGroup;
                columnGeometry.Transform = transGroup;
                outerPerimeterGeometry.Transform = transGroup;
                holeGeometry.Transform = transGroup;
                spursGeometry.Transform = transGroup;
                linksGeometry.Transform = transGroup;
                slabGeometry.Transform = transGroup;
                innerLinksPerimeterGeometry.Transform = transGroup;
                outerLinksPerimeterGeometry.Transform = transGroup;
                voidGeometry.Transform = transGroup;
                holeEdgeLinesGeometry.Transform = transGroup;

                var dashedLine = new Pen(Brushes.Gray, 2) { DashStyle = new DashStyle(new List<double> { 20, 5, 5, 5 }, 0) };
                var dashedLine2 = new Pen(Brushes.Gray, 2) { DashStyle = new DashStyle(new List<double> { 10, 5 }, 0) };

                r.DrawGeometry(Brushes.LightGreen, new Pen(Brushes.Green, 5), columnGeometry);
                r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Yellow, 3), slabGeometry);
                r.DrawGeometry(Brushes.Transparent, dashedLine2, holeEdgeLinesGeometry);
                r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Red, 8), reducedControlPerimeterGeometry);
                r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Red, 4), controlPerimeterGeometry);
                r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Green, 5), outerPerimeterGeometry);
                r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Red, 5), columnLoadedFaceGeometry);
                r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Yellow, 3), holeGeometry);
                r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Black, 1), voidGeometry);
                r.DrawGeometry(Brushes.Transparent, dashedLine, innerLinksPerimeterGeometry);
                r.DrawGeometry(Brushes.Transparent, dashedLine, outerLinksPerimeterGeometry);
                r.DrawGeometry(Brushes.Red, new Pen(Brushes.PaleVioletRed, 1), spursGeometry);
                r.DrawGeometry(Brushes.LightGray, new Pen(Brushes.Black, 1), linksGeometry);
            }

            testImage.Render(visual);
            return testImage;
        }
    }
}
