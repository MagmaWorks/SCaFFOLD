using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcCore
{
    public class CalcCoreMesh
    {
        public List<int[]> MeshIndices { get; private set; }
        public List<CalcCoreNode> Nodes { get; private set; }
        public double Opacity { get; set; }

        public CalcCoreMesh()
        {
            MeshIndices = new List<int[]>();
            Nodes = new List<CalcCoreNode>();
            Opacity = 1;
        }

        public void addNode(double x, double y, double z, CalcCorePoint2D textureCoordinates)
        {
            Nodes.Add(new CalcCoreNode(CalcCorePoint3D.point3DByCoordinates(x, y, z), textureCoordinates));
        }

        public void setIndices(List<int[]> indices)
        {
            MeshIndices = new List<int[]>();
            foreach (var item in indices)
            {
                if (item.Length == 3) MeshIndices.Add(item);
            }
        }
    }
}
