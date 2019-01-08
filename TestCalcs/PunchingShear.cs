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

namespace TestCalcs
{
    public class PunchingShear : CalcCore.CalcBase
    {
        CalcSelectionList _colType;
        CalcSelectionList _linkArrangement;
        CalcDouble _fck;
        double fyv = 500;
        double linkDia = 10;
        CalcDouble _columnAdim;
        CalcDouble _columnBdim;
        double G = 600;
        double E = 400;
        double F = -300;
        double H = 200;
        CalcCore.CalcDouble _punchingLoad;
        double udl = 0;
        CalcDouble _h;
        CalcDouble _offsety;
        CalcDouble _offsetz;
        double dy;
        double dz;
        double asy = 1000;
        double asz = 1000;
        double d_average;
        double ui;
        CalcCore.CalcDouble _beta;
        CalcDouble _stressvEdi;
        double stressvEd1;
        double reinforcementRatioy;
        double reinforcementRatioz;
        double rhoL;
        double vRdc;
        double vRdMax;
        double u1;
        CalcDouble _Asw;
        CalcDouble _rebarY;
        CalcDouble _rebarZ;
        CalcDouble _rho;
        double sr;
        double fywd;
        double fywdef;
        CalcDouble _uoutef;
        CalcDouble _distToFirstLinkPerim;
        CalcDouble _perimSpacing;
        CalcDouble _linksInFirstPerim;
        CalcDouble _numberOfPerimeters;
        CalcDouble _legsTotal;
        CalcDouble _legDia;
        CalcDouble _holePosX;
        CalcDouble _holePosY;
        CalcDouble _holeSizeX;
        CalcDouble _holeSizeY;
        CalcDouble _hole2PosX;
        CalcDouble _hole2PosY;
        CalcDouble _hole2SizeX;
        CalcDouble _hole2SizeY;
        CalcDoubleList _moreHoles;

        List<Formula> expressions = new List<Formula>();
        List<Tuple<Line, Line>> _holeEdges;
        //Line holeEdge1;
        //Line holeEdge2;
        //Line holeEdge3;
        //Line holeEdge4;
        List<PolyLine> controlPerimeters;
        List<PolyLine> outerPerimeters;
        PolyLine columnOutline;
        List<PolyLine> columnOutlines;
        List<PolyLine> holeOutlines;
        PolyLine slabEdge;
        List<PolyLine> shearLinkStartPerimeters;
        List<PolyLine> shearLinkEndPerimeters;
        List<Line> shearSpurs;
        List<List<Vector2>> shearLinks;
        List<List<PolyLine>> allPerimeters;
        List<List<PolyLine>> perimetersToReinforce;

        public PunchingShear()
        {
            _colType = inputValues.CreateCalcSelectionList("Column condition", "INTERNAL", new List<string> { "INTERNAL", "EDGE", "CORNER" });
            _linkArrangement = inputValues.CreateCalcSelectionList("Shear link arrangement", "SPURS", new List<string> { "SPURS", "EACH_PERIMETER", "CRUCIFORM" });
            _beta = inputValues.CreateDoubleCalcValue("Beta value", @"\beta", "", 1.367);
            _columnAdim = inputValues.CreateDoubleCalcValue("Column A dimension", "A", "mm", 350);
            _columnBdim = inputValues.CreateDoubleCalcValue("Column B dimension", "B", "mm", 350);
            _h = inputValues.CreateDoubleCalcValue("Slab depth", "h", "mm", 225);
            _offsety = inputValues.CreateDoubleCalcValue("Offset to effective depth y dir", "d_{y,offset}", "mm", 45);
            _offsetz = inputValues.CreateDoubleCalcValue("Offset to effective depth z dir", "d_{z,offset}", "mm", 65);
            _rebarY = inputValues.CreateDoubleCalcValue("Tension reinforcement y dir", "", @"mm^2/m", 1695);
            _rebarZ = inputValues.CreateDoubleCalcValue("Tension reinforcement z dir", "", @"mm^2/m", 1695);
            _fck = inputValues.CreateDoubleCalcValue("Concrete strength", "", @"N/{mm^2}", 40);
            _rho = outputValues.CreateDoubleCalcValue("Reinforcement ratio", "", "", 0);
            _punchingLoad = inputValues.CreateDoubleCalcValue("Punching shear load", "P", "kN", 457);
            _stressvEdi = outputValues.CreateDoubleCalcValue("Shear stress at column face", @"v_{Ed,i}", @"N/{mm^2} ", 0);
            _Asw = outputValues.CreateDoubleCalcValue("Punching shear leg area required per perimeter", "A_{sw}", "mm^2", 0);
            _uoutef = outputValues.CreateDoubleCalcValue("Outer perimeter required", "u_{out,ef}", "mm", 0);
            _linksInFirstPerim = outputValues.CreateDoubleCalcValue("Number of links in first perimeter", "", "No.", 0);
            _numberOfPerimeters = outputValues.CreateDoubleCalcValue("Legs per spur", "", "No.", 0);
            _perimSpacing = outputValues.CreateDoubleCalcValue("Spacing of link perimeters", "", "mm", 0);
            _distToFirstLinkPerim = outputValues.CreateDoubleCalcValue("Distance to first link perimeter", "", "mm", 0);
            _legsTotal = outputValues.CreateDoubleCalcValue("Total legs", "", "No.", 0);
            _legDia = outputValues.CreateDoubleCalcValue("Leg diameter", "", "mm", 0);
            _holePosX = inputValues.CreateDoubleCalcValue("Hole X position", "", "mm", 0);
            _holePosY = inputValues.CreateDoubleCalcValue("Hole Y position", "", "mm", 220);
            _holeSizeX = inputValues.CreateDoubleCalcValue("Hole X size", "", "mm", 200);
            _holeSizeY = inputValues.CreateDoubleCalcValue("Hole Y size", "", "mm", 200);
            _hole2PosX = inputValues.CreateDoubleCalcValue("Hole X position", "", "mm", 500);
            _hole2PosY = inputValues.CreateDoubleCalcValue("Hole Y position", "", "mm", -300);
            _hole2SizeX = inputValues.CreateDoubleCalcValue("Hole X size", "", "mm", 150);
            _hole2SizeY = inputValues.CreateDoubleCalcValue("Hole Y size", "", "mm", 150);
            //_moreHoles = inputValues.CreateCalcDoubleList("Hole corner coordinates", new List<List<double>> { new List<double> { 500, -500, 400, -600, 400, -700, 600,-700, 800,-500 } });
            UpdateCalc();
        }


        public override List<Formula> GenerateFormulae()
        {
            return expressions;
        }

        private void resetFields()
        {
            controlPerimeters = new List<PolyLine>();
            outerPerimeters = new List<PolyLine>();
            columnOutline = null;
            columnOutlines = new List<PolyLine>();
            holeOutlines = null;
            slabEdge = null;
            shearLinkStartPerimeters = new List<PolyLine>();
            shearLinkEndPerimeters = new List<PolyLine>();
            shearSpurs = new List<Line>();
            shearLinks = new List<List<Vector2>>();
            allPerimeters = new List<List<PolyLine>>();
            perimetersToReinforce = new List<List<PolyLine>>();
            _holeEdges = null;
        }

        public override void UpdateCalc()
        {
            formulae = null;
            resetFields();
            expressions = new List<Formula>();

            string _colCondition = _colType.ValueAsString;

            // calculate effective depths in each direction
            dy = _h.Value - _offsety.Value;
            dz = _h.Value - _offsetz.Value;
            d_average = (dy + dz) / 2;
            expressions.Add(new Formula()
            {
                Narrative = "Calculate effective depths",
                Expression = new List<string>
                {
                    string.Format(@"{0} = {1} - {2} = {3}{4}", "d_y", _h.Symbol, _offsety.Symbol, dy, "mm"),
                    string.Format(@"{0} = {1} - {2} = {3}{4}", "d_z", _h.Symbol, _offsetz.Symbol, dz, "mm"),
                    string.Format(@"{0} = \frac{{{1} + {2}}}{{2}} = {3}{4}", @"d_{average}", "d_x", "d_y", d_average, "mm")
                }
            });

            // SET UP GEOMETRY
            // holes
            List<List<Vector2>> allHoleCorners = new List<List<Vector2>>();
            if (_holeSizeX.Value != 0 && _holeSizeY.Value !=0)
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
            foreach (var holeCorners in allHoleCorners)
            {
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
                var holeEdge1 = new Line(new Vector2(0, 0), new Vector2((float)(10000 * Math.Cos(minAngle)), (float)(10000 * Math.Sin(minAngle))));
                var holeEdge2 = new Line(new Vector2(0, 0), new Vector2((float)(10000 * Math.Cos(maxAngle)), (float)(10000 * Math.Sin(maxAngle))));
                _holeEdges.Add(new Tuple<Line, Line>(holeEdge1, holeEdge2));
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


            //outline of column
            var x = (float)_columnAdim.Value / 2f; var y = (float)_columnBdim.Value / 2f;
            generateColumnOutlines();

            generateSlabEdge();

            // outline of control perimeter
            var controlPerimeterNoHoles = generatePerimeter(_columnAdim.Value, _columnBdim.Value, 2 * d_average);
            controlPerimeters = generatePerimeterWithHoles(2 * d_average);

            // calculate shear stress at face
            ui = columnOutlines.Sum(a => a.Length);
            _stressvEdi.Value = _beta.Value * _punchingLoad.Value * 1000 / (ui * d_average);
            var colFaceStressFormula = new Formula()
            {
                Narrative = "Determine effective perimeter at column face and check shear stress.",
                Expression = new List<string>
                {
                    string.Format("u_{{i,no holes}} = {0}mm", Math.Round(columnOutline.Length, 2)),
                    string.Format(@"u_i = {0} mm", Math.Round(ui, 2)),
                    _beta.Symbol + " = " + _beta.Value + _beta.Unit,
                    String.Format(@"{0} = \frac{{{1} {2}}}{{u_i d_{{average}}}} = {3}{4}",
                        _stressvEdi.Symbol,
                        _beta.Symbol, 
                        _punchingLoad.Symbol, 
                        Math.Round(_stressvEdi.Value, 2), 
                        @"N/{mm^2}")
                }
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
                expressions.Insert(0,new Formula
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
                colFaceStressFormula.Expression.Add(string.Format(@"{0} = {1} \leq {2} = {3}{4}", _stressvEdi.Symbol, Math.Round(_stressvEdi.Value, 2), @"v_{Rd,max}", Math.Round(vRdMax, 2), @"N/_{mm^2}"));
                expressions.Add(colFaceStressFormula);
            }

            u1 = controlPerimeters.Sum(a => a.Length);
            stressvEd1 = _beta.Value * _punchingLoad.Value * 1000 / (u1 * d_average);
            expressions.Add(new Formula
            {
                Narrative = "Determine control perimeter including effect of openings and calculate shear stress",
                Expression = new List<string>
                {
                    "u_{1,no holes} = " + Math.Round(controlPerimeterNoHoles.Length,2) + "mm",
                    "u_1 = " + Math.Round(u1,2) + "mm",
                    @"v_{Ed1} = \frac{" + _beta.Symbol + " " + _punchingLoad.Symbol + @"}{u_1 d_{average}} = " + Math.Round(stressvEd1,2) + @"N/{mm^2}"
                },
            });

            reinforcementRatioy = _rebarY.Value / (1000 * dy);
            reinforcementRatioz = _rebarZ.Value / (1000 * dz);

            _rho.Value = Math.Pow(reinforcementRatioy * reinforcementRatioz, 0.5);

            vRdc = shearResistanceNoRein(_rho.Value, d_average, _fck.Value);

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
                expressions.Insert(0,new Formula
                {
                    Narrative = "Diagram:" + Environment.NewLine +
                    "Column shown in green, control and outer perimeters in red.",
                    Image = image2,
                    Conclusion = "No links required",
                    Status = CalcStatus.PASS
                });
                return;
            }
            if (stressvEd1 > 2 * vRdc)
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
                expressions.Insert(0,new Formula
                {
                    Narrative = "Diagram:" + Environment.NewLine +
                    "Column shown in green, control and outer perimeters in red.",
                    Image = image2,
                    Conclusion = "Redesign slab",
                    Status = CalcStatus.FAIL
                });
                return;
            }

            sr = 0.75 * d_average;
            expressions.Add(new Formula
            {
                Narrative = "Maximum radial spacing",
                Expression = new List<string>
                {
                    @"s_r = 0.75 \times d_{average}",
                    @"s_r = " + Math.Round(sr,0) + "mm"
                }
            });
            fywd = 338; // need to calc this in future
            fywdef = Math.Min(250 + 0.25 * d_average, fywd);
            _Asw.Value = (stressvEd1 - 0.75 * vRdc) * sr * u1 / (1.5 * fywdef);
            expressions.Add(new Formula
            {
                Narrative = "Area of shear reinforcement required",
                Expression = new List<string>
                {
                    @"f_{ywdef} = \min{(250 + 0.25 \times d_{average},f_{ywd})} = \min{(" + (250 + 0.25 * d_average) + @"," + fywd+@")} = " + fywdef + @"N/mm^2",
                    _Asw.Symbol + @" = \frac{v_{Ed1} - 0.75v_{Rdc}s_ru_1}{1.5 \times f_{ywdef}} =" + Math.Round(_Asw.Value,0) + _Asw.Unit
                }
            });

            _uoutef.Value = _beta.Value * 1000 * _punchingLoad.Value / (vRdc * d_average);

            _distToFirstLinkPerim.Value = 0.5 * d_average;
            _perimSpacing.Value = 0.75 * d_average;
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

            //if (_linkArrangement.ValueAsString == "SPURS")
            //{
            //    int numberOfPerimeters = Math.Min(shearLinkStartPerimeters.Count, shearLinkEndPerimeters.Count);
            //    for (int k = 0; k < numberOfPerimeters; k++)
            //    {
            //        var shearLinkStartPerimeter = shearLinkStartPerimeters[k];
            //        var shearLinkEndPerimeter = shearLinkEndPerimeters[k];
            //        int spursRequired = (int)Math.Max(Math.Ceiling(shearLinkEndPerimeter.Length / (1.5 * d_average)), 2);
            //        double spurFraction = 1d / Math.Max((spursRequired - 1), 1);
            //        for (int i = 0; i < spursRequired; i++)
            //        {
            //            var startPoint = shearLinkStartPerimeter.PointAtParameter(i * spurFraction);
            //            var endPoint = shearLinkEndPerimeter.PointAtParameter(i * spurFraction);
            //            var shearLine = new Line(startPoint, endPoint);
            //            shearSpurs.Add(shearLine);
            //            double maxSpacing = 0.75 * d_average;
            //            double numberOfStuds = perimetersToReinforce.Count;
            //            var spacingVector = (shearLine.End - shearLine.Start) / ((float)numberOfStuds - 1);
            //            var links = new List<Vector2>();
            //            for (int j = 0; j < numberOfStuds; j++)
            //            {
            //                links.Add(shearLine.Start + spacingVector * j);
            //            }
            //            shearLinks.Add(links);
            //        }
            //    }
            //}

            var shearLinkStartSpurs = new List<PolyLine>();
            for (int i = 0; i < perimetersToReinforce.Count; i++)
            {
                shearLinkStartSpurs.Add(generatePerimeter(_columnAdim.Value, _columnBdim.Value, 0.5 * d_average + i * (0.75 * d_average)));
            }
            //if (_linkArrangement.ValueAsString == "SPURS")
            //{
            //    int spursRequired = (int)Math.Max(Math.Ceiling(shearLinkStartPerimeters.Sum(a => a.Length) / (1.5 * d_average)), 2);


            //    foreach (var perimeter in shearLinkStartSpurs)
            //    {
            //        double fraction = perimeter.Length / (spursRequired);
            //        if (perimeter.Length / spursRequired > 1.5* d_average)
            //        {
            //            spursRequired = spursRequired * 2;
            //            fraction = fraction / 2;
            //        }
            //        for (int k = 0; k < spursRequired; k++)
            //        {
            //            var parameter = (k * fraction) / perimeter.Length;
            //            var point = perimeter.PointAtParameter(parameter);
            //            var shearLine = new Line(point, point);
            //            shearSpurs.Add(shearLine);
            //            var links = new List<Vector2>() { point };
            //            //for (int j = 0; j < numberOfStuds; j++)
            //            //{
            //            //    links.Add(shearLine.Start + spacingVector * j);
            //            //}
            //            shearLinks.Add(links);

            //        }
            //    }

            //}

            if (_linkArrangement.ValueAsString == "SPURS")
            {
                perimetersToReinforce = new List<List<PolyLine>> { perimetersToReinforce[0] };
                int spursRequired = 0;

                switch (_colType.ValueAsString)
                {
                    case "INTERNAL":
                        if ((shearLinkStartPerimeters.Sum(a => a.Length) / 8) < 1.5 * d_average)
                            spursRequired = 8;
                        else spursRequired = 12;
                        break;
                    case "EDGE":
                        Line line1 = new Line(new Vector2(0, 0), new Vector2(0, -10000));
                        Line line2 = new Line(new Vector2(0, 0), new Vector2(0, 10000));
                        IntersectionResult inter1 = shearLinkStartSpurs[0].intersection(line1)[0];
                        IntersectionResult inter2 = shearLinkStartSpurs[0].intersection(line2)[0];
                        shearLinkStartSpurs[0] = shearLinkStartSpurs[0].Cut(inter1.Parameter, inter2.Parameter);
                        if ((shearLinkStartPerimeters.Sum(a => a.Length) / 5) < 1.5 * d_average)
                            spursRequired = 5;
                        else spursRequired = 9;
                        break;
                    case "CORNER":
                        Line line3 = new Line(new Vector2(0, 0), new Vector2(0, -10000));
                        Line line4 = new Line(new Vector2(0, 0), new Vector2(10000, 0));
                        IntersectionResult inter3 = shearLinkStartSpurs[0].intersection(line3)[0];
                        IntersectionResult inter4 = shearLinkStartSpurs[0].intersection(line4)[0];
                        shearLinkStartSpurs[0] = shearLinkStartSpurs[0].Cut(inter3.Parameter, inter4.Parameter);
                        if ((shearLinkStartPerimeters.Sum(a => a.Length) / 3) < 1.5 * d_average)
                            spursRequired = 3;
                        else spursRequired = 5;
                        break;
                    default:
                        break;
                }

                var spurPoints = new List<List<Vector2>>();
                double fraction = shearLinkStartSpurs[0].Length / (spursRequired);
                if (_colType.ValueAsString != "INTERNAL")
                {
                    fraction = shearLinkStartSpurs[0].Length / (spursRequired -1);
                }
                for (int i = 0; i < spursRequired; i++)
                {
                    var parameter = (i * fraction) / shearLinkStartSpurs[0].Length;
                    var point = shearLinkStartSpurs[0].PointAtParameter(parameter);
                    spurPoints.Add(new List<Vector2> { point });
                }
                for (int i = 1; i < 25; i++)
                {
                    int spursOnPrevPerim = spurPoints.Count;
                    for (int k = 0; k < spursOnPrevPerim; k++)
                    {
                        var vector = Vector2.Normalize(spurPoints[k][0]);
                        spurPoints[k].Add(spurPoints[k].Last() + vector * 0.75f * (float)d_average);
                    }
                    var segs = new List<GeometryBase>();
                    var prevPoint = spurPoints.Last().Last();
                    int start = 0;
                    if (_colType.ValueAsString != "INTERNAL")
                    {
                        prevPoint = spurPoints.First().Last();
                        start = 1;
                    }


                    for (int j = start; j < spurPoints.Count; j++)
                    {
                        var item = spurPoints[j];
                        if ((item.Last() - prevPoint).Length() > 1.5 * d_average)
                        {
                            var interPoint = (prevPoint + item.Last()) / 2;
                            spurPoints.Insert(j, new List<Vector2>
                            {
                                interPoint
                            });
                            segs.Add(new Line(prevPoint, interPoint));
                            segs.Add(new Line(interPoint, item.Last()));
                        }
                        else
                        {
                            segs.Add(new Line(prevPoint, item.Last()));
                        }
                        prevPoint = item.Last();
                    }
                    var newPerim = new PolyLine(segs);
                    var newPerimWithHoles = generatePerimeterWithHoles(newPerim);
                    perimetersToReinforce.Add(newPerimWithHoles);
                    if (newPerimWithHoles.Sum(a => a.Length) > _uoutef.Value)
                    {
                        break;
                    }
                }
                List<int> indicesToDelete = new List<int>();
                for (int i = 0; i < spurPoints.Count; i++)
                {
                    var item = spurPoints[i];
                    if (item.Count < 3)
                    {
                        indicesToDelete.Add(i);
                    }
                    else
                    {
                        item.RemoveRange(item.Count - 2, 2);
                    }
                }
                indicesToDelete.Reverse();
                foreach (var item in indicesToDelete)
                {
                    spurPoints.RemoveAt(item);
                }

                outerPerimeters = perimetersToReinforce.Last() ;
                if (perimetersToReinforce.Count > 3)
                {
                    perimetersToReinforce.RemoveRange(perimetersToReinforce.Count - 2, 2);
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
                            var angle3 = Math.Atan2(link.Y, link.X);
                            if (angle3 < 0) angle3 += Math.PI * 2;
                            if (Math.Abs(angle1 - angle2) < Math.PI)
                            {
                                if (angle3>Math.Min(angle1, angle2) && angle3<Math.Max(angle1, angle2))
                                {
                                    include = false;
                                }
                            }
                            else
                            {
                                if (angle3 < Math.Min(angle1, angle2) && angle3 > Math.Max(angle1, angle2))
                                {
                                    include = false;
                                }
                            }
                        }
                        if (include) filteredList.Add(link);
                    }
                    shearLinks.Add(filteredList);
                }

            }

            if (_linkArrangement.ValueAsString == "EACH_PERIMETER")
            {
                shearLinks = new List<List<Vector2>>();
                foreach (var perimeter in perimetersToReinforce)
                {
                    var linksList = new List<Vector2>();
                    foreach (var segment in perimeter)
                    {
                        if (segment.Length < 1.5 * d_average)
                        {
                            var pt = segment.PointAtParameter(0.5);
                            linksList.Add(pt);
                        }
                        else if (segment.Length >= 1.5 * d_average)
                        {
                            int links = (int)Math.Ceiling(segment.Length / (1.5 * d_average));
                            double halfSpacing = 1d / (2d * links);
                            for (int i = 0; i < links; i++)
                            {
                                var pt = segment.PointAtParameter(halfSpacing * (1 + i * 2));
                                linksList.Add(pt);
                            }
                        }
                    }
                    shearLinks.Add(linksList);
                }
            }

            if (_linkArrangement.ValueAsString == "CRUCIFORM")
            {

            }

            if (_linkArrangement.ValueAsString == "EACH_PERIMETER")
            {
                _linksInFirstPerim.Value = shearLinks[0].Count;
            }
            else
            {
                _linksInFirstPerim.Value = shearLinks.Count;
            }
            _numberOfPerimeters.Value = perimetersToReinforce.Count;
            _legsTotal.Value = shearLinks.Sum(a => a.Count);
            _legDia.Value = calcBarSizeAndDia(_Asw.Value / _linksInFirstPerim.Value, new List<int> { 8, 10, 12, 16 });

            expressions.Add(new Formula
            {
                Narrative = "Detailing dimensions:",
                Expression = new List<string>
                {
                    @"\text{Distance to first perimeter} =" + Math.Round(_distToFirstLinkPerim.Value,0) + _distToFirstLinkPerim.Unit,
                    @"\text{Perimeter spacing} =" + Math.Round(_perimSpacing.Value,0) + _perimSpacing.Unit
                }
            });

            var image = generateImage();
            expressions.Insert(0, new Formula
            {
                Narrative = "Diagram:" + Environment.NewLine +
                "Column shown in green, control and outer perimeters in red.",
                Image = image,
                Conclusion = "OK",
                Status = CalcStatus.PASS
            });
        }

        private void generateColumnOutlines()
        {
            var x = (float)_columnAdim.Value / 2f; var y = (float)_columnBdim.Value / 2f;
            columnOutline = new PolyLine(new List<GeometryBase>
            {
                new Line(new Vector2(-x,y), new Vector2(-x,-y)),
                new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                new Line(new Vector2(x,-y), new Vector2(x,y)),
            })
            { IsClosed = true };
            var columnOutline2 = columnOutline;

            switch (_colType.ValueAsString)
            {
                case "INTERNAL":
                    break;
                case "EDGE":
                    if (1.5 * d_average > _columnAdim.Value)
                    {
                        columnOutline2 = new PolyLine(new List<GeometryBase>
                        {
                            new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                            new Line(new Vector2(x,-y), new Vector2(x,y)),
                            new Line(new Vector2(x,y), new Vector2(-x,y)),
                        });
                    }
                    else
                    {
                        columnOutline2 = new PolyLine(new List<GeometryBase>
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
                        columnOutline2 = new PolyLine(new List<GeometryBase>
                        {
                            new Line(new Vector2(-x,-y), new Vector2(x,-y)),
                            new Line(new Vector2(x,-y), new Vector2(x,y)),
                        });
                    }
                    else
                    {
                        columnOutline2 = new PolyLine(new List<GeometryBase>
                        {
                            new Line(new Vector2(x-(float)Math.Min(1.5*d_average, _columnAdim.Value),-y), new Vector2(x,-y)),
                            new Line(new Vector2(x,-y), new Vector2(x,(float)Math.Min(1.5*d_average, _columnBdim.Value) -y)),
                        });
                    }
                    break;
                default:
                    break;
            }
            columnOutlines = generatePerimeterWithHoles(columnOutline2);
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

        public static double shearResistanceNoRein(double rhoL, double d_eff, double charStrength)
        {
            double k = Math.Min(1 + Math.Sqrt(200 / d_eff),2);

            double resistance1 = 0.12 * k * Math.Pow((100 * rhoL * charStrength), 1f / 3f);

            double resistance = Math.Max(resistance1,0.035 * Math.Pow(k, 1.5) * Math.Pow(charStrength, 0.5));

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
                    inter2 = perimeter.intersection(new Line(new Vector2(-x, -10000), new Vector2(-x, y)));
                    inter1 = perimeter.intersection(new Line(new Vector2(-x, y), new Vector2(-x, 10000)));
                    perimeter2 = perimeter.Cut(inter2[0].Parameter, inter1[0].Parameter);
                    break;
                case "CORNER":
                    inter2 = perimeter.intersection(new Line(new Vector2(-x, -10000), new Vector2(-x, y)));
                    inter1 = perimeter.intersection(new Line(new Vector2(-x, y), new Vector2(10000, y)));
                    perimeter2 = perimeter.Cut(inter2[0].Parameter, inter1[0].Parameter);
                    break;
                default:
                    break;
            }

            return perimeter2;
        }

        private List<PolyLine> generatePerimeterWithHoles(PolyLine perimeter)
        {
            var perimeters = new List<PolyLine>();
            var perimeter2 = perimeter;
            //var inter1 = perimeter2.intersection(holeEdge1);
            //var inter2 = perimeter2.intersection(holeEdge2);
            //var inter3 = perimeter2.intersection(holeEdge3);
            //var inter4 = perimeter2.intersection(holeEdge4);
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


            //if (inter1.Count == 0 && inter2.Count == 0)
            //{
            //    perimeters.Add(perimeter2);
            //}
            //else if (inter1.Count > 0 && inter2.Count > 0)
            //{
            //    if (inter1[0].Parameter > inter2[0].Parameter)
            //    {
            //        perimeters.Add(perimeter2.Cut(inter2[0].Parameter, inter1[0].Parameter));
            //    }
            //    else
            //    {
            //        perimeters.Add(perimeter2.Cut(0, inter1[0].Parameter));
            //        perimeters.Add(perimeter2.Cut(inter2[0].Parameter, 1));
            //    }
            //}
            //else if (inter1.Count == 0 && inter2.Count > 0)
            //{
            //    perimeters.Add(perimeter2.Cut(inter2[0].Parameter, 1));
            //}
            //else if (inter1.Count > 0 && inter2.Count == 0)
            //{
            //    perimeters.Add(perimeter2.Cut(0, inter1[0].Parameter));
            //}
            return perimeters;
        }


        private List<PolyLine> generatePerimeterWithHoles(double offset)
        {
            var perimeters = new List<PolyLine>();
            var perimeter2 = generatePerimeter(_columnAdim.Value, _columnBdim.Value, offset);
            return generatePerimeterWithHoles(perimeter2);
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
                var outerPerimeterGeometry = generateGeometry(outerPerimeters);
                var columnGeometry = generateGeometry(columnOutline);
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
                double scale = 800 / (Math.Max(newBounds.Width, newBounds.Height));
                var scaleTransform = new ScaleTransform(scale, -scale);
                var transX = scale * -newBounds.X;
                var transY = scale * -newBounds.Y;
                var transTransform = new TranslateTransform(transX, transY);
                var transGroup = new TransformGroup();
                transGroup.Children.Add(scaleTransform);
                transGroup.Children.Add(transTransform);

                columnLoadedFaceGeometry.Transform = transGroup;
                controlPerimeterGeometry.Transform = transGroup;
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
                r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Green, 3), slabGeometry);
                r.DrawGeometry(Brushes.Transparent, dashedLine2, holeEdgeLinesGeometry);
                r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Red, 5), controlPerimeterGeometry);
                r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Red, 5), outerPerimeterGeometry);
                r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Red, 5), columnLoadedFaceGeometry);
                r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Green, 3), holeGeometry);
                r.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Black, 1), voidGeometry);
                r.DrawGeometry(Brushes.Transparent, dashedLine, innerLinksPerimeterGeometry);
                r.DrawGeometry(Brushes.Transparent, dashedLine, outerLinksPerimeterGeometry);
                r.DrawGeometry(Brushes.Red, new Pen(Brushes.PaleVioletRed, 1), spursGeometry);
                r.DrawGeometry(Brushes.LightGray, new Pen(Brushes.Black, 1), linksGeometry);
            }

            testImage.Render(visual);
            return testImage;
        }

        abstract class GeometryBase
        {
            public abstract Vector2 Start { get;  }
            public abstract Vector2 End { get;  }
            public abstract double Length { get;  }
            public abstract List<IntersectionResult> intersection(Line line);
            public abstract List<GeometryBase> Cut(double parameter);
            public abstract Vector2 PointAtParameter(double parameter);
            public abstract Vector2 PerpAtParameter(double parameter);
        }

        class PolyLine
        {
            List<GeometryBase> _segments;
            public List<GeometryBase> Segments
            {
                get
                {
                    return _segments.ToList();
                }
            }


            public Vector2 Start
            {
                get
                {
                    return _segments.First().Start;
                }
            }

            public Vector2 End
            {
                get
                {
                    return _segments.Last().End;
                }
            }

            public double Length
            {
                get
                {
                    double returnLength = 0;
                    foreach (var item in _segments)
                    {
                        returnLength += item.Length;
                    }
                    return returnLength;
                }
            }

            bool _isClosed = false;
            public bool IsClosed
            {
                get
                {
                    return _isClosed;
                }
                set
                {
                    if (value)
                    {
                        if ((End - Start).Length() < 0.1)
                        {
                            _isClosed = value;
                        }
                        else
                        {
                            _segments.Add(new Line(End, Start));
                        }
                    }
                    else
                        _isClosed = value;
                }
            }

            public PolyLine(List<GeometryBase> segments)
            {
                _segments = segments;
            }

            public List<IntersectionResult> intersection(Line line) // need to add full support for NONE and PROJECTED 
            {
                double totalLength = this.Length;
                double lengthToSegment = 0;
                List<IntersectionResult> returnList = new List<IntersectionResult>();
                foreach (var segment in _segments)
                {
                    var intersections = segment.intersection(line);
                    foreach (var intersection in intersections)
                    {
                        if (intersection.TypeOfIntersection == IntersectionType.WITHIN)
                        {
                            returnList.Add(new IntersectionResult((lengthToSegment + intersection.Parameter * segment.Length) / totalLength, intersection.Point, intersection.TypeOfIntersection));
                        }
                    }
                    lengthToSegment += segment.Length;
                }
                if (returnList.Count == 0)
                {
                    returnList.Add(new IntersectionResult(double.NaN, new Vector2(), IntersectionType.NONE));
                }
                return returnList;
            }

            public override string ToString()
            {
                string returnString = "";
                returnString = "Start " + _segments[0].Start.ToString();
                returnString += Environment.NewLine;

                foreach (var item in _segments)
                {
                    if (item.GetType() == typeof(Arc))
                    {
                        returnString += " Arc ";
                        returnString += item.End;
                        returnString += Environment.NewLine;
                    }
                    if (item.GetType() == typeof(Line))
                    {
                        returnString += " Line ";
                        returnString += item.End;
                        returnString += Environment.NewLine;
                    }
                }

                return returnString;
            }

            public Vector2 PointAtParameter(double parameter)
            {
                double totalLength = this.Length;
                double lengthToSegmentStart = 0;
                double lengthToSegmentEnd = 0;
                for (int i = 0; i < _segments.Count; i++)
                {
                    var segment = _segments[i];
                    lengthToSegmentStart = lengthToSegmentEnd;
                    lengthToSegmentEnd += segment.Length;
                    double parameterToSegmentEnd = lengthToSegmentEnd / totalLength;
                    double parameterToSegmentStart = lengthToSegmentStart / totalLength;
                    if (parameter >= parameterToSegmentStart && parameter <= parameterToSegmentEnd)
                    {
                        double segmentParameter = (parameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                        if (double.IsNaN(segmentParameter))
                            segmentParameter = 0;
                        return segment.PointAtParameter(segmentParameter);
                    }
                }
                return _segments.Last().PointAtParameter(1);
            }

            public Vector2 PerpAtParameter(double parameter)
            {
                double totalLength = this.Length;
                double lengthToSegmentStart = 0;
                double lengthToSegmentEnd = 0;
                for (int i = 0; i < _segments.Count; i++)
                {
                    var segment = _segments[i];
                    lengthToSegmentStart = lengthToSegmentEnd;
                    lengthToSegmentEnd += segment.Length;
                    double parameterToSegmentEnd = lengthToSegmentEnd / totalLength;
                    double parameterToSegmentStart = lengthToSegmentStart / totalLength;
                    if (parameter >= parameterToSegmentStart && parameter <= parameterToSegmentEnd)
                    {
                        double segmentParameter = (parameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                        if (double.IsNaN(segmentParameter))
                            segmentParameter = 0;
                        return segment.PerpAtParameter(segmentParameter);
                    }
                }
                return _segments.Last().PerpAtParameter(1);
            }

            public PolyLine Cut(double startParameter, double endParameter)
            {
                double totalLength = this.Length;
                double lengthToSegmentStart = 0;
                double lengthToSegmentEnd = 0;
                bool started = false;
                bool finished = false;
                List<GeometryBase> segments = new List<GeometryBase>(); 
                for (int i = 0; i < _segments.Count; i++)
                {
                    var segment = _segments[i];
                    lengthToSegmentStart = lengthToSegmentEnd;
                    lengthToSegmentEnd += segment.Length;
                    double parameterToSegmentEnd = lengthToSegmentEnd / totalLength;
                    double parameterToSegmentStart = lengthToSegmentStart / totalLength;
                    if ((endParameter > parameterToSegmentEnd && started) || (endParameter <= startParameter && started))
                    {
                        segments.Add(segment);
                    }
                    else if (startParameter <= parameterToSegmentEnd && !started)
                    {
                        if (endParameter == 0.1)
                        {
                            //
                        }
                        if (endParameter <= parameterToSegmentEnd && endParameter >= startParameter)
                        {
                            double segmentParameter = (endParameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                            if (double.IsNaN(segmentParameter))
                                segmentParameter = 0;
                            var partialCut = segment.Cut(segmentParameter)[0];
                            double secondSegmentParameter = (startParameter - parameterToSegmentStart) / (endParameter - parameterToSegmentStart);
                            segments.Add(partialCut.Cut(secondSegmentParameter)[1]);
                            started = true;
                            finished = true;
                        }
                        else
                        {
                            double segmentParameter = (startParameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                            if (double.IsNaN(segmentParameter))
                                segmentParameter = 0;
                            segments.Add(segment.Cut(segmentParameter)[1]);
                            started = true;
                        }
                    }
                    else if (endParameter <= parameterToSegmentEnd && endParameter >= startParameter && started && !finished)
                    {
                        double segmentParameter = (endParameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                        if (double.IsNaN(segmentParameter))
                            segmentParameter = 0;
                        segments.Add(segment.Cut(segmentParameter)[0]);
                        finished = true;
                        break;
                    }
                }
                if (finished)
                {
                    return new PolyLine(segments);
                }
                lengthToSegmentEnd = 0;
                lengthToSegmentStart = 0;
                for (int i = 0; i < _segments.Count; i++)
                {
                    var segment = _segments[i];
                    lengthToSegmentStart = lengthToSegmentEnd;
                    lengthToSegmentEnd += segment.Length;
                    double parameterToSegmentEnd = lengthToSegmentEnd / totalLength;
                    double parameterToSegmentStart = lengthToSegmentStart / totalLength;
                    if (endParameter > parameterToSegmentEnd)
                    {
                        segments.Add(segment);
                    }
                    //else if (startParameter <= parameterToSegmentEnd && !started)
                    //{
                    //    double segmentParameter = (startParameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                    //    segments.Add(segment.Cut(segmentParameter)[1]);
                    //    started = true;
                    //}
                    else if (endParameter <= parameterToSegmentEnd)
                    {
                        double segmentParameter = (endParameter - parameterToSegmentStart) / (parameterToSegmentEnd - parameterToSegmentStart);
                        if (double.IsNaN(segmentParameter))
                            segmentParameter = 0;
                        segments.Add(segment.Cut(segmentParameter)[0]);
                        finished = true;
                        break;
                    }
                }
                return new PolyLine(segments);
            }
        }

        class Line : GeometryBase
        {
            Vector2 _start;
            public override Vector2 Start { get => _start; }
            Vector2 _end;
            public override Vector2 End { get => _end; }
            public override double Length
            {
                get
                {
                    {
                        return (End - Start).Length();
                    }
                }
            }

            public override List<GeometryBase> Cut(double parameter)
            {
                if (parameter < 0 || parameter > 1)
                {
                    return null;
                }
                else
                {
                    Vector2 vector = End - Start;
                    return new List<GeometryBase>
                    {
                        new Line(Start, Start + vector * (float)parameter),
                        new Line(Start + vector * (float)parameter, End)
                    };
                }
            }

            public Line(Vector2 start, Vector2 end)
            {
                _start = start;
                _end = end;
            }

            public override Vector2 PointAtParameter(double parameter)
            {
                    Vector2 vector = End - Start;
                    return (float)parameter * vector + Start;
            }

            public override Vector2 PerpAtParameter(double parameter)
            {
                Vector2 v = End - Start;
                var returnVector = new Vector2((
                    float)(v.X * Math.Cos(Math.PI / 2) - v.Y * Math.Sin(Math.PI / 2)),
                    (float)(v.X * Math.Sin(Math.PI / 2) + v.Y * Math.Cos(Math.PI / 2)));
                return Vector2.Normalize(returnVector);
            }

            public override List<IntersectionResult> intersection(Line line)
            {
                var p1 = _start;
                var p1_2 = _end;
                var p2 = line.Start;
                var p2_2 = line.End;

                double A1 = p1_2.Y - p1.Y;
                double B1 = p1.X - p1_2.X;
                double C1 = A1 * p1.X + B1 * p1.Y;
                double A2 = p2_2.Y - p2.Y;
                double B2 = p2.X - p2_2.X;
                double C2 = A2 * p2.X + B2 * p2.Y;
                double det = A1 * B2 - A2 * B1;
                double endX = 0; double endY = 0;
                if (det == 0)
                {
                    //return new IntersectionResult(IntersectionType.NONE, new Point());
                    return new List<IntersectionResult> { new IntersectionResult(double.NaN, new Vector2(), IntersectionType.NONE) };
                    //return new List<Vector2>();
                }
                endX = (B2 * C1 - B1 * C2) / det;
                endY = (A1 * C2 - A2 * C1) / det;
                double tol = 0.00000000001;
                if (
                    endX + tol >= Math.Min(p1.X, p1_2.X)
                    &&
                    endX - tol <= Math.Max(p1.X, p1_2.X)
                    &&
                    endY + tol >= Math.Min(p1.Y, p1_2.Y)
                    &&
                    endY - tol <= Math.Max(p1.Y, p1_2.Y)
                    &&
                    endX + tol >= Math.Min(p2.X, p2_2.X)
                    &&
                    endX - tol <= Math.Max(p2.X, p2_2.X)
                    &&
                    endY + tol >= Math.Min(p2.Y, p2_2.Y)
                    &&
                    endY - tol <= Math.Max(p2.Y, p2_2.Y)
                    )
                {
                    //return new IntersectionResult(IntersectionType.WITHIN, new Point(endX, endY));
                    Vector2 inter = new Vector2((float)endX, (float)endY);
                    double param = (inter - this.Start).Length() / this.Length;
                    return new List<IntersectionResult> { new IntersectionResult(param, inter , IntersectionType.WITHIN) };
                    //return new List<Vector2> { new Vector2((float)endX, (float)endY) };
                }
                else
                {
                    //return new IntersectionResult(IntersectionType.PROJECTED, new Point(endX, endY));
                    Vector2 inter = new Vector2((float)endX, (float)endY);
                    double param = (inter - this.Start).Length() / this.Length;
                    return new List<IntersectionResult> { new IntersectionResult(double.NaN, inter, IntersectionType.PROJECTED) };
                    //return new List<Vector2>();

                }
            }
        }

        class IntersectionResult
        {
            public double Parameter { get; private set; }
            public Vector2 Point { get; private set; }
            public IntersectionType TypeOfIntersection { get; private set; } 

            public IntersectionResult(double parameter, Vector2 point, IntersectionType interType)
            {
                Parameter = parameter;
                Point = point;
                TypeOfIntersection = interType;
            }

            public override string ToString()
            {
                return string.Format("Point {0}; parameter {1}; type {2}", Point, Parameter, TypeOfIntersection);
            }

        }

        enum IntersectionType
        {
            NONE,
            WITHIN,
            PROJECTED
        }

        class Ellipse
        {
            public Vector2 Centre { get; set; }
            public double StartAngle { get; set; }
            public double EndAngle { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
        }

        class Arc : GeometryBase
        {
            public Vector2 Centre { get; set; }
            public double StartAngle { get; set; }
            public double EndAngle { get; set; }
            double _radius;
            public double Radius
            {
                get
                {
                    return _radius;
                }
                set
                {
                    _radius = Math.Abs(value);
                }
            }
            public override Vector2 Start
            {
                get
                {
                    return new Vector2((float)(Centre.X + Radius * Math.Cos(StartAngle)), (float)(Centre.Y + Radius * Math.Sin(StartAngle)));
                }
            }
            public override Vector2 End
            {
                get
                {
                    return new Vector2((float)(Centre.X + Radius * Math.Cos(EndAngle)), (float)(Centre.Y + Radius * Math.Sin(EndAngle)));
                }
            }
            public override double Length
            {
                get
                {
                    if (StartAngle == EndAngle)
                    {
                        return 0;
                    }
                    if (StartAngle > EndAngle)
                    {
                        double end = EndAngle + Math.PI;
                        return ((end - StartAngle) / (2 * Math.PI)) * 2 * Math.PI * Radius;
                    }
                    else
                    {
                        double end = EndAngle;
                        return ((end - StartAngle) / (2 * Math.PI)) * 2 * Math.PI * Radius;
                    }
                }
            }

            public override List<GeometryBase> Cut(double parameter)
            {
                if (parameter < 0 || parameter > 1)
                {
                    return null;
                }
                else
                {
                    double diffAngle = parameter * (EndAngle - StartAngle);
                    if (diffAngle < 0)
                    {
                        diffAngle += 2*Math.PI;
                    }

                    return new List<GeometryBase>
                    {
                        new Arc{Centre = this.Centre, StartAngle = this.StartAngle, EndAngle = this.StartAngle + diffAngle, Radius = this.Radius},
                        new Arc{Centre = this.Centre, StartAngle = this.StartAngle + diffAngle, EndAngle = this.EndAngle, Radius = this.Radius}
                    };
                }
            }

            public override Vector2 PointAtParameter(double parameter)
            {
                    double diffAngle = parameter * (EndAngle - StartAngle);
                    if (diffAngle < 0)
                    {
                        diffAngle += 2 * Math.PI;
                    }
                    return new Vector2((float)(Centre.X + Radius * Math.Cos(StartAngle + diffAngle)), (float)(Centre.Y + Radius * Math.Sin(StartAngle + diffAngle)));
            }

            public override Vector2 PerpAtParameter(double parameter)
            {
                double diffAngle = parameter * (EndAngle - StartAngle);
                if (diffAngle < 0)
                {
                    diffAngle += 2 * Math.PI;
                }
                var point = new Vector2((float)(Centre.X + Radius * Math.Cos(StartAngle + diffAngle)), (float)(Centre.Y + Radius * Math.Sin(StartAngle + diffAngle)));
                var returnVector = point - Centre;
                return Vector2.Normalize(returnVector);
            }

            // Find the points of intersection.
            public override List<IntersectionResult> intersection(Line line)
            {
                var arc = this;
                var point1 = line.Start;
                var point2 = line.End;
                var cx = arc.Centre.X;
                var cy = arc.Centre.Y;
                var radius = (float)arc.Radius;

                Vector2 intersection1;
                Vector2 intersection2;

                float dx, dy, A, B, C, det, t;

                dx = point2.X - point1.X;
                dy = point2.Y - point1.Y;

                A = dx * dx + dy * dy;
                B = 2 * (dx * (point1.X - cx) + dy * (point1.Y - cy));
                C = (point1.X - cx) * (point1.X - cx) + (point1.Y - cy) * (point1.Y - cy) - radius * radius;

                det = B * B - 4 * A * C;
                if ((A <= 0.0000001) || (det < 0))
                {
                    // No real solutions.
                    return new List<IntersectionResult>();
                }
                else if (det == 0)
                {
                    // One solution.
                    var returnList = new List<IntersectionResult>();
                    t = -B / (2 * A);
                    intersection1 = new Vector2(point1.X + t * dx, point1.Y + t * dy);
                    double angle = Math.Atan2(intersection1.Y - arc.Centre.Y, intersection1.X - arc.Centre.X);
                    if (angle < 0)
                    {
                        angle += 2 * Math.PI;
                    }
                    var endX = intersection1.X; var endY = intersection1.Y; float tol = 0.000001f;
                    if (angle >= arc.StartAngle && 
                        angle <= arc.EndAngle &&
                        endX + tol >= Math.Min(point1.X, point2.X) &&
                        endX - tol <= Math.Max(point1.X, point2.X) &&
                        endY + tol >= Math.Min(point1.Y, point2.Y) &&
                        endY - tol <= Math.Max(point1.Y, point2.Y))
                    {
                        double temp = angle - StartAngle;
                        if (temp < 0) temp += 2 * Math.PI;

                        double param = (this.Radius * temp) / (this.Length);
                        
                        returnList.Add(new IntersectionResult(param, intersection1, IntersectionType.WITHIN));
                    }
                    return returnList;
                }
                else
                {
                    // Two solutions.
                    var returnList = new List<IntersectionResult>();
                    t = (float)((-B + Math.Sqrt(det)) / (2 * A));
                    intersection1 = new Vector2(point1.X + t * dx, point1.Y + t * dy);
                    double angle = Math.Atan2(intersection1.Y - arc.Centre.Y, intersection1.X - arc.Centre.X);
                    if (angle < 0)
                    {
                        angle += 2 * Math.PI;
                    }
                    var endX = intersection1.X; var endY = intersection1.Y; float tol = 0.000001f;
                    if (angle >= arc.StartAngle &&
                        angle <= arc.EndAngle &&
                        endX + tol >= Math.Min(point1.X, point2.X) &&
                        endX - tol <= Math.Max(point1.X, point2.X) &&
                        endY + tol >= Math.Min(point1.Y, point2.Y) &&
                        endY - tol <= Math.Max(point1.Y, point2.Y))
                    {
                        double temp = angle - StartAngle;
                        if (temp < 0) temp += 2 * Math.PI;

                        double param = (this.Radius * temp) / (this.Length);

                        returnList.Add(new IntersectionResult(param, intersection1, IntersectionType.WITHIN));
                    }
                    t = (float)((-B - Math.Sqrt(det)) / (2 * A));
                    intersection2 = new Vector2(point1.X + t * dx, point1.Y + t * dy);
                    angle = Math.Atan2(intersection2.Y - arc.Centre.Y, intersection2.X - arc.Centre.X);
                    if (angle < 0)
                    {
                        angle += 2 * Math.PI;
                    }
                    endX = intersection2.X; endY = intersection2.Y; 
                    if (angle >= arc.StartAngle &&
                        angle <= arc.EndAngle &&
                        endX + tol >= Math.Min(point1.X, point2.X) &&
                        endX - tol <= Math.Max(point1.X, point2.X) &&
                        endY + tol >= Math.Min(point1.Y, point2.Y) &&
                        endY - tol <= Math.Max(point1.Y, point2.Y))
                    {
                        double temp = angle - StartAngle;
                        if (temp < 0) temp += 2 * Math.PI;

                        double param = (this.Radius * temp) / (this.Length);

                        returnList.Add(new IntersectionResult(param, intersection2, IntersectionType.WITHIN));
                    }
                    return returnList;
                }
            }
        }

    }
}
