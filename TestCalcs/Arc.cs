using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TestCalcs
{
    public class Arc : GeometryBase
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
                    diffAngle += 2 * Math.PI;
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
