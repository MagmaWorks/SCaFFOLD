using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CalcCore;

namespace EssentialCalcs
{
    [CalcName("STM node check")]
    public class NodeCheck : CalcCore.CalcBase
    {
        CalcDouble side1Length;
        CalcDouble side1Force;
        CalcDouble side2Length;
        CalcDouble side2Force;
        CalcDouble side3Length;
        CalcDouble side3Force;
        CalcListOfDoubleArrays p3Coords;
        CalcDouble nodeWidth;
        CalcDouble face1Stress;
        CalcDouble face2Stress;
        CalcDouble face3Stress;
        CalcDouble cylinderStr;
        CalcDouble reductionFactor;
        CalcDouble cccNodeFactor;
        CalcDouble cctNodeFactor;
        CalcDouble cttNodeFactor;
        CalcDouble alphaCC;
        CalcDouble concPartialFactor;
        CalcDouble concreteCompStr;
        CalcDouble nodeCompStrength;
        string nodeType = "CCC";
        Vector2 p1;
        Vector2 p2;
        Vector2 p3;

        Formula exp1;
        Formula exp2;
        Formula exp3;

        public NodeCheck()
        {
            side1Length = inputValues.CreateDoubleCalcValue("Side 1 Length", "l_1", "mm", 100);
            side1Force = inputValues.CreateDoubleCalcValue("Side 1 force", "F_1", "kN", 100);
            side2Length = inputValues.CreateDoubleCalcValue("Side 2 Length", "l_2", "mm", 100);
            side2Force = inputValues.CreateDoubleCalcValue("Side 2 force", "F_2", "kN", 100);
            side3Length = inputValues.CreateDoubleCalcValue("Side 3 Length", "l_3", "mm", 100);
            side3Force = inputValues.CreateDoubleCalcValue("Side 3 force", "F_3", "kN", 100);
            nodeWidth = inputValues.CreateDoubleCalcValue("Node width", "W", "mm", 200);
            cylinderStr = inputValues.CreateDoubleCalcValue("Cylinder strength", "f_{ck}", "N/mm^2", 35);
            reductionFactor = outputValues.CreateDoubleCalcValue("Reduction factor", "v'", "", 1);
            cccNodeFactor = inputValues.CreateDoubleCalcValue("CCC node factor", "k_1", "", 1);
            cctNodeFactor = inputValues.CreateDoubleCalcValue("CCT node factor", "k_2", "", 0.85);
            cttNodeFactor = inputValues.CreateDoubleCalcValue("CTT node factor", "k_3", "", 0.75);
            alphaCC = inputValues.CreateDoubleCalcValue("Coefficient", @"\alpha_{cc}", "", 1);
            concPartialFactor = inputValues.CreateDoubleCalcValue("Partial material factor for concrete", @"\gamma_C", "", 1.15);
            concreteCompStr = inputValues.CreateDoubleCalcValue("Compressive strength of concrete", "f_{cd}", "N/mm2", 0);

            p3Coords = outputValues.CreateCalcListOfDoubleArrays("Point 3", new List<double[]> { new double[] { 0, 0 } });
            face1Stress = outputValues.CreateDoubleCalcValue("Face 1 stress", @"\sigma_1", "N/mm^2", 0);
            face2Stress = outputValues.CreateDoubleCalcValue("Face 1 stress", @"\sigma_2", "N/mm^2", 0);
            face3Stress = outputValues.CreateDoubleCalcValue("Face 1 stress", @"\sigma_3", "N/mm^2", 0);
            nodeCompStrength = outputValues.CreateDoubleCalcValue("Node compressive strength", @"\sigma_{Rd,max}", "", 0);
            exp1 = new Formula();
            exp2 = new Formula();
            exp3 = new Formula();

            UpdateCalc();

        }


        public override List<Formula> GenerateFormulae()
        {
            var returnVal = new List<Formula>()
            {
                //new Formula() { Image = generateImage()[0] },
                new Formula() { Narrative = "Node type: " + nodeType + ". "},
                new Formula() { Ref="cl.6.5.2", Expression= new List<string>{reductionFactor.Symbol + @"= 1 - \frac{" + cylinderStr.Symbol + "}{250} = " + Math.Round(reductionFactor.Value,2).ToString() + reductionFactor.Unit } },
                exp1,
                exp2,
                exp3,
            };
            return returnVal;
        }


        public override void UpdateCalc()
        {
            formulae = null;
            concreteCompStr.Value = alphaCC.Value * cylinderStr.Value / concPartialFactor.Value;
            //clasify node
            int tensionSides = 0;
            if (side1Force.Value < 0)
                tensionSides++;
            if (side2Force.Value < 0)
                tensionSides++;
            if (side3Force.Value < 0)
                tensionSides++;
            switch (tensionSides)
            {
                case 0:
                    nodeType = "CCC";
                    nodeCompStrength.Value = cccNodeFactor.Value * reductionFactor.Value * concreteCompStr.Value;
                    break;
                case 1:
                    nodeType = "CCT";
                    nodeCompStrength.Value = cctNodeFactor.Value * reductionFactor.Value * concreteCompStr.Value;
                    break;
                case 2:
                    nodeType = "CTT";
                    nodeCompStrength.Value = cttNodeFactor.Value * reductionFactor.Value * concreteCompStr.Value;
                    break;
                default:
                    break;
            }

            reductionFactor.Value = 1 - cylinderStr.Value / 250;

            p1 = new Vector2(0, 0);
            p2 = new Vector2((float)side1Length.Value, 0);
            var inter = circleCircleIntersection(p2, p1, (float)side2Length.Value, (float)side3Length.Value);
            if (inter.Count > 0)
            {
                p3Coords.Value = new List<double[]> { new double[] { inter[0].X, inter[0].Y } };
                p3 = inter[0];
            }
            else
                p3Coords.Value = new List<double[]> { new double[] { 0, 0 } };


            exp1 = new Formula();
            face1Stress.Value = 1000 * side1Force.Value / (nodeWidth.Value * side1Length.Value);
            exp1.Expression = new List<string>() { face1Stress.Symbol + @"=\frac{" + side1Force.Symbol + @"}{" + nodeWidth.Symbol + side1Length.Symbol + "}=" + Math.Round(face1Stress.Value, 2) + face1Stress.Unit };
            if (face1Stress.Value < nodeCompStrength.Value)
            {
                face1Stress.Status = CalcStatus.PASS;
                exp1.Conclusion = "OK";
                exp1.Status = CalcStatus.PASS;
            }
            else
            {
                face1Stress.Status = CalcStatus.FAIL;
                exp1.Conclusion = "Face stress too high";
                exp1.Status = CalcStatus.FAIL;
            }

            exp2 = new Formula();
            face2Stress.Value = 1000 * side2Force.Value / (nodeWidth.Value * side2Length.Value);
            exp2.Expression = new List<string>() { face2Stress.Symbol + @"=\frac{" + side2Force.Symbol + @"}{" + nodeWidth.Symbol + side2Length.Symbol + "}=" + Math.Round(face2Stress.Value, 2) + face2Stress.Unit };
            if (face2Stress.Value < nodeCompStrength.Value)
            {
                face2Stress.Status = CalcStatus.PASS;
                exp2.Conclusion = "OK";
                exp2.Status = CalcStatus.PASS;
            }
            else
            {
                face2Stress.Status = CalcStatus.FAIL;
                exp2.Conclusion = "Face stress too high";
                exp2.Status = CalcStatus.FAIL;
            }

            exp3 = new Formula();
            face3Stress.Value = 1000 * side3Force.Value / (nodeWidth.Value * side3Length.Value);
            exp3.Expression = new List<string>() { face3Stress.Symbol + @"=\frac{" + side3Force.Symbol + @"}{" + nodeWidth.Symbol + side3Length.Symbol + "}=" + Math.Round(face3Stress.Value, 2) + face3Stress.Unit };
            if (face3Stress.Value < nodeCompStrength.Value)
            {
                face3Stress.Status = CalcStatus.PASS;
                exp3.Conclusion = "OK";
                exp3.Status = CalcStatus.PASS;
            }
            else
            {
                face3Stress.Status = CalcStatus.FAIL;
                exp3.Conclusion = "Face stress too high";
                exp3.Status = CalcStatus.FAIL;
            }


        }

        private static List<Vector2> circleCircleIntersection(Vector2 c1, Vector2 c2, float rad1, float rad2)
        {
            List<Vector2> rLp = new List<Vector2>();
            float d = Vector2.Distance(c1, c2);

            if (d > (rad1 + rad2))
                return rLp;
            else if (d == 0 && rad1 == rad2)
                return rLp;
            else if ((d + Math.Min(rad1, rad2)) < Math.Max(rad1, rad2))
                return rLp;
            else
            {
                float a = (rad1 * rad1 - rad2 * rad2 + d * d) / (2 * d);
                float h = (float)Math.Sqrt(rad1 * rad1 - a * a);

                Vector2 p2 = new Vector2((float)(c1.X + (a * (c2.X - c1.X)) / d), (float)(c1.Y + a * (c2.Y - c1.Y) / d));

                Vector2 i1 = new Vector2((float)(p2.X + (h * (c2.Y - c1.Y)) / d), (float)(p2.Y - (h * (c2.X - c1.X)) / d));
                Vector2 i2 = new Vector2((float)(p2.X - (h * (c2.Y - c1.Y)) / d), (float)(p2.Y + (h * (c2.X - c1.X)) / d));

                if (d == (rad1 + rad2))
                    rLp.Add(i1);
                else
                {
                    rLp.Add(i1);
                    rLp.Add(i2);
                }

                return rLp;
            }

        }

        //private List<BitmapSource> generateImage()
        //{
        //    var testImage = new RenderTargetBitmap(1000, 1000, 96, 96, PixelFormats.Pbgra32);
        //    var visual = new DrawingVisual();
        //    using (var r = visual.RenderOpen())
        //    {
        //        var nodeEdgeGeometry = new GeometryGroup();
        //        var forceLinesGeometry = new GeometryGroup();
        //        var forceAnnotation = new GeometryGroup();

        //        nodeEdgeGeometry.Children.Add(new LineGeometry(new System.Windows.Point(p1.X, p1.Y), new System.Windows.Point(p2.X, p2.Y)));
        //        nodeEdgeGeometry.Children.Add(new LineGeometry(new System.Windows.Point(p2.X, p2.Y), new System.Windows.Point(p3.X, p3.Y)));
        //        nodeEdgeGeometry.Children.Add(new LineGeometry(new System.Windows.Point(p3.X, p3.Y), new System.Windows.Point(p1.X, p1.Y)));

        //        Vector2 perp1 = Vector2.Normalize(new Vector2(p2.Y - p1.Y, -(p2.X - p1.X)));
        //        Vector2 perp2 = Vector2.Normalize(new Vector2(p3.Y - p2.Y, -(p3.X - p2.X)));
        //        Vector2 perp3 = Vector2.Normalize(new Vector2(p1.Y - p3.Y, -(p1.X - p3.X)));

        //        Vector2 mid1 = (p1 + p2) / 2;
        //        Vector2 mid2 = (p2 + p3) / 2;
        //        Vector2 mid3 = (p3 + p1) / 2;

        //        forceLinesGeometry.Children.Add(new LineGeometry(new System.Windows.Point(mid1.X, mid1.Y), new System.Windows.Point((mid1 + perp1 * (float)side1Force.Value).X, (mid1 + perp1 * (float)side1Force.Value).Y)));
        //        forceLinesGeometry.Children.Add(new LineGeometry(new System.Windows.Point(mid2.X, mid2.Y), new System.Windows.Point((mid2 + perp2 * (float)side2Force.Value).X, (mid2 + perp2 * (float)side2Force.Value).Y)));
        //        forceLinesGeometry.Children.Add(new LineGeometry(new System.Windows.Point(mid3.X, mid3.Y), new System.Windows.Point((mid3 + perp3 * (float)side3Force.Value).X, (mid3 + perp3 * (float)side3Force.Value).Y)));


        //        forceAnnotation.Children.Add(gettext("F1=" + side1Force.ValueAsString + side1Force.Unit, new System.Windows.Point((mid1 + perp1 * (float)side1Force.Value).X, (mid1 + perp1 * (float)side1Force.Value).Y)));
        //        forceAnnotation.Children.Add(gettext("F2=" + side2Force.ValueAsString + side2Force.Unit, new System.Windows.Point((mid2 + perp2 * (float)side2Force.Value).X, (mid2 + perp2 * (float)side2Force.Value).Y)));
        //        forceAnnotation.Children.Add(gettext("F3=" + side3Force.ValueAsString + side3Force.Unit, new System.Windows.Point((mid3 + perp3 * (float)side3Force.Value).X, (mid3 + perp3 * (float)side3Force.Value).Y)));


        //        GeometryGroup allGeometry = new GeometryGroup();
        //        allGeometry.Children = new GeometryCollection(new List<Geometry> { nodeEdgeGeometry, forceLinesGeometry, forceAnnotation });
        //        var newBounds = allGeometry.Bounds;
        //        double maxX = Math.Max(Math.Abs(newBounds.X), Math.Abs(newBounds.BottomRight.X));
        //        double maxY = Math.Max(Math.Abs(newBounds.Y), Math.Abs(newBounds.BottomRight.Y));

        //        double scale = 900 / (Math.Max(2 * maxX, 2 * maxY));
        //        var scaleTransform = new ScaleTransform(scale, -scale);
        //        var transX = 50 + scale * -newBounds.X;
        //        var transY = 50 + scale * newBounds.BottomRight.Y;
        //        var transTransform = new TranslateTransform(transX, transY);
        //        var transGroup = new TransformGroup();
        //        transGroup.Children.Add(scaleTransform);
        //        transGroup.Children.Add(transTransform);

        //        nodeEdgeGeometry.Transform = transGroup;
        //        forceLinesGeometry.Transform = transGroup;
        //        forceAnnotation.Transform = transGroup;

        //        var dashedLine = new Pen(Brushes.Gray, 2) { DashStyle = new DashStyle(new List<double> { 20, 5, 5, 5 }, 0) };
        //        var dashedLine2 = new Pen(Brushes.Gray, 2) { DashStyle = new DashStyle(new List<double> { 10, 5 }, 0) };

        //        r.DrawGeometry(Brushes.LightGreen, new Pen(Brushes.Green, 5), nodeEdgeGeometry);
        //        r.DrawGeometry(Brushes.Red, new Pen(Brushes.Red, 5), forceLinesGeometry);
        //        r.DrawGeometry(Brushes.Black, new Pen(Brushes.Black, 1), forceAnnotation);
        //    }

        //    testImage.Render(visual);

        //    return new List<BitmapSource> { testImage };
        //}

        //private Geometry gettext(string text, Point pos)
        //{
        //    FormattedText returnText = new FormattedText(text,
        //        CultureInfo.CurrentCulture,
        //        FlowDirection.LeftToRight,
        //        new Typeface(new FontFamily("Franklin Gothic Book"), FontStyles.Normal, FontWeights.Light, FontStretches.Normal),
        //        12,
        //        Brushes.Black);
        //    var pos1 = new Point(pos.X, pos.Y - returnText.Height / 2);
        //    Geometry geometry = returnText.BuildGeometry(new Point(0, 0));
        //    var trans = new TransformGroup();
        //    trans.Children.Add(new ScaleTransform(1, -1));
        //    trans.Children.Add(new TranslateTransform(pos.X, pos.Y));
        //    geometry.Transform = trans;
        //    return geometry;
        //}

    }
}
