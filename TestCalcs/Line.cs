using MWGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace TestCalcs
{
    public class Line : GeometryBase
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
                return new List<IntersectionResult> { new IntersectionResult(param, inter, IntersectionType.WITHIN) };
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

    public class MWGeometry2
    {
        /// <summary>
        /// Returns transformation matrix setting 2D plane in 3D space to XY plane
        /// </summary>
        /// <param name="pt1 at origin"></param>
        /// <param name="pt2 on x axis"></param>
        /// <param name="pt3 on xy plane"></param>
        /// <returns></returns>
        public static Matrix4x4 TransformTo2DPlane(MWPoint3D pt1, MWPoint3D pt2, MWPoint3D pt3)
        {
            //Point4D p1 = new Point4D(pt1.X, pt1.Y, pt1.Z, 1);
            //Point4D p2 = new Point4D(pt2.X, pt2.Y, pt2.Z, 1);
            //Point4D p3 = new Point4D(pt3.X, pt3.Y, pt3.Z, 1);

            MWVector3D xAxis = new MWVector3D(pt2.X - pt1.X, pt2.Y - pt1.Y, pt2.Z - pt1.Z);
            MWVector3D hAxis = new MWVector3D(pt3.X - pt1.X, pt3.Y - pt1.Y, pt3.Z - pt1.Z);
            MWVector3D zAxis = CrossProduct(xAxis, hAxis);
            MWVector3D yAxis = CrossProduct(zAxis, xAxis);

            xAxis = xAxis.Normalised();
            yAxis = yAxis.Normalised();
            zAxis = zAxis.Normalised();

            Matrix4x4 trans = new Matrix4x4((float)xAxis.X, (float)xAxis.Y, (float)xAxis.Z, 0,
                                          (float)yAxis.X, (float)yAxis.Y, (float)yAxis.Z, 0,
                                          (float)zAxis.X, (float)zAxis.Y, (float)zAxis.Z, 0,
                                          (float)pt1.X, (float)pt1.Y, (float)pt1.Z, 1);

            //Matrix4x4 trans = new Matrix4x4((float)xAxis.X, (float)yAxis.X, (float)zAxis.X, 0,
            //                              (float)xAxis.Y, (float)yAxis.Y, (float)zAxis.Y, 0,
            //                              (float)xAxis.Z, (float)yAxis.Z, (float)zAxis.Z, 0,
            //                              (float)pt1.X, (float)pt1.Y, (float)pt1.Z, 1);
            Matrix4x4 returnMatrix;
            Matrix4x4.Invert(trans, out returnMatrix);

            Matrix3D trans2 = new Matrix3D(xAxis.X, xAxis.Y, xAxis.Z, 0,
                              yAxis.X, yAxis.Y, yAxis.Z, 0,
                              zAxis.X, zAxis.Y, zAxis.Z, 0,
                              pt1.X, pt1.Y, pt1.Z, 1);
            trans2.Invert();

            return trans;
        }

        public static MWPoint3D TransformedPoint(MWPoint3D pt, Matrix4x4 matrix)
        {
            double newX = matrix.M11 * pt.X + matrix.M21 * pt.Y + matrix.M31 * pt.Z + matrix.M41;
            double newY = matrix.M12 * pt.X + matrix.M22 * pt.Y + matrix.M32 * pt.Z + matrix.M42;
            double newZ = matrix.M13 * pt.X + matrix.M23 * pt.Y + matrix.M33 * pt.Z + matrix.M43;
            return new MWPoint3D(newX, newY, newZ);

        }

        //public static MWPoint3D TransformedPoint(MWPoint3D pt, Matrix3D matrix)
        //{
        //    double newX = matrix.M11 * pt.X + matrix.M12 * pt.Y + matrix.M13 * pt.Z + matrix.M14;
        //    double newY = matrix.M21 * pt.X + matrix.M22 * pt.Y + matrix.M23 * pt.Z + matrix.M24;
        //    double newZ = matrix.M31 * pt.X + matrix.M32 * pt.Y + matrix.M33 * pt.Z + matrix.M34;
        //    return new MWPoint3D(newX, newY, newZ);

        //}

        public static MWVector3D CrossProduct(MWVector3D v1, MWVector3D v2)
        {
            double x, y, z;
            x = v1.Y * v2.Z - v2.Y * v1.Z;
            y = (v1.X * v2.Z - v2.X * v1.Z) * -1;
            z = v1.X * v2.Y - v2.X * v1.Y;

            var rtnvector = new MWVector3D(x, y, z);
            return rtnvector;
        }

        public static Tuple<MWVector3D, MWVector3D, MWVector3D> LocalCoordSystemFromLinePoints(MWPoint3D point1, MWPoint3D point2)
        {
            MWVector3D normal = point2 - point1;
            MWVector3D newX = CrossProduct(normal, new MWVector3D(0,0,1));
            MWVector3D newY = CrossProduct(normal, newX);

            var newX2 = newX.Normalised();
            var newY2 = newY.Normalised();
            var newZ2 = normal.Normalised();

            return new Tuple<MWVector3D, MWVector3D, MWVector3D>(newX2, newY2, newZ2);
        }
    }
}
