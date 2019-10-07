using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class CalcCorePoint3D
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        private CalcCorePoint3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static CalcCorePoint3D point3DByCoordinates(double x, double y, double z)
        {
            return new CalcCorePoint3D(x, y, z);
        }
    }
}
