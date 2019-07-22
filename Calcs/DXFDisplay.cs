using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using netDxf;

namespace Calcs
{
    public class DXFDisplay
    {
        public static List<DXFDrawData> ReadDXF(netDxf.DxfDocument dxf)
        {
            var returnGroup = new List<DXFDrawData>();
            double thicknessScale = 0.1;
            foreach (var layer in dxf.Layers)
            {
                var stroke = new SolidColorBrush(Color.FromRgb(layer.Color.R, layer.Color.G, layer.Color.B));
                var layerStrokeThickness = Math.Max((double)layer.Lineweight * thicknessScale, 2); 
                foreach (var line in dxf.Lines)
                {
                    if (line.Layer == layer)
                    {
                        double strokeThickness = 0;
                        if (line.Lineweight == Lineweight.ByLayer)
                        {
                            strokeThickness = layerStrokeThickness;
                        }
                        else
                        {
                            strokeThickness = Math.Max((double)line.Lineweight * thicknessScale, 2);
                        }
                        returnGroup.Add(new DXFDrawData(stroke, strokeThickness, new LineGeometry(new System.Windows.Point(line.StartPoint.X, line.StartPoint.Y), new System.Windows.Point(line.EndPoint.X, line.EndPoint.Y))));
                    }
                }
                foreach (var arc in dxf.Arcs)
                {
                    double x = arc.Center.X;
                    double y = arc.Center.Y;
                    double start = arc.StartAngle;
                    double end = arc.EndAngle;
                    double r = arc.Radius;
                    var endPoint = e(arc.Normal, arc.Center, (float)arc.Radius, (float)(arc.EndAngle * Math.PI / 180));
                    var startPoint = e(arc.Normal, arc.Center, (float)arc.Radius, (float)(arc.StartAngle * Math.PI / 180));


                    ArcSegment newarc = new ArcSegment(endPoint, new Size(r,r), (end - start)*Math.PI/180, false, SweepDirection.Clockwise, true);
                    PathFigure fig = new PathFigure(startPoint, new List<PathSegment> { newarc }, false);
                    PathGeometry path = new PathGeometry(new List<PathFigure> { fig });
                    if (arc.Layer == layer)
                    {
                        double strokeThickness = 0;
                        if (arc.Lineweight == Lineweight.ByLayer)
                        {
                            strokeThickness = layerStrokeThickness;
                        }
                        else
                        {
                            strokeThickness = Math.Max((double)arc.Lineweight * thicknessScale, 2);
                        }
                        returnGroup.Add(new DXFDrawData(stroke, strokeThickness, path));
                    }


                    //returnGroup.Children.Add(new ArcSegment());
                }

                foreach (var circle in dxf.Circles)
                {
                    if (circle.Layer == layer)
                    {
                        double strokeThickness = 0;
                        if (circle.Lineweight == Lineweight.ByLayer)
                        {
                            strokeThickness = layerStrokeThickness;
                        }
                        else
                        {
                            strokeThickness = Math.Max((double)circle.Lineweight * thicknessScale, 2);
                        }
                        returnGroup.Add(new DXFDrawData(stroke, strokeThickness, new EllipseGeometry(new Point(circle.Center.X, circle.Center.Y), circle.Radius, circle.Radius)));
                    }
                }
            }


            return returnGroup;
        }

        static Point e(Vector3 normal, Vector3 centre, float r, float a)
        {
            var nx = (float)normal.X;
            var ny = (float)normal.Y;
            var nz = (float)normal.Z;
            var cx = (float)centre.X;
            var cy = (float)centre.Y;
            var cz = (float)centre.Z;

            // Only needed if normal vector (nx, ny, nz) is not already normalized.
            float s = 1.0f / (nx * nx + ny * ny + nz * nz);
            float v3x = s * nx;
            float v3y = s * ny;
            float v3z = s * nz;

            // Calculate v1.
            s = 1.0f / (v3x * v3x + v3z * v3z);
            float v1x = s * v3z;
            float v1y = 0.0f;
            float v1z = s * -v3x;

            // Calculate v2 as cross product of v3 and v1.
            // Since v1y is 0, it could be removed from the following calculations. Keeping it for consistency.
            float v2x = v3y * v1z - v3z * v1y;
            float v2y = v3z * v1x - v3x * v1z;
            float v2z = v3x * v1y - v3y * v1x;

            // For each circle point.
            var px = cx + r * (v1x * Math.Cos(a) + v2x * Math.Sin(a));
            var py = cy + r * (v1y * Math.Cos(a) + v2y * Math.Sin(a));
            var pz = cz + r * (v1z * Math.Cos(a) + v2z * Math.Sin(a));

            return new Point(px, py);
        }
    }
}
