using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class CalcCoreNode
    {
        public CalcCorePoint3D Point { get; private set; }
        public CalcCorePoint2D TextureCoordinates { get; private set; }

        public CalcCoreNode(CalcCorePoint3D point, CalcCorePoint2D textureCoords)
        {
            Point = point;
            TextureCoordinates = textureCoords;
        }
    }
}
